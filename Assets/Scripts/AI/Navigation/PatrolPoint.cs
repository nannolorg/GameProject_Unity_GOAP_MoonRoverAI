using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PatrolPoint : MonoBehaviour
{
    public bool waitActive = false;
    public float waitTime = 0f;
    public bool triggerAnimAtLocation = false;
    public Animation animToPlay;
    public float gizmoRadius = 1f;

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }


}
