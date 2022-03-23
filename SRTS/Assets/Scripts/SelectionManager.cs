using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private GameManager gameManager;
    private MapGenerator map;

    private Color hightLightedTileColor = new Color(0, 1, 0, 0.5f);
    private List<Transform> hightLightedTiles = new List<Transform>();
    private List<Transform> highLightedUnits = new List<Transform>();

    private Vector3 mouse0DownPoint;

    //events
    public event System.Action OnMouse0UpEvent;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        map = FindObjectOfType<MapGenerator>();
    }

    private void Update()
    {
        //if there is no input form mouse 0 button hight light pointed tile
        if (!(gameManager.MouseKey0Down || gameManager.MouseKey0Hold || gameManager.MouseKey0Up))
        {
            ClearHighligtedTiles();
            MapGenerator.Coord temp = map.PositionToCoord(gameManager.Point);
            hightLightedTiles.Add(HighLightingTile(temp, hightLightedTileColor));
        }

        if (gameManager.MouseKey0Down)
        {
            mouse0DownPoint = gameManager.Point;
        }

        if (gameManager.MouseKey0Hold)
        {
            //manage drag selection
            ManageDragSelection();
        }

        if (gameManager.MouseKey0Up)
        {
            //event for deselcting unit
            OnMouse0UpEvent();
            //manage selecting unit
            SelectUnit();
        }
    }

    private Transform HighLightingTile(MapGenerator.Coord coord, Color color)
    {
        map.ChangeSelectTileColor(coord, color);
        return map.MapTiles[coord.x, coord.y];
    }

    private void ClearHighligtedTiles()
    {
        for (int i = 0; i < hightLightedTiles.Count; i++)
        {
            map.ChangeSelectTileColor(map.PositionToCoord(hightLightedTiles[i].position), map.SelectTileOriginalColor);
        }
        hightLightedTiles.Clear();
    }

    private void ManageDragSelection()
    {
        ClearHighligtedTiles();

        MapGenerator.Coord start_XY = map.PositionToCoord(mouse0DownPoint);
        MapGenerator.Coord current_XY = map.PositionToCoord(gameManager.Point);
        int temp;

        if (start_XY.x > current_XY.x)
        {
            temp = start_XY.x;
            start_XY.x = current_XY.x;
            current_XY.x = temp;
        }
        if (start_XY.y > current_XY.y)
        {
            temp = start_XY.y;
            start_XY.y = current_XY.y;
            current_XY.y = temp;
        }


        for (int x = start_XY.x; x <= current_XY.x; x++)
        {
            for (int y = start_XY.y; y <= current_XY.y; y++)
            {
                MapGenerator.Coord coord = new MapGenerator.Coord(x, y);
                hightLightedTiles.Add(HighLightingTile(coord, hightLightedTileColor));
                if (map.CheckIfUnitIsOnCoord(coord))
                {
                    highLightedUnits.Add(map.CheckIfUnitIsOnCoord(coord));
                }
            }
        }
    }

    private void SelectUnit()
    {
        for (int i = 0; i < highLightedUnits.Count; i++)
        {
            highLightedUnits[i].GetComponent<BaseUnit>().SelectUnit();
        }
        highLightedUnits.Clear();
    }

}
