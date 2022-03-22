using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    private MapGenerator map;
    protected GameController gameController;
    [SerializeField] private GameObject selectionMarker;

    public int owner;
    public bool isSelected = false;

    private void Start()
    {
        selectionMarker.SetActive(false);
        map = FindObjectOfType<MapGenerator>();
        gameController = FindObjectOfType<GameController>();
        map.UpdateUnitPosition(transform);

        gameController.OnMouse0Up += DeselectUnit;
        gameController.OnMouse1Down += OrderManage;

        GetToGrid();
    }

    private void GetToGrid()
    {
        transform.position = map.CoordToPosition(map.PositionToCoord(transform.position).x, map.PositionToCoord(transform.position).y) + Vector3.up * 0.5f;
    }

    public void SelectUnit()
    {
        isSelected = true;
        selectionMarker.SetActive(isSelected);
    }

    private void DeselectUnit()
    {
        isSelected = false;
        selectionMarker.SetActive(isSelected);
    }

    public virtual void OrderManage()
    {
        //Debug.Log("Order Click");
    }
}
