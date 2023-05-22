using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_Wander : Goal_Base
{
    [SerializeField]
    private int MinPriority = 0;
    private int MaxPriority = 30;
    //if idle for a time, then it build the priority by 1
    private float PriorityBuildRate = 1f;
    private float PriorityDecayRate = 0.1f;


    private float CurrentPriority = 0f;

    public override void OnTickGoal()
    {
        if (Agent.IsMoving)
        {
            CurrentPriority -= PriorityDecayRate * Time.deltaTime;
        }
        else
        {
            CurrentPriority += PriorityBuildRate * Time.deltaTime;
        }
    }

    public override void OnGoalActivated()
    {
        CurrentPriority = MaxPriority;
    }

    public override int OnCalculatePriority()
    {
        return Mathf.FloorToInt(CurrentPriority);
    }

    public override bool CanRun()
    {
        return true;
    }
    
    
}
