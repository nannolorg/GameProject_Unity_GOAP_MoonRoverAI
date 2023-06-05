using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_ReturnHome : Goal_Base
{
    [SerializeField] int ItemLimit = 5;

    public override void PreTick()
    {
        if (IsActive)
        {
            CanRun = EntityInfo.Inventory[EntityInfo.ItemName.Samples] > ItemLimit;
        }
        else
        {
            CanRun = EntityInfo.Inventory[EntityInfo.ItemName.Samples] < ItemLimit;
        }

        Priority = MaxPriority;
    }
}
