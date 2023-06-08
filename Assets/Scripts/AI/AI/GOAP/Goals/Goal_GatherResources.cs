using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_GatherResources : Goal_Base
{
    [SerializeField] float PriorityDecayRate = 0.3f;
    [SerializeField] int BasePriority = 50;

    public override void PreTick()
    {
        //priority cannot change while running, but the priority will decay overtime.
        if (IsActive)
        {
            Priority -= Mathf.FloorToInt(PriorityDecayRate);
            return;
        }

        //can't run if inventory is full
        if (EntityInfo.IsInventoryFull())
        {
            CanRun = false;
            Priority = 0;
        } else
        {
            CanRun = true;
            Priority = BasePriority;
        }
    }
}
