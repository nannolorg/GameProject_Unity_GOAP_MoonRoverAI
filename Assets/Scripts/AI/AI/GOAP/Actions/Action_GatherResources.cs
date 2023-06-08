using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_GatherResources : Action_FSMBase<Action_GatherResources.EState>
{
    public enum EState
    {
        PickResource,
        MoveToResource,
        CollectResource,
        ReturnHome,
        OffloadResource
    }


    protected override void Initialise()
    {
        AddState(EState.PickResource);
        AddState(EState.MoveToResource);
        AddState(EState.CollectResource);
        AddState(EState.ReturnHome);
        AddState(EState.OffloadResource);
    }

    protected override void Enter()
    {
        switch(State)
        {
            case EState.PickResource:
                break;
            case EState.MoveToResource:
                break;
            case EState.CollectResource:
                break;
            case EState.ReturnHome:
                break;
            case EState.OffloadResource:
                break;
        }
    }

    protected override void Tick()
    {
        switch (State)
        {
            case EState.PickResource:
                break;
            case EState.MoveToResource:
                break;
            case EState.CollectResource:
                break;
            case EState.ReturnHome:
                break;
            case EState.OffloadResource:
                break;
        }
    }

    protected override void Exit()
    {
        switch (State)
        {
            case EState.PickResource:
                break;
            case EState.MoveToResource:
                break;
            case EState.CollectResource:
                break;
            case EState.ReturnHome:
                break;
            case EState.OffloadResource:
                break;
        }
    }

    protected override EState CheckTransition()
    {
        switch (State)
        {
            case EState.PickResource:
                break;
            case EState.MoveToResource:
                break;
            case EState.CollectResource:
                break;
            case EState.ReturnHome:
                break;
            case EState.OffloadResource:
                break;
        }

        return State;
    }




    public override bool CanSatisfy(Goal_Base goal)
    {
        return goal is Goal_GatherResources; 
    }

    public override float Cost()
    {
        return 0f;
    }

}
