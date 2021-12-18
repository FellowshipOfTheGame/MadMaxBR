using System;
using System.Diagnostics.SymbolStore;
using UnityEngine;

internal enum DriverMode
{
    Player,
    AI
}

[RequireComponent(typeof (CarController))]
public class CarUserControl : MonoBehaviour {
    [SerializeField] DriverMode driverMode = DriverMode.Player;
    private CarController m_Car; // the car controller we want to use
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private bool handbrake = false;
    private CarMovementAI carMovementAI;

    public bool ControlActive;

    private void Awake() {
        // get the car controller
        m_Car = GetComponent<CarController>();
        carMovementAI = GetComponent<CarMovementAI>();
    }

    private void Update() {
        if (driverMode == DriverMode.Player) {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            handbrake = Input.GetKeyDown(KeyCode.Space);
        } else if (driverMode == DriverMode.AI) {
            horizontalInput = carMovementAI.steer;
            verticalInput = carMovementAI.throttle;
            handbrake = carMovementAI.brake;
        }
    }

    private void FixedUpdate() {
        // pass the input to the car!
        if (ControlActive) {
            m_Car.Move(horizontalInput, verticalInput, verticalInput, handbrake);
        }
        
        //m_Car.Move(h, v, v, 0f);

    }
}