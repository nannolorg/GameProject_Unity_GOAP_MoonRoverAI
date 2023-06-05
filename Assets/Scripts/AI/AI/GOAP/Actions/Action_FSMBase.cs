using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Action_FSMBase<S> : Action_Base
{
    class StateConfig
    {
        public Action OnEnter;
        public Action OnTick;
        public Action OnExit;

        public Func<S> CheckTransition;
    }

    [SerializeField] protected S InitialState;


    //S is state enum
    Dictionary<S, StateConfig> StateMachine = new Dictionary<S, StateConfig>();
    protected S State { get; private set; }

    protected void AddState(S state, Action onEnterFn = null,
                                     Action onTickFn = null,
                                     Action onExitFn = null,
                                     Func<S> checkTransitionFn = null)
    {
        StateMachine[state] = new StateConfig()
        {   //if onEnterFn not equal to null [(?)then] use onEnterFn [(:)otherwise] use Enter
            OnEnter = onEnterFn != null ? onEnterFn : Enter,
            OnTick = onTickFn != null ? onTickFn : Tick,
            OnExit = onExitFn != null ? onExitFn : Exit,
            CheckTransition = checkTransitionFn != null ? checkTransitionFn : CheckTransition
        };
    }

    public sealed override void OnTick()
    {
        // tick the current state
        StateMachine[State].OnTick();

        // look for a transition
        S nextState = StateMachine[State].CheckTransition();
        //can't compare normally
        if (EqualityComparer<S>.Default.Equals(State, nextState))
            return;

        // perform the transition
        StateMachine[State].OnExit();
        State = nextState;
        StateMachine[State].OnEnter();
    }

    protected virtual void Enter()
    {

    }

    protected virtual void Tick()
    {

    }

    protected virtual void Exit()
    {

    }

    protected virtual S CheckTransition()
    {
        return State;
    }

    public sealed override void OnActivate()
    {
        State = InitialState;
        HasFinished = false;
        StateMachine[State].OnEnter();
    }

    public sealed override void OnDeactivate()
    {
        StateMachine[State].OnExit();
    }
}