﻿using UnityEngine;
using System.Collections;

public class RaceCamera : MonoBehaviour {

    public GameObject Target;
    public float CircleScale = 5;
    public float MaxDistance = 10;

    private Vector3 Velocity;
    new Transform transform;
    private Transform targetTransform;
    private Rigidbody targetRigidbody;

    // Use this for initialization
    void Start ()
    {
        transform = GetComponent<Transform>();
        targetTransform = Target.GetComponent<Transform>();
        targetRigidbody = Target.GetComponent<Rigidbody>();
        Velocity = Vector3.zero;
		transform.SetParent (null);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(targetTransform);
        Vector3 newVel = targetRigidbody.velocity.x * Vector3.right + targetRigidbody.velocity.z * Vector3.forward;
        Vector3 newPos = transform.position;
        if (newVel.sqrMagnitude > 1)
        {
            if ((newVel * CircleScale).sqrMagnitude > MaxDistance * MaxDistance)
            {
                newPos = targetRigidbody.position - newVel.normalized * MaxDistance + Vector3.up * 4;
            }
            else
            {
                newPos = targetRigidbody.position - newVel * CircleScale + Vector3.up * 4;
            }
        }
        
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 3);
    }
}
