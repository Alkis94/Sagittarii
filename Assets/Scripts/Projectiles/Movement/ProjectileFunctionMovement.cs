using UnityEngine;
using System;

public class ProjectileFunctionMovement : MonoBehaviour
{
    private Vector3 originalPosition;
    private Func<float,float> SomeFunction;
    private float t = 0;
    private float theta;
    private float speed;


    private void OnEnable()
    {
        GetComponent<ProjectileImpact>().OnCollision += DisableComponent;
    }

    private void OnDisable()
    {
        GetComponent<ProjectileImpact>().OnCollision -= DisableComponent;
    }

    private void DisableComponent()
    {
        enabled = false;
    }


    // Use this for initialization
    void Start()
    {
        originalPosition = transform.position;
        theta = transform.eulerAngles.z * Mathf.Deg2Rad;
        speed = GetComponent<ProjectileDataInitializer>().Speed;
        SomeFunction = Mathf.Sin;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = originalPosition + RotatedFunctionPoint();
    }

    private Vector3 RotatedFunctionPoint()
    {
        t += Time.deltaTime * speed;
        //float ft = Mathf.Sin(t);
        float ft = SomeFunction(t);
        float nextX = t * Mathf.Cos(theta) - ft * Mathf.Sin(theta);
        float nextY = t * Mathf.Sin(theta) + ft * Mathf.Cos(theta);
        return new Vector3(nextX, nextY, 0);
    }

    

}
