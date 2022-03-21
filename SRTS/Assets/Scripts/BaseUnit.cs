using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    private MapGenerator map;
    private GameController gameController;

    public int owner;
    public bool isSelected = false;

    private void Start()
    {
        map = FindObjectOfType<MapGenerator>();
        gameController = FindObjectOfType<GameController>();
        map.UpdateUnitPosition(transform);

        gameController.OnClickMouse0 += DeselectUnit;
    }

    private void DeselectUnit()
    {
        isSelected = false;
    }

}
