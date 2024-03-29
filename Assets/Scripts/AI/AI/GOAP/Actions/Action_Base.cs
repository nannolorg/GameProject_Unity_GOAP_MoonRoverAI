using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action_Base : MonoBehaviour
{
    protected CharacterAgent Agent;
    protected AwarenessSystem Sensors;
    protected EntityInfo EntityInfo;
    protected GOAPBrain AIBrain;

    protected Goal_Base LinkedGoal;
    public bool HasFinished { get; protected set; } = false;
    


    private void Awake()
    {
        Agent = GetComponent<CharacterAgent>();
        AIBrain = GetComponent<GOAPBrain>();
        Sensors = GetComponent<AwarenessSystem>();
        EntityInfo = GetComponent<EntityInfo>();
        Initialise();
    }

    protected abstract void Initialise();
    public abstract bool CanSatisfy(Goal_Base goal);
    public abstract float Cost();
    public abstract void OnActivate();
    public abstract void OnDeactivate();
    public abstract void OnTick();
    public virtual string GetDebugInfo()
    {
        return string.Empty;
    }
}
