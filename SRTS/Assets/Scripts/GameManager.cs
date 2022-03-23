using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapGenerator map;
    private BoxCollider mapCollider;
    private Camera viewCamera;

    //input variables
    private Vector3 point = Vector3.zero;
    public Vector3 Point
    { get { return point; } }
    private bool mouseKey0Down = false;
    public bool MouseKey0Down
    { get { return mouseKey0Down; } }
    private bool mouseKey0Hold = false;
    public bool MouseKey0Hold
    { get { return mouseKey0Hold; } }
    private bool mouseKey0Up = false;
    public bool MouseKey0Up
    { get { return mouseKey0Up; } }
    private bool mouseKey1Down = false;
    public bool MouseKey1Down
    { get { return mouseKey1Down; } }

    //events
    public event System.Action OnMouse1DownEvent;

    private void Start()
    {
        viewCamera = Camera.main;
        map = FindObjectOfType<MapGenerator>();
        mapCollider = map.GetComponent<BoxCollider>();
    }

    void Update()
    {
        //Get Input
        //get mouse keys
        mouseKey0Down = Input.GetButtonDown("Fire1");
        mouseKey0Hold = Input.GetButton("Fire1");
        mouseKey0Up = Input.GetButtonUp("Fire1");

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

        if (mouseKey1Down)
        {
            OnMouse1DownEvent();
        }
    }
}
