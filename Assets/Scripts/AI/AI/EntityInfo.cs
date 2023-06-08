using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    
    public HomeBase Home { get; private set; } = null;
    private Dictionary<WorldResource.EType, int> Inventory;
    private float InventoryMaxWeight = 30f; //Weight in kg
    private float SampleItemWeight = 5f;
    private bool bInventoryFull = false;

    public void SetHome(HomeBase _home)
    {
        Home = _home;
    }

    public bool IsInventoryFull()
    {
        int ItemAmount = 0;
        ICollection<int> InventoryValues = Inventory.Values;
        foreach(int value in InventoryValues)
        {
            ItemAmount += value;
        }


        //calculate the current weight based on a uniform item weight
        float currentWeight = SampleItemWeight * ItemAmount;

        //check if inventory is full/overflow or not
        if (currentWeight >= InventoryMaxWeight)
        {
            bInventoryFull = true;
        } 
        else
        {
            bInventoryFull = false;
        }

        return bInventoryFull;
    }
  
}
