using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDetectableTargetManager : MonoBehaviour
{
    public List<DetectableTarget> AllTargets { get; private set; } = new List<DetectableTarget>();

    private void Update()
    {
        //check if resource is still active in scene
        if (AllTargets.Count > 0)
        {
            for (int i = 0; i < AllTargets.Count; i++)
            {
                if (AllTargets[i] != null)
                    continue;

                RemoveTargets(AllTargets[i]);
            }
        }
        
    }

    void RemoveTargets(DetectableTarget target)
    {
        AllTargets.Remove(target);
    }

    void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<DetectableTarget>();
        if (target != null && !AllTargets.Contains(target))
            AllTargets.Add(target); 
    }

    void OnTriggerExit(Collider other)
    {
        var target = other.gameObject.GetComponent<DetectableTarget>();
        if (target != null)
            AllTargets.Remove(target);
    }

}