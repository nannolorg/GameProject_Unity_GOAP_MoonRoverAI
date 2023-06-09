using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_ReturnHome : Action_FSMBase<Action_ReturnHome.EState>
{
    public enum EState
    {
        ReturnHome,
        OffloadResource
    }

    [SerializeField] float StorageTime = 5f;
    float ActionTimeRemaining = 0f;

    protected override void Initialise()
    {
        AddState(EState.ReturnHome);
        AddState(EState.OffloadResource);
    }


    protected override void Enter()
    {
        switch (State)
        {
            case EState.ReturnHome:
                Agent.MoveTo(EntityInfo.Home.GetRandomPointAroundBase());
                break;
            case EState.OffloadResource:
                ActionTimeRemaining = StorageTime;
                break;
        }
    }

    protected override void Tick()
    {
        switch (State)
        {
            case EState.ReturnHome:
                break;
            case EState.OffloadResource:
                ActionTimeRemaining -= Time.deltaTime;
                break;
            
        }
    }

    protected override void Exit()
    {
        Agent.StopMovement();
    }

    protected override EState CheckTransition()
    {
        switch (State)
        {
            case EState.ReturnHome:
                
                if (Agent.AtDestination)
                {

                    return EState.OffloadResource;
                }
                break;
            case EState.OffloadResource:
                if (ActionTimeRemaining <= 0f)
                {
                    EntityInfo.AddInventoryItemsToHome();
                    EntityInfo.ResetInventory();
                    HasFinished = true;
                }
                break;
        }

        return State;
    }


    public override bool CanSatisfy(Goal_Base goal)
    {
        
        return goal is Goal_ReturnHome;
    }
    public override float Cost()
    {
        return 0f;
    }
    
}
