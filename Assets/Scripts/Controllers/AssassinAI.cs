using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinAI : AIController
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
                // Do work
                DoAttackState();
                // Check for transitions
                if (!CanSee(target) && !CanHear(target))
                {
                    ChangeState(AIState.Patrol);
                }
                break;
            case AIState.Flee:
                //Do work
                ChangeState(AIState.Chase);
                break;
            case AIState.ChooseTarget:
                //Do work
                TargetPlayerOne();
                break;
            case AIState.Patrol:
                // Do work 
                Patrol();
                // Check for transitions
                if (CanSee(target) || CanHear(target))
                {
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.Dead:
                //do nothing. you are dead.
                break;
            case AIState.Wait:
                //Do work
                ChangeState(AIState.Chase);
                break;
            default:
                ChangeState(AIState.ChooseTarget);
                break;
        }
    }
}
