using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR


public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround;

    public float health;
    private bool alreadyAttacked;
    public float timeBetweenAttacks;

    public float CosVisionConeAngle { get; private set; } = 0f;

    [Header("Animation")]
    public Animator animator;

    [Header("Wander")]
    private Vector3 walkPoint;
    private bool walkPointSet = false;
    public float walkPointRange = 30f;

    [Header("Patrol")]
    public List<PatrolPoint> PatrolPointList = new List<PatrolPoint>();
    private PatrolPoint targetPoint;

    [Header("Attack")]
    public float attackRange;
    private bool playerInVisionConeRange, playerInAttackRange;

    [Header("Sight")]
    public float VisionConeRange = 60f;
    public float VisionConeAngle = 30f;
    public Color VisionConeColour = new Color(1f, 0f, 0f, 0.25f);

    public Vector3 EyeLocation => transform.position;
    public Vector3 EyeDirection => transform.forward;

    public float HearingRange = 20f;
    public Color HearingRangeColour = new Color(1f, 1f, 0f, 0.25f);

    public float ProximityDetectionRange = 3f;
    public Color ProximityDetectionColour = new Color(1f, 1f, 1f, 0.25f);

    private bool playerSighted = false;

    AwarenessSystem Awareness;

    private Vector3 previousPlayerPos;
    private float distanceThreshold = 1f;
    private float normalSpeed;
    private float sprintSpeed = 6f;

    /// <summary>
    /// States:
    /// 1/Patrol
    /// 2/Chase
    /// 3/Attack
    /// </summary>

    public void ResetAI()
    {
        playerSighted = false;
        alreadyAttacked = false;
        playerInVisionConeRange = false;
        playerInAttackRange = false;
    }


    private void Awake()
    {
        //caching the cosine vision angle
        CosVisionConeAngle = Mathf.Cos(VisionConeAngle * Mathf.Deg2Rad);
        Awareness = GetComponent<AwarenessSystem>();

        //Get Animator component
        animator = GetComponent<Animator>();


        //targetPoint = PatrolPointList[0];
        alreadyAttacked = false;
        //Change find object name to the player name
        //player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        normalSpeed = agent.speed;
        //Vector3 randomPosition = GetRandomNavMeshPosition();
        //Debug.Log(randomPosition);
    }

    private void Update()
    {
        //Check for sight range and attack range

        //playerInVisionConeRange = Physics.CheckSphere(transform.position, VisionConeRange, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //Debug.Log("playerInVisionConeRange: " + playerInVisionConeRange);
        //Debug.Log("playerInAttackRange: " + playerInAttackRange);
        //Debug.Log(player.gameObject.activeSelf);

        animator.SetFloat("Speed", agent.velocity.magnitude);
        //Debug.Log("agent.velocity.magnitude: " + agent.velocity.magnitude);
        //Debug.Log("Speed: " + agent.speed);

        //if (!playerInVisionConeRange && !playerInAttackRange) Patrol();
        if (!playerSighted) Wander();
        if (playerInVisionConeRange && !playerInAttackRange) Chase();
        if (playerInVisionConeRange && playerInAttackRange) Attack();



        
    }


    private void Wander()
    {
        
        if (!walkPointSet && !agent.hasPath)
        {
            walkPointSet = true;
            //SetRandomDestination();
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 0.1f)
        {
            walkPointSet = false;
        }
    }

    private void SetRandomDestination()
    {
        //1. pick a point
        float rx = Random.Range(-walkPointRange, walkPointRange);
        float rz = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + rx, this.transform.position.y, transform.position.x + rz);

        agent.SetDestination(walkPoint);

        Invoke("CheckPointOnPath", 0.2f);


        //walkPointSet = false;

        //Debug.Log("agent.pathEndPosition: " + agent.pathEndPosition);
        //Debug.Log("walkPoint" + walkPoint);
    }

    private void CheckPointOnPath()
    {
        ////3. draw line (#1 tutorial)
        //if (agent.path.corners.Length >= 2)
        //{
        //    line.positionCount = agent.path.corners.Length;
        //    for (int i = 0; i < agent.path.corners.Length; i++)
        //    {
        //        line.SetPosition(i, agent.path.corners[i]);
        //    }
        //}
        


        //4. check //if point is not on navmesh!!!
        //if (agent.pathEndPosition != walkPoint)
        //{
        //    //Debug.Log("Point not on mesh");

        //    SetRandomDestination();
        //}
        if (Vector3.Distance(agent.pathEndPosition, walkPoint) < distanceThreshold)
        {
            walkPointSet = true;
        }
        else
        {
            SetRandomDestination();
        }


    }


    private void Patrol()
    {
        //if the character has valid patrol point, if patrolpoint list is not null and targetpoint is not null
        if (PatrolPointList != null && targetPoint != null)
        {
            //Travel to point's transform
            agent.SetDestination(targetPoint.gameObject.transform.position);
            //Once reached point, we increment the point, check if the player enter another object collision, and that collision contains a patrol point script

            var distToTarget =  targetPoint.gameObject.transform.position - gameObject.transform.position;

            

            if (distToTarget.magnitude < 1f)
            {
                Invoke("IncrementPoint", targetPoint.waitTime);
                IncrementPoint();
            }
        }


    }




    public void IncrementPoint()
    {
        //if the character is at the end of the patrol points, then revert targetpoint to the first patrol point.
        //else just increment to the next patrol point 
        if (targetPoint == PatrolPointList[PatrolPointList.Count - 1])
        {
            targetPoint = PatrolPointList[0];
        }
        else
        {
            int targetPointID = PatrolPointList.IndexOf(targetPoint);
            targetPoint = PatrolPointList[targetPointID + 1];
        }
        
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }

    }

    private void Chase()
    {
        var PlayerActive = player.gameObject.activeSelf;

        if (PlayerActive)
        {
            agent.speed = sprintSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.speed = normalSpeed;
            playerInVisionConeRange = false;
            playerInAttackRange = false;
        }
       
    }

    private void Attack()
    {
        var PlayerActive = player.gameObject.activeSelf;
        //Make sure enemy doesn't move
        //agent.SetDestination(transform.position);
        if (PlayerActive)
        {
            agent.SetDestination(player.position);

            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                ///Attack code here
                ///
                /// 
                /// 
                /// 
                ///
                GameManager.instance.GameOver();
                //Debug.Log("GAME OVER");


                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
        else
        {
            playerInVisionConeRange = false;
            playerInAttackRange = false;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void ReportCanSee(DetectableTarget seen)
    {
        
        Awareness.ReportCanSee(seen);
        Debug.Log("Can see player");
        playerInVisionConeRange = true;
    }

    public void ReportCanHear(GameObject source, Vector3 location, EHeardSoundCategory category, float intensity)
    {
        Awareness.ReportCanHear(source, location, category, intensity);
    }

    public void ReportInProximity(DetectableTarget target)
    {
        Awareness.ReportInProximity(target);
        Debug.Log("Player in proximity");
        if (playerInVisionConeRange)
        {
            playerInAttackRange = true;
        }
        
    }

    public void OnSuspicious()
    {
        //FeedbackDisplay.text = "I hear you";
    }

    public void OnDetected(GameObject target)
    {
        //FeedbackDisplay.text = "I see you " + target.gameObject.name;
    }

    public void OnFullyDetected(GameObject target)
    {
        //FeedbackDisplay.text = "Charge! " + target.gameObject.name;
        previousPlayerPos = player.position;
    }

    public void OnLostDetect(GameObject target)
    {
        agent.speed = normalSpeed;
        Debug.Log("I lost player, going to point: " + previousPlayerPos);
        //playerInVisionConeRange = false;
        agent.SetDestination(previousPlayerPos);
        //FeedbackDisplay.text = "Where are you " + target.gameObject.name;
    }

    public void OnLostSuspicion()
    {
        //FeedbackDisplay.text = "Where did you go";
    }

    public void OnFullyLost()
    {
        playerInVisionConeRange = false;
        agent.speed = normalSpeed;
        //FeedbackDisplay.text = "Must be nothing";
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyAI))]
public class EnemyAIEditor : Editor
{
    public void OnSceneGUI()
    {
        var ai = target as EnemyAI;

        // draw the detectopm range
        Handles.color = ai.ProximityDetectionColour;
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.ProximityDetectionRange);

        // draw the hearing range
        Handles.color = ai.HearingRangeColour;
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.HearingRange);

        // work out the start point of the vision cone
        Vector3 startPoint = Mathf.Cos(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.forward +
                             Mathf.Sin(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.right;

        // draw the vision cone
        Handles.color = ai.VisionConeColour;
        Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.VisionConeAngle * 2f, ai.VisionConeRange);        
    }
}
#endif // UNITY_EDITOR