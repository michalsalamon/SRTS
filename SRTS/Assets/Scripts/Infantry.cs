using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : BaseUnit
{
    private void Start()
    {
        
    }

    public override void OrderManage()
    {
        //base.OrderManage();
        //manage orders if unit is selected
        Debug.Log("Here");
        if (isSelected)
        {
            Debug.Log("Order");
        }
    }
}
