using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Wander : Action_Base
{
    [SerializeField] float SearchRange = 10f;

    protected override void Initialise()
    {

    }

    public override bool CanSatisfy(Goal_Base goal)
    {
        return goal is Goal_Wander;
    }
    public override float Cost()
    {
        return 0f;
    }
    public override void OnActivate()
    {
        Vector3 location = Agent.PickLocationInRange(SearchRange);

        Agent.MoveTo(location);
    }
    public override void OnDeactivate()
    {

    }
    public override void OnTick()
    {
        //arrived at destination??
        if (Agent.AtDestination)
        {
            OnActivate();
        }
    }
}
