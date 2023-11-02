using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderData;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.UIElements;
using System;

[System.Serializable]
public class AIController : Controller
{
    //define variables
    private float lastStateChangeTime;
    public float fleeDistance;
    public float safeDistance;
    public float hearingDistance;
    public float fieldOfView;
    public float viewDistance;
    public float waypointStopDistance;
    protected bool doUpdate = true;
    public float fireRate;
    private float timeSinceShot;
    public float waitTime;
    private float waitedTime;

    //variables for collision detection
    private float steeringAmount = 1f;
    public float collisionRange = 3f;

    public GameObject target;

    public Transform[] waypoints;
    private int currentWaypoint = 0;

    public enum AIState { Idle, Chase, Flee, ChooseTarget, Patrol, Dead, Wait};
    public AIState currentState;

    // Start is called before the first frame update
    public override void Start()
    {
        // Run the parent (base) Start
        base.Start();

        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            //check if player list exists
            if (GameManager.Instance.AI != null)
            {
                //add self to player list
                GameManager.Instance.AI.Add(this);
            }
        }
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
            // Run the parent (base) Update
            base.Update();
        }
    }

    public virtual void MakeDecisions()
    {
        if(target == null)
        {
            ChangeState(AIState.ChooseTarget);
        }
        switch (currentState)
        {
            case AIState.Idle:
                // Do work 
                DoIdleState();
                //check for flee condition
                if (IsDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Flee);
                }
                // Check for transitions
                else if (CanSee(target) || CanHear(target))
                {
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.Chase:
                // Do work
                DoAttackState();
                //check for flee condition
                if (IsDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Flee);
                }
                // Check for transitions
                else if (!CanSee(target) && !CanHear(target))
                {
                    ChangeState(AIState.Patrol);
                }
                break;
            case AIState.Flee:
                //Do work
                Flee();
                //check if far enough away
                if (!IsDistanceLessThan(target, safeDistance))
                {
                    ChangeState(AIState.Patrol);
                }
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
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.Dead:
                //do nothing. you are dead.
                break;
            case AIState.Wait:
                DoWaitState();
                break;
            default:
                ChangeState(AIState.ChooseTarget);
                break;
        }
    }
    public virtual void ChangeState(AIState newState)
    {
        // Change the current state
        currentState = newState;
        // Save the time when we changed states
        lastStateChangeTime = Time.time;

    }
    public void TargetPlayerOne()
    {
        // If the GameManager exists
        if (GameManager.Instance != null)
        {
            // And the array of players exists
            if (GameManager.Instance.players != null)
            {
                // And there are players in it
                if (GameManager.Instance.players.Count > 0)
                {
                    //Then target the gameObject of the pawn of the first player controller in the list
                    target = GameManager.Instance.players[0].pawn.gameObject;
                    ChangeState(AIState.Patrol);
                }
            }
        }
    }
    protected bool IsHasTarget()
    {
        // return true if we have a target, false if we don't
        return (target != null);
    }
    public void DoSeekState()
    {
        // Seek our target
        Seek(target);
    }
    public void Seek(GameObject target)
    {
        // Seek the position of our target Transform
        Seek(target.transform.position);
    }
    public void Seek(Vector3 targetPosition) //this is the one all the others refer to
    {
        //trace for collision forwards
        CollisionDetect();
        // RotateTowards the Funciton
        pawn.RotateTowards(targetPosition, steeringAmount);
        // Move Forward
        pawn.MoveForwards();

    }
    public void Seek(Transform targetTransform)
    {
        // Seek the position of our target Transform
        Seek(targetTransform.position);
    }
    public void Seek(Pawn targetPawn)
    {
        // Seek the pawn's transform!
        Seek(targetPawn.transform);
    }
    protected virtual void DoChaseState()
    {
        Seek(target);
    }
    protected virtual void DoIdleState()
    {
        // Do Nothing
    }
    protected bool IsDistanceLessThan(GameObject target, float distance)
    {
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Shoot()
    {
        //only shoot as fast as fire rate allows
        if(timeSinceShot >= fireRate)
        {
            // Tell the pawn to shoot
            pawn.Shoot();
            timeSinceShot = 0;
        }
        else
        {
            //add time
            timeSinceShot += Time.deltaTime;
        }
    }
    protected virtual void DoAttackState()
    {
        // Chase
        Seek(target);
        // Shoot
        Shoot();
    }
    protected void Flee()
    {
        float targetDistance = Vector3.Distance(target.transform.position, pawn.transform.position);
        float percentOfFleeDistance = targetDistance / fleeDistance;
        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);
        float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;

        // Find the Vector to our target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;
        // Find the Vector away from our target by multiplying by -1
        Vector3 vectorAwayFromTarget = -vectorToTarget;
        // Find the vector we would travel down in order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance * flippedPercentOfFleeDistance;
        // Seek the point that is "fleeVector" away from our current position
        Seek(pawn.transform.position + fleeVector);

        
    }
    protected void Patrol()
    {
        // If we have a enough waypoints in our list to move to a current waypoint
        if (waypoints.Length > currentWaypoint)
        {
            //debug line
            Debug.DrawLine(pawn.transform.position + new Vector3(0f, 0.5f, 0f), waypoints[currentWaypoint].position, color: Color.green);
            // Then seek that waypoint
            Seek(waypoints[currentWaypoint]);
            // If we are close enough, then increment to next waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance)
            {
                currentWaypoint++;
            }
        }
        else
        {
            RestartPatrol();
        } 
    }
    protected void RestartPatrol()
    {
        // Set the index to 0
        currentWaypoint = 0;
    }
    public void DoWaitState()
    {
        if(waitedTime >= waitTime)
        {
            ChangeState(AIState.Chase);
            waitedTime = 0;
        }
        else
        {
            waitedTime += Time.deltaTime;
        }
    }
    public bool CanHear(GameObject target)
    {
        // Get the target's NoiseMaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();
        // If they don't have one, they can't make noise, so return false
        if (noiseMaker == null)
        {
            return false;
        }
        // If they are making 0 noise, they also can't be heard
        if (noiseMaker.GetVolumeDistance() <= 0)
        {
            return false;
        }
        // If they are making noise, add the volumeDistance in the noisemaker to the hearingDistance of this AI
        float totalDistance = noiseMaker.GetVolumeDistance() + hearingDistance;
        // If the distance between our pawn and target is closer than this...
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            // ... then we can hear the target
            return true;
        }
        else
        {
            // Otherwise, we are too far away to hear them
            return false;
        }
    }
    public bool CanSee(GameObject target)
    {
        // Find the vector from the agent to the target
        Vector3 agentToTargetVector = target.transform.position - pawn.transform.position;
        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target.
        float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);
        //raycast from AI to player
        //do raycast and assign to bool
        RaycastHit hit;
        bool hasSightLine = false;
        Physics.Raycast(pawn.transform.position + new Vector3(0f,0.5f, 0f), agentToTargetVector, out hit, viewDistance);
        //set sight line to true if raycast hits player
        if(hit.collider != null)
        {
            if (hit.collider.gameObject == target)
            {
                hasSightLine = true;
            }
        }
        //Debug.DrawRay(pawn.transform.position + new Vector3(0f, 0.5f, 0f), agentToTargetVector);
        // if that angle is less than our field of view and has line of sight
        if (angleToTarget < fieldOfView && hasSightLine)
        {
            return true;
        }
        else
        {
            return false;
        }


    }
    public void CollisionDetect()
    {
        Vector3 offsetAngle = pawn.transform.forward;
        //do multiple raycasts in front of AI and test which hits shortest
        float closestHit = collisionRange;
        RaycastHit hit;
        Physics.Raycast(pawn.transform.position + new Vector3(0f, 0.5f, 0f), offsetAngle, out hit, collisionRange);
        Debug.DrawLine(pawn.transform.position + new Vector3(0f, 0.5f, 0f), pawn.transform.position + offsetAngle*collisionRange + new Vector3(0f, 0.5f, 0f), color:Color.red);

        if(hit.collider != null)
        {
            steeringAmount = collisionRange / hit.distance;
        }
    }
}
