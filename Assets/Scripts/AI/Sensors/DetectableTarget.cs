using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectableTarget : MonoBehaviour
{
    public bool IsLocalOnly = false;
    // Start is called before the first frame update
    void Start()
    {
        if(!IsLocalOnly)
            GlobalDetectableTargetManager.Instance.Register(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        
        if (!IsLocalOnly && GlobalDetectableTargetManager.Instance != null)
            GlobalDetectableTargetManager.Instance.Deregister(this);
    }
}