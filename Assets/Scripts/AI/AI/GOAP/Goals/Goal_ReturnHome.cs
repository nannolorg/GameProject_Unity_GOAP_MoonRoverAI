using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_ReturnHome : Goal_Base
{
    

    public override void PreTick()
    {

        //Can return home if inventory is full
        if (EntityInfo.IsInventoryFull())
        {
            CanRun = true;
            Priority = MaxPriority;
        }
        else
        {
            CanRun = false;
            Priority = 0;
        }
    }
}
