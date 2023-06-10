using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAgent))]
public class ProximitySensor : MonoBehaviour
{
    CharacterAgent LinkedAI;
    LocalDetectableTargetManager TargetManager;

    // Start is called before the first frame update
    void Start()
    {
        LinkedAI = GetComponent<CharacterAgent>();
        TargetManager = GetComponent<LocalDetectableTargetManager>();
    }

    // Update is called once per frame
    void Update()
    {
        List<DetectableTarget> targets = TargetManager != null ? TargetManager.AllTargets : GlobalDetectableTargetManager.Instance.AllTargets;
        for (int index = 0; index < targets.Count; ++index)
        {
            if (targets[index] == null)
                continue;

            var candidateTarget = targets[index];

            // skip if ourselves
            if (candidateTarget.gameObject == gameObject)
                continue;

            if (Vector3.Distance(LinkedAI.EyeLocation, candidateTarget.transform.position) <= LinkedAI.ProximityDetectionRange)
                LinkedAI.ReportInProximity(candidateTarget);
        }

        
    }
}