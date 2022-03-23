using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    protected MapGenerator map;
    protected SelectionManager selecttionManager;
    protected GameManager gameManager;
    [SerializeField] private GameObject selectionMarker;

    public int owner;
    public bool isSelected = false;

    private void Start()
    {
        map = FindObjectOfType<MapGenerator>();
        gameManager = FindObjectOfType<GameManager>();
        selecttionManager = FindObjectOfType<SelectionManager>();

        map.UpdateUnitPosition(transform);

        selectionMarker.SetActive(false);

        selecttionManager.OnMouse0UpEvent += DeselectUnit;
        gameManager.OnMouse1DownEvent += OrderManage;

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

    }
}
