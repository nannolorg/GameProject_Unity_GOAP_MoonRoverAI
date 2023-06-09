using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_GatherResources : Goal_Base
{
    [SerializeField] int BasePriority = 50;


    //Run if the inventory is not full
    public override void PreTick()
    {
        //priority cannot change while running
        if (IsActive)
        {
            return;
        }

        //check if there is any resource to collect
        //if (!EntityInfo.Home.IsThereResourcesToCollect())
        //{
        //    CanRun = false;
        //    Priority = 0;
        //    return;
        //}

        //can't run if inventory is full
        if (EntityInfo.IsInventoryFull())
        {
            CanRun = false;
            Priority = 0;
        }
        else
        {
            CanRun = true;
            Priority = BasePriority;
        }
    }

    public override void OnActivate()
    {
        base.OnActivate();
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();

    }
}
