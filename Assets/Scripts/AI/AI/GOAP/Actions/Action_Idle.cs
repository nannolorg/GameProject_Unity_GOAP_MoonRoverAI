using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Idle : Action_Base
{
    protected override void Initialise()
    {

    }

    public override bool CanSatisfy(Goal_Base goal)
    {
        return goal is Goal_Idle;
    }
    public override float Cost()
    {
        return 0f;
    }
    public override void OnActivate()
    {

    }
    public override void OnDeactivate()
    {

    }
    public override void OnTick()
    {

    }

}
