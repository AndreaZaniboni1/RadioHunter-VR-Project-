using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTargetReach : MonoBehaviour
{
    private float threshold = 0.02f;
    public Transform target;
    public UnityEvent OnReach;
    private bool wasReached= false;
    void FixedUpdate()
    {
        float distance =Vector3.Distance(transform.position, target.position);
        if(distance < threshold && !wasReached)
        {
            OnReach.Invoke();
            wasReached = true;
        }
        else if(distance >= threshold)
        {
            wasReached = false;
        }
    }
}
