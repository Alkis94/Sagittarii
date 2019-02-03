using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField]
    private float maxJumpHeight = 4;
    [SerializeField]
    private float minJumpHeight = 1;
    [SerializeField]
    private float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;

    [HideInInspector]
    public float value;
    [HideInInspector]
    public float maxJumpVelocity;
    [HideInInspector]
    public float minJumpVelocity;
    

    void Start()
    {
        value = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(value) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(value) * minJumpHeight);
    }


}
