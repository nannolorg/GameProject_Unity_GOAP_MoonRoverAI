using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR


public enum EOffmeshLinkStatus
{
    NotStarted,
    InProgress
}

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterAgent : CharacterBase
{
    [SerializeField] float NearestPointSearchRange = 5f;

    NavMeshAgent Agent;
    bool DestinationSet = false;
    bool ReachedDestination = false;
    EOffmeshLinkStatus OffMeshLinkStatus = EOffmeshLinkStatus.NotStarted;

    public bool IsMoving => Agent.velocity.magnitude > float.Epsilon;

    public bool AtDestination => ReachedDestination;

    //
    public float CosVisionConeAngle { get; private set; } = 0f;
    [Header("Animation")]
    public Animator animator;
    [Header("Sight")]
    public float VisionConeRange = 60f;
    public float VisionConeAngle = 30f;
    public Color VisionConeColour = new Color(1f, 0f, 0f, 0.25f);

    public Vector3 EyeLocation => transform.position;
    public Vector3 EyeDirection => transform.forward;
    [Header("Hearing")]
    public float HearingRange = 20f;
    public Color HearingRangeColour = new Color(1f, 1f, 0f, 0.25f);
    [Header("Proximity")]
    public float ProximityDetectionRange = 3f;
    public Color ProximityDetectionColour = new Color(1f, 1f, 1f, 0.25f);

    AwarenessSystem Awareness;

    private Vector3 WalkPoint;
    public float WalkPointRange = 30f;
    private bool WalkPointSet = false;
    private float DistanceThreshold = 1f;

    EntityInfo EntityInfo;

    // Start is called before the first frame update
    protected void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        //caching the cosine vision angle
        CosVisionConeAngle = Mathf.Cos(VisionConeAngle * Mathf.Deg2Rad);
        Awareness = GetComponent<AwarenessSystem>();

        //Get Animator component
        animator = GetComponent<Animator>();
        EntityInfo = GetComponent<EntityInfo>();


    }

    // Update is called once per frame
    protected void Update()
    {
        // have a path and near the end point?
        if (!Agent.pathPending && !Agent.isOnOffMeshLink && DestinationSet && (Agent.remainingDistance <= Agent.stoppingDistance + 3f))
        {
            DestinationSet = false;
            ReachedDestination = true;
        }

        // are we on an offmesh link?
        if (Agent.isOnOffMeshLink)
        {
            // have we started moving along the link
            if (OffMeshLinkStatus == EOffmeshLinkStatus.NotStarted)
            {
                StartCoroutine(FollowOffmeshLink());
            }
        }
        animator.SetFloat("Speed", Agent.velocity.magnitude);
    }

    

    IEnumerator FollowOffmeshLink()
    {
        // start the offmesh link - disable NavMesh agent control
        OffMeshLinkStatus = EOffmeshLinkStatus.InProgress;
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        // move along the path
        Vector3 newPosition = transform.position;
        while (!Mathf.Approximately(Vector3.Distance(newPosition, Agent.currentOffMeshLinkData.endPos), 0f))
        {
            newPosition = Vector3.MoveTowards(transform.position, Agent.currentOffMeshLinkData.endPos, Agent.speed * Time.deltaTime);
            transform.position = newPosition;

            yield return new WaitForEndOfFrame();
        }

        // flag the link as completed
        OffMeshLinkStatus = EOffmeshLinkStatus.NotStarted;
        Agent.CompleteOffMeshLink();

        // return control the agent
        Agent.updatePosition = true;
        Agent.updateRotation = true;
        Agent.updateUpAxis = true;
    }

    public Vector3 PickLocationInRange(float range)
    {
        WalkPointSet = false;
        
        Vector3 Point = SetRandomDestination();
        return Point;
    }

    private Vector3 SetRandomDestination()
    {
        //1. pick a point
        float rx = Random.Range(-WalkPointRange, WalkPointRange);
        float rz = Random.Range(-WalkPointRange, WalkPointRange);
        WalkPoint = new Vector3(transform.position.x + rx, this.transform.position.y, transform.position.x + rz);

        Agent.SetDestination(WalkPoint);

        
        Invoke("CheckPointOnPath", 0.2f);
        return WalkPoint;
    }
    private void CheckPointOnPath()
    {
        if (Vector3.Distance(Agent.pathEndPosition, WalkPoint) < DistanceThreshold)
        {
            WalkPointSet = true;
        }
        else
        {
            SetRandomDestination();
        }


    }

    protected virtual void CancelCurrentCommand()
    {
        // clear the current path
        Agent.ResetPath();

        DestinationSet = false;
        ReachedDestination = false;
        OffMeshLinkStatus = EOffmeshLinkStatus.NotStarted;
    }

    public virtual void MoveTo(Vector3 destination)
    {
        CancelCurrentCommand();

        SetDestination(destination);
    }
    public virtual void StopMovement()
    {
        CancelCurrentCommand();
    }

    public virtual void SetDestination(Vector3 destination)
    {
        // find nearest spot on navmesh and move there
        NavMeshHit hitResult;
        if (NavMesh.SamplePosition(destination, out hitResult, NearestPointSearchRange, NavMesh.AllAreas))
        {
            Agent.SetDestination(hitResult.position);
            DestinationSet = true;
            ReachedDestination = false;
        }
    }

    //=========AWARENESS SYSTEM==============
    public void ReportCanSee(DetectableTarget seen)
    {
        WorldResource foundResource;
        if (seen.transform.TryGetComponent<WorldResource>(out foundResource))
        {
            EntityInfo.Home.SawResource(foundResource);
        }
        Awareness.ReportCanSee(seen);
        //Debug.Log("Can see target");
    }

    public void ReportCanHear(GameObject source, Vector3 location, EHeardSoundCategory category, float intensity)
    {
        Awareness.ReportCanHear(source, location, category, intensity);
    }

    public void ReportInProximity(DetectableTarget target)
    {
        Awareness.ReportInProximity(target);
        //Debug.Log("Target in proximity");

    }

    public void OnSuspicious()
    {

    }

    public void OnDetected(GameObject target)
    {

    }

    public void OnFullyDetected(GameObject target)
    {
        WorldResource foundResource;
        if (target.TryGetComponent<WorldResource>(out foundResource))
        {
            EntityInfo.Home.SawResource(foundResource);
        }
    }

    public void OnLostDetect(GameObject target)
    {

    }

    public void OnLostSuspicion()
    {

    }

    public void OnFullyLost()
    {

    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterAgent))]
public class CharacterAgentEditor : Editor
{
    public void OnSceneGUI()
    {
        var ai = target as CharacterAgent;

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