using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private MapGenerator map;
    private BoxCollider mapCollider;
    private Camera viewCamera;

    private Vector3 point;
    private bool mouseKey0 = false;

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
        mouseKey0 = Input.GetButtonDown("Fire1");

        //get mouse point
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float distance = float.MaxValue;
        if (mapCollider.Raycast(ray, out hit, distance))
        {
            point = hit.point;
            Debug.DrawLine(ray.origin, point, Color.red);
        }

        HighLightPointedTile();
        if (mouseKey0)
        {
            OnClickMouse0();
            ManageMouse0Click();
        }

    }
    
    private void HighLightPointedTile()
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
