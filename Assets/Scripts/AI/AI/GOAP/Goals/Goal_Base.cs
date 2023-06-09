using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Goal_Base : MonoBehaviour
{
    protected CharacterAgent Agent;
    protected AwarenessSystem Sensors;
    protected EntityInfo EntityInfo;
    protected GOAPBrain AIBrain;

    protected Action_Base LinkedAction;
    //protected GOAPUI DebugUI;

    //Priority rate is between 0-100
    public const int MaxPriority = 100;
    public bool CanRun { get; protected set; } = false;
    public int Priority { get; protected set; } = 0;
    public bool IsActive { get; protected set; } = false;


    void Awake()
    {
        Agent = GetComponent<CharacterAgent>();
        AIBrain = GetComponent<GOAPBrain>();
        Sensors = GetComponent<AwarenessSystem>();
        EntityInfo = GetComponent<EntityInfo>();
    }

    public void SetAction(Action_Base newAction)
    {
        LinkedAction = newAction;

        LinkedAction.OnActivate();
    }
    public abstract void PreTick();

    public virtual void OnTick()
    {
        LinkedAction.OnTick();
    }

    public virtual void OnActivate()
    {
        IsActive = true;
    }
    public virtual void OnDeactivate()
    {
        LinkedAction.OnDeactivate();

        IsActive = false;
    }
}
