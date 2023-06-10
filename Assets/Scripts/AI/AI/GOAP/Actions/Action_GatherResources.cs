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

    [SerializeField] float CollectionTime = 5f;
    [SerializeField] float StorageTime = 5f;
    float ActionTimeRemaining = 0f;

    WorldResource Target;

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
                Target = EntityInfo.Home.GetGatherTarget(AIBrain);
                break;
            case EState.MoveToResource:
                Agent.MoveTo(Target.transform.position);
                break;
            case EState.CollectResource:
                ActionTimeRemaining = CollectionTime;
                break;
            //case EState.ReturnHome:
            //    Agent.MoveTo(EntityInfo.Home.GetRandomPointAroundBase());
            //    break;
            //case EState.OffloadResource:
            //    ActionTimeRemaining = StorageTime;
            //    break;
        }
    }

    protected override void Tick()
    {
        switch (State)
        {
            case EState.PickResource:
                if (Target == null)
                {
                    Target = EntityInfo.Home.GetGatherTarget(AIBrain);
                }
                break;
            case EState.MoveToResource:
                if (Target == null)
                {
                    HasFinished = true;
                }
                break;
            case EState.CollectResource:
            case EState.OffloadResource:
                ActionTimeRemaining -= Time.deltaTime;
                break;
            //case EState.ReturnHome:

            //    break;
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
            case EState.PickResource:
                if (Target != null)
                {
                    return EState.MoveToResource;
                }else
                {
                    HasFinished = true;
                }
                break;
            case EState.MoveToResource:
                if (Agent.AtDestination)
                {
                    return EState.CollectResource;
                }
                break;  
            case EState.CollectResource:
                Debug.Log(ActionTimeRemaining);
                if (ActionTimeRemaining <= 0f)
                {
                    HasFinished = true;
                    var ResourceToAdd = Target.HarvestAll();
                    EntityInfo.AddToInventory(Target.Type, ResourceToAdd);
                    //Make resource depleted

                    return EState.PickResource;
                    //return EState.ReturnHome;
                }

                //check if inventory is full or not
               
                break;
            //case EState.ReturnHome:
            //    Debug.Log(ActionTimeRemaining);
            //    if (Agent.AtDestination)
            //    {
                    
            //        return EState.OffloadResource;
            //    }
            //    break;
            //case EState.OffloadResource:
            //    if (ActionTimeRemaining <= 0f)
            //    {
            //        EntityInfo.ResetInventory();
            //        HasFinished = true;
            //    }
            //    break;
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
