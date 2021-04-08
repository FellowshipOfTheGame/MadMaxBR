using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class CarController : MonoBehaviour
{
    public InputManager inputManager;
    public List<WheelCollider> throttleWheels;
    public List<WheelCollider> steeringWheels;
    public float maxMotorTorque = 800f;
    public float maxSteerAngle = 45f;
    public float brakeStrength = 1000f;
    public float currentSpeed;
    public float maxSpeed = 150f;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    void FixedUpdate()
    {
        Steer();
        Drive();
        Braking();
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * throttleWheels[0].radius * throttleWheels[0].rpm * 60 / 1000;

        foreach (WheelCollider wheel in throttleWheels)
        {
            if (currentSpeed < maxSpeed && !inputManager.braking)
            {
                wheel.motorTorque = maxMotorTorque * inputManager.throttle;
            }
            else
            {
                wheel.motorTorque = 0f;
            }
        }
    }

    private void Braking()
    {
        foreach (WheelCollider wheel in throttleWheels)
        {
            if (inputManager.braking)
            {
                wheel.brakeTorque = brakeStrength;
            }
            else
            {
                wheel.brakeTorque = 0f;
            }
        }
    }

    private void Steer()
    {
        foreach (WheelCollider wheel in steeringWheels)
        {
            wheel.steerAngle = maxSteerAngle * inputManager.steer;
        }
    }
}
