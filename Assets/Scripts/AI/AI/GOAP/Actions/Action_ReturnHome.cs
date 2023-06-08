using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_ReturnHome : Action_Base
{
    protected override void Initialise()
    {

    }

    public override bool CanSatisfy(Goal_Base goal)
    {
        return goal is Goal_ReturnHome;
    }
    public override float Cost()
    {
        return 0f;
    }
    public override void OnActivate()
    {
        Agent.MoveTo(Info.Home.GetRandomPointAroundBase());
    }
    public override void OnDeactivate()
    {

    }
    public override void OnTick()
    {

    }
}
