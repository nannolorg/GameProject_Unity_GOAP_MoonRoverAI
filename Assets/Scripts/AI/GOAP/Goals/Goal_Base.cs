using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoal
{
    //This return the goal priority | Priority rate 0-100
    int OnCalculatePriority();
    //This is when we want to know if the goal is able to run
    bool CanRun();
    //Child class overwrite this, instead of Update
    void OnTickGoal();
    //This is when we want to know if a goal become active or not | to start off a logic before a goal start, if needed
    void OnGoalActivated();
    //This is when we want to know if a goal become deactive | to start off a logic before a goal end, if needed
    void OnGoalDeactivate();
}

public class Goal_Base : MonoBehaviour, IGoal
{
    protected CharacterAgent Agent;
    protected AwarenessSystem Sensors;
    //protected GOAPUI DebugUI;

    //Priority rate is between 0-100
 
    void Awake()
    {
        Agent = GetComponent<CharacterAgent>();
        Sensors = GetComponent<AwarenessSystem>();
    }

    void Start()
    {
        //DebugUI = FindObjectOfType<GOAPUI>();
    }
    //it get messy if we declare functions in update
    void Update()
    {
        OnTickGoal();

        //DebugUI.UpdateGoal(this, GetType().Name, "", OnCalculatePriority());
    }


    
    public virtual int OnCalculatePriority()
    {
        return -1;
    }

    
    public virtual bool CanRun()
    {
        return false;
    }

    
    public virtual void OnTickGoal()
    {

    }

    
    public virtual void OnGoalActivated()
    {

    }

    
    public virtual void OnGoalDeactivate()
    {

    }
}
