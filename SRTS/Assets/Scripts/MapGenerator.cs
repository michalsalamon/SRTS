using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Coord mapSize;
    public Coord MapSize
    {
        get { return mapSize; }
    }
    [SerializeField] private Transform baseTilePrefab;

    [SerializeField] private float tileSize = 1;
    [Range(0, 1)] [SerializeField] private float outlinePercent = 0.1f;

    private List<Coord> allMapTileCoords;
    private Transform[,] mapTiles;

    public Transform[,] unitPositions;

    private void Awake()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        allMapTileCoords = new List<Coord>();
        mapTiles = new Transform[mapSize.x, mapSize.y];
        unitPositions = new Transform[MapSize.x, MapSize.y];

        //box collider for map floor
        GetComponent<BoxCollider>().size = new Vector3(mapSize.x * tileSize, 0.01f, mapSize.y * tileSize);

        //generate list of all map tiles coordinates
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allMapTileCoords.Add(new Coord(x, y));
            }
        }

        //creating game object holding all map elements
        string holderName = "Whole Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        //spawn base tails
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePos = CoordToPosition(x, y);
                Transform newTile = Instantiate(baseTilePrefab, tilePos, Quaternion.Euler(Vector3.right * 90));
                newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newTile.transform.parent = mapHolder;
                mapTiles[x, y] = newTile;
            }
        }
    }

    public Vector3 CoordToPosition(int f_x, int f_y)
    {
        return new Vector3(-mapSize.x / 2f + 0.5f + f_x, 0, -mapSize.y / 2f + 0.5f + f_y) * tileSize;
    }

    public Transform PositionToTile(Vector3 f_position)
    {
        Coord XY = PositionToCoord(f_position);
        XY.x = Mathf.Clamp(XY.x, 0, mapTiles.GetLength(0) - 1);
        XY.y = Mathf.Clamp(XY.y, 0, mapTiles.GetLength(1) - 1);
        return mapTiles[XY.x, XY.y];
    }

    public Coord PositionToCoord(Vector3 f_position)
    {
        int x = Mathf.RoundToInt(f_position.x / tileSize + (mapSize.x - 1) / 2f);
        int y = Mathf.RoundToInt(f_position.z / tileSize + (mapSize.y - 1) / 2f);
        return new Coord(x, y);
    }

    public void UpdateUnitPosition(Transform unit)
    {
        Coord XY;
        //save new position of unit
        XY = PositionToCoord(unit.position);
        unitPositions[XY.x, XY.y] = unit;
    }

    public void UpdateUnitPosition(Transform unit, Vector3 previousPosition)
    {
        Coord XY;
        if (previousPosition != null)
        {
            //remove previous position of unit
            XY = PositionToCoord(previousPosition);
            unitPositions[XY.x, XY.y] = null;
        }
        //save new position of unit
        XY = PositionToCoord(unit.position);
        unitPositions[XY.x, XY.y] = unit;
    }

    public Transform CheckIfUnitIsOnCoord(Vector3 f_position)
    {
        Coord XY = PositionToCoord(f_position);
        return unitPositions[XY.x, XY.y];
    }

    [System.Serializable]
    public class Coord
    {
        public int x;
        public int y;

        public Coord(int f_x, int f_y)
        {
            x = f_x;
            y = f_y;
        }
    }
}
