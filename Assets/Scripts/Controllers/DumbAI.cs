using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbAI : AIController
{
    // Start is called before the first frame update
    public override void Start()
    {
        // Run the parent (base) Start
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (doUpdate)
        {
            if (pawn != null)
            {
                //check if AI is dead
                if (pawn.GetComponent<DeathComponent>() != null)
                {
                    ChangeState(AIState.Dead);
                    doUpdate = false;
                }
            }
            // Make decisions
            MakeDecisions();
        }
    }
    public override void MakeDecisions()
    {
        switch (currentState)
        {
            case AIState.Idle:
                //Roam
                ChangeState(AIState.Patrol);
                break;
            case AIState.Chase:
                //Roam
                ChangeState(AIState.Patrol);
                break;
            case AIState.Flee:
                //Roam
                ChangeState(AIState.Patrol);
                break;
            case AIState.ChooseTarget:
                //Do work
                TargetPlayerOne();
                break;
            case AIState.Patrol:
                // Do work 
                Patrol();
                //check for flee condition
                if (IsDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Flee);
                }
                // Check for transitions
                else if (CanSee(target) || CanHear(target))
                {
                    ChangeState(AIState.Flee);
                }
                break;
            case AIState.Dead:
                //do nothing. you are dead.
                break;
            case AIState.Wait:
                //Roam
                ChangeState(AIState.Patrol);
                break;
            default:
                ChangeState(AIState.ChooseTarget);
                break;
        }
    }
}
