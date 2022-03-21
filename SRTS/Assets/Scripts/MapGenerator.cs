using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Coord mapSize;
    [SerializeField] private Transform baseTilePrefab;

    [SerializeField] private float tileSize = 1;
    [Range (0,1)][SerializeField] private float outlinePercent = 0.1f;

    private List<Coord> allMapTileCoords;
    private Transform[,] mapTiles;

    private void Start()
    {
        mapTiles = new Transform[mapSize.x, mapSize.y];
    }

    public void GenerateMap()
    {
        //generate list of all map tiles coordinates
        allMapTileCoords = new List<Coord>();
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

    private Vector3 CoordToPosition(int f_x, int f_y)
    {
        return new Vector3(-mapSize.x / 2f + 0.5f + f_x, 0, -mapSize.y / 2f + 0.5f + f_y) * tileSize;
    }

    [System.Serializable]
    public struct Coord
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
