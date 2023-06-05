using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action_Base : MonoBehaviour
{
    protected CharacterAgent Agent;
    protected AwarenessSystem Sensors;
    protected EntityInfo Info;

    protected Goal_Base LinkedGoal;
    public bool HasFinished { get; protected set; } = false;
    


    private void Awake()
    {
        Agent = GetComponent<CharacterAgent>();
        Sensors = GetComponent<AwarenessSystem>();
        Initialise();
    }

    protected abstract void Initialise();
    public abstract bool CanSatisfy(Goal_Base goal);
    public abstract float Cost();
    public abstract void OnActivate();
    public abstract void OnDeactivate();
    public abstract void OnTick();
}
