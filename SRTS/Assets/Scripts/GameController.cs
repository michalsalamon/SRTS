using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private MapGenerator map;
    private BoxCollider mapCollider;
    private Camera viewCamera;

    private Vector3 point;
    private bool mouseKey0Down = false;
    private bool mouseKey0Hold = false;
    private bool mouseKey0UP = false;
    private bool mouseKey1Down = false;

    private Vector3 mouseStartHoldPoint;

    private List<Transform> dragSelectTiles = new List<Transform>();

    private List<Transform> selectedUnits = new List<Transform>();

    Color orginalTileColor = Color.black;
    Transform previousTilePoint;
    private Color hightLightedTileColor = new Color(0, 1, 0, 0.5f);

    private void Start()
    {
        viewCamera = Camera.main;
        map = FindObjectOfType<MapGenerator>();
        mapCollider = map.GetComponent<BoxCollider>();
    }

    void Update()
    {
        //get mouse keys
        mouseKey0Down = Input.GetButtonDown("Fire1");
        mouseKey0Hold = Input.GetButton("Fire1");
        mouseKey0UP = Input.GetButtonUp("Fire1");

        mouseKey1Down = Input.GetButtonDown("Fire2");

        //get mouse point
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float distance = float.MaxValue;
        if (mapCollider.Raycast(ray, out hit, distance))
        {
            point = hit.point;
            Debug.DrawLine(ray.origin, point, Color.red);
        }


        if (mouseKey0Down)
        {
            mouseStartHoldPoint = point;
        }

        if (mouseKey0Hold)
        {
            ManageDragSelecting();
        }
        if (mouseKey0UP)
        {
            //deselect units
            OnMouse0Up();
            //select units on highlighted area
            SelectUnits();
            ClearTileSelection();
        }
        if (!mouseKey0Hold)
        {
            if (previousTilePoint != HighLightTile(point) && previousTilePoint != null)
            {
                previousTilePoint.GetComponent<Renderer>().material.color = orginalTileColor;
            }
            previousTilePoint = HighLightTile(point);
        }

        if (mouseKey1Down)
        {
            OnMouse1Down();
        }
    }

    private Transform HighLightTile(Vector3 f_position)
    {
        int x = map.PositionToCoord(f_position).x;
        int y = map.PositionToCoord(f_position).y;
        map.SelectTiles[x,y].GetComponent<Renderer>().material.color = hightLightedTileColor;
        return map.SelectTiles[x, y];
    }

    private void ManageDragSelecting()
    {
        //hightlighting tiles
        ClearTileSelection();
        //clear unit list
        selectedUnits.Clear();
        int start_x = map.PositionToCoord(mouseStartHoldPoint).x;
        int start_y = map.PositionToCoord(mouseStartHoldPoint).y;
        int current_x = map.PositionToCoord(point).x;
        int current_y = map.PositionToCoord(point).y;
        int temp;
        
        if (start_x > current_x)
        {
            temp = start_x;
            start_x = current_x;
            current_x = temp;
        }
        if (start_y > current_y)
        {
            temp = start_y;
            start_y = current_y;
            current_y = temp;
        }


        for (int x = start_x; x <= current_x; x++)
        {
            for (int y = start_y; y <= current_y; y++)
            {
                //highlighting tiles on drag area   
                dragSelectTiles.Add(map.SelectTiles[x, y]);
                map.SelectTiles[x, y].GetComponent<Renderer>().material.color = hightLightedTileColor;
                //check if there are units on coords and make list of them
                if (map.UnitPositions[x, y])
                {
                    selectedUnits.Add(map.UnitPositions[x, y]);
                }
            }
        }
    }

    private void SelectUnits()
    {
        for (int i = 0; i < selectedUnits.Count; i ++)
        {
            selectedUnits[i].GetComponent<BaseUnit>().SelectUnit();
        }
    }

    private void ClearTileSelection()
    {
        for (int i = 0; i < dragSelectTiles.Count; i++)
        {
            dragSelectTiles[i].GetComponent<Renderer>().material.color = orginalTileColor;
        }
        dragSelectTiles.Clear();
    }

    public event System.Action OnMouse0Up;
    public event System.Action OnMouse1Down;
}
