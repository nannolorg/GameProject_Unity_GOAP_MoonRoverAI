using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class HomeBase : MonoBehaviour
{
    [SerializeField] int BaseRadius = 10;
    public Color ProximityColour = new Color(1f, 1f, 1f, 0.25f);

    [SerializeField] float PerfectKnowledgeRange = 10f;
    [SerializeField] WorldResource.EType DefaultResource = WorldResource.EType.Samples;
    Dictionary<WorldResource.EType, List<WorldResource>> TrackedResources = null;
    Dictionary<WorldResource.EType, int> ResourcesStored = new Dictionary<WorldResource.EType, int>();
    public int NumAvailableResources { get; private set; } = 0;


    void Awake()
    {
        var resourceTypes = System.Enum.GetValues(typeof(WorldResource.EType));
        foreach (var value in resourceTypes)
        {
            ResourcesStored[(WorldResource.EType)value] = 0;

        }

    }


    // Update is called once per frame
    void Update()
    {
        if (TrackedResources == null)
        {
            PopulateResources();
        }
    }

    public Vector3 GetRandomPointAroundBase()
    {
        float x_pos = Random.Range(-BaseRadius, BaseRadius);
        float z_pos = Random.Range(-BaseRadius, BaseRadius);


        Vector3 point = new Vector3(x_pos, this.transform.position.y, z_pos);

        return point;
    }


    void PopulateResources()
    {
        // build up the resource knowledge
        var resourceTypes = System.Enum.GetValues(typeof(WorldResource.EType));
        TrackedResources = new Dictionary<WorldResource.EType, List<WorldResource>>();
        foreach (var value in resourceTypes)
        {
            var type = (WorldResource.EType)value;
            TrackedResources[type] = ResourceTracker.Instance.GetResourcesInRange(type, transform.position, PerfectKnowledgeRange);

            NumAvailableResources += TrackedResources[type].Count;
        }
    }

    public WorldResource GetGatherTarget(GOAPBrain AIBrain)
    {
        WorldResource.EType targetResource = DefaultResource;

        //if (TrackedResources[targetResource].Count == 0)
        //    return null;


        //find the closest resource to us
        var sortedResources = TrackedResources[targetResource].OrderBy(resource => Vector3.Distance(AIBrain.transform.position, resource.transform.position)).ToList();
        return sortedResources[Random.Range(0, Mathf.Min(BaseRadius, sortedResources.Count))];
    }

    public void SawResource(WorldResource resource)
    {
        if (!TrackedResources[resource.Type].Contains(resource))
        {
            TrackedResources[resource.Type].Add(resource);
            ++NumAvailableResources;
        }
    }

    public void StoreResource(WorldResource.EType type, int amount)
    {
        ResourcesStored[type] += amount;
    }

    //public bool IsThereResourcesToCollect()
    //{
    //    if (NumAvailableResources <= 0)
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}




#if UNITY_EDITOR
    [CustomEditor(typeof(HomeBase))]
    public class HomeBaseEditor : Editor
    {
        public void OnSceneGUI()
        {
            var ai = target as HomeBase;

            // draw the detectopm range
            Handles.color = ai.ProximityColour;
            Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.BaseRadius);

            
        }
    }
#endif // UNITY_EDITOR

}
