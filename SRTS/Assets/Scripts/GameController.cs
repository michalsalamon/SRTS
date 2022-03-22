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
    private List<Color> dragSelectTilesOriginalColor = new List<Color>();

    Color orginalTileColor = Color.white;
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

        //get mouse point
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float distance = float.MaxValue;
        if (mapCollider.Raycast(ray, out hit, distance))
        {
            point = hit.point;
            Debug.DrawLine(ray.origin, point, Color.red);
        }

        HighLightTile();

        if (mouseKey0Down)
        {
            OnClickMouse0();
            mouseStartHoldPoint = point;
        }

        if (mouseKey0Hold)
        {
            ManageDragSelecting();
        }

    }
    
    private void HighLightTile()
    {
        //get pointed tile
        Transform tilePointing = map.PositionToTile(point);

        if (previousTilePoint != tilePointing)
        {
            if (previousTilePoint != null)
            {
                previousTilePoint.GetComponent<Renderer>().material.color = orginalTileColor;
            }
            orginalTileColor = tilePointing.GetComponent<Renderer>().material.color;
            tilePointing.GetComponent<Renderer>().material.color = hightLightedTileColor;
            previousTilePoint = tilePointing;
        }       
    }

    private void ManageDragSelecting()
    {
        dragSelectTiles.Clear();
        dragSelectTilesOriginalColor.Clear();

        int start_x = map.PositionToCoord(mouseStartHoldPoint).x;
        int start_y = map.PositionToCoord(mouseStartHoldPoint).y;
        int current_x = map.PositionToCoord(point).x;
        int current_y = map.PositionToCoord(point).y;

        for (int x = start_x; x <= current_x; x++)
        {
            for (int y = start_y; y <= current_y; y++)
            {
                dragSelectTiles.Add(map.MapTiles[x, y]);
                Material tileMat = map.MapTiles[x, y].GetComponent<Renderer>().material;
                dragSelectTilesOriginalColor.Add(tileMat.color);
                tileMat.color = hightLightedTileColor;
            }
        }
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
