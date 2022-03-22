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
    private Vector3 mouseStartHoldPoint;

    private List<Transform> dragSelectTiles = new List<Transform>();

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
            OnClickMouse0();
            mouseStartHoldPoint = point;
        }

        if (mouseKey0Hold)
        {
            ManageDragSelecting();
        }
        if (mouseKey0UP)
        {
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
                dragSelectTiles.Add(map.SelectTiles[x, y]);
                Material tileMat = map.SelectTiles[x, y].GetComponent<Renderer>().material;
                tileMat.color = hightLightedTileColor;
            }
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

    private void ManageMouse0Click()
    {
        //if unit is on the tile
        if (map.CheckIfUnitIsOnCoord(point))
        {
            Debug.Log("Unit is on this tile");
            Transform unit = map.CheckIfUnitIsOnCoord(point);
            unit.GetComponent<BaseUnit>().isSelected = true;

        }
    }

    public event System.Action OnClickMouse0;
}
