using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPBrain : MonoBehaviour
{
    Goal_Base[] Goals;
    Action_Base[] Actions;

    Goal_Base ActiveGoal;
    Action_Base ActiveAction;



    private void Awake()
    {
        Goals = GetComponents<Goal_Base>();
        Actions = GetComponents<Action_Base>();
    }

    private void Update()
    {
        //Update all of the goals
        for (int goalID = 0; goalID < Goals.Length; ++goalID)
        {
            Goals[goalID].PreTick();
        }

        RefreshPlan();

        if (ActiveAction != null)
        {
            ActiveAction.OnTick();

            //if action finished - cleanup goal
            if (ActiveAction.HasFinished)
            {
                ActiveGoal.OnDeactivate();
                ActiveGoal = null;
                ActiveAction = null;
            }

        }

    }

    void RefreshPlan()
    {
        //find the best goal-action pair
        Goal_Base bestGoal = null;
        Action_Base bestAction = null;

        //find the highest priority goal that can be activated
        for (int goalID = 0; goalID < Goals.Length; ++goalID)
        {
            var goal = Goals[goalID];

            //can it run?
            if (!goal.CanRun)
            {
                continue;
            }

            //is it a better priority?
            if (bestGoal != null && bestGoal.Priority > goal.Priority)
            {
                continue;
            }


            //find the cheapest action for this goal
            Action_Base candidateAction = null;
            foreach (var action in Actions)
            {
                //skip if this action can't satisfy the goal
                if (!action.CanSatisfy(goal))
                {
                    continue;
                }

                //skip if this action is more expensive
                if (candidateAction != null && action.Cost() > candidateAction.Cost())
                {
                    continue;
                }

                candidateAction = action;
            }

            //did we find an action?
            if (candidateAction != null)
            {
                bestGoal = goal;
                bestAction = candidateAction;
            }
        }

        //if current plan is equal to the calculated best plan - do nothing
        if (bestGoal == ActiveGoal && bestAction == ActiveAction)
        {
            return;
        }


        //if no best goal, but active goal is true then, deactivate it, set it to null
        if (bestGoal == null)
        {
            if (ActiveGoal != null)
            {
                ActiveGoal.OnDeactivate();
            }

            ActiveGoal = null;
            ActiveAction = null;
            return;
        }

        //if goal has changed, put previous one to sleep, and wake up current one
        if (bestGoal != ActiveGoal && ActiveGoal != null)
        {
            if (ActiveGoal != null)
            { 
                ActiveGoal.OnDeactivate();
            }

            bestGoal.OnActivate();
        }

        ActiveGoal = bestGoal;
        ActiveAction = bestAction;
        ActiveGoal.SetAction(ActiveAction);

        ////if no current goal
        //if (ActiveGoal == null)
        //{
        //    ActiveGoal = bestGoal;
        //    ActiveAction = bestAction;

        //    if (ActiveGoal != null)
        //    {
        //        ActiveGoal.OnActivate();
        //    }
        //    if (ActiveAction != null)
        //    {
        //        ActiveAction.OnActivate();
        //    }

        //}//no change in goal?
        //else if (ActiveGoal = bestGoal)
        //{
        //    //action changed?
        //    if (ActiveAction != bestAction)
        //    {
        //        ActiveAction.OnDeactivate();
        //        ActiveAction = bestAction;
        //        ActiveAction.OnActivate();
        //    }
        //}// change to new goal or no valid goal
        //else if (ActiveGoal != bestGoal)
        //{
        //    ActiveGoal.OnDeactivate();
        //    ActiveAction.OnDeactivate();

        //    ActiveGoal = bestGoal;
        //    ActiveAction = bestAction;

        //    if (ActiveGoal != null)
        //    {
        //        ActiveGoal.OnActivate();
        //    }
        //    if (ActiveAction != null)
        //    {
        //        ActiveAction.OnActivate();
        //    }

        //}

        ////tick the action
        //if (ActiveAction != null)
        //{
        //    ActiveAction.OnTick();
        //}
    }

    public string GetActiveGoal()
    {
        if (ActiveGoal)
        {
            return ActiveGoal.ToString();
        }
        return "Unidentified Goal";
    }
}
