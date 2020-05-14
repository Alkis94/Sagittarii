using UnityEngine;
using System;

public class ProjectileFunctionMovement : MonoBehaviour
{
    private Vector3 originalPosition;
    private Func<float,float> SomeFunction;
    private FunctionMovementTypeEnum functionMovementType;
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
        functionMovementType = GetComponent<ProjectileDataInitializer>().FunctionMovementType;
        AssignCorrectFunctionType(functionMovementType);
    }

    private void AssignCorrectFunctionType(FunctionMovementTypeEnum functionMovementType)
    {
        switch(functionMovementType)
        {
            case FunctionMovementTypeEnum.straight:
                SomeFunction = StraightFunction;
                break;
            case FunctionMovementTypeEnum.sin:
                SomeFunction = Mathf.Sin;
                break;
            case FunctionMovementTypeEnum.root:
                SomeFunction = Mathf.Sqrt;
                break;
            case FunctionMovementTypeEnum.square:
                SomeFunction = Square;
                break;
            case FunctionMovementTypeEnum.sinCos:
                SomeFunction = SinCosFour;
                break;


        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = originalPosition + RotatedFunctionPoint();
    }

    private Vector3 RotatedFunctionPoint()
    {
        t += Time.deltaTime * speed;
        float ft = SomeFunction(t);
        float nextX = t * Mathf.Cos(theta) - ft * Mathf.Sin(theta);
        float nextY = t * Mathf.Sin(theta) + ft * Mathf.Cos(theta);
        return new Vector3(nextX, nextY, 0);
    }

    private float StraightFunction(float t)
    {
        return 0;
    }

    private float Square(float t)
    {
        return Mathf.Pow(t,2)/10;
    }

    private float SinCosFour(float t)
    {
        return (Mathf.Sin(t) + Mathf.Cos(2*t))/2;
    }

}
