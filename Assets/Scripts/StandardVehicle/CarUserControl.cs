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
    public bool ControlActive;

    [SerializeField] DriverMode driverMode = DriverMode.Player;

    public PowerUpUseBtn PowerUpUseButtons;

    [System.Serializable]
    public class PowerUpUseBtn {
        public KeyCode Slot1UseButton = KeyCode.X;
        public KeyCode Slot2UseButton = KeyCode.X;
        public KeyCode Slot3UseButton = KeyCode.X;
        public KeyCode Slot4UseButton = KeyCode.LeftShift;
    }

    private CarController m_Car; // the car controller we want to use
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private bool handbrake = false;
    private CarMovementAI carMovementAI;

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

            if (Input.GetKey(PowerUpUseButtons.Slot1UseButton)) {
                // machigun & rifle
            }
            if (Input.GetKey(PowerUpUseButtons.Slot2UseButton)) {
                // smoke
            }
            if (Input.GetKey(PowerUpUseButtons.Slot3UseButton)) {
                // explosive mine & deactivator mine & explosion mine
            }
            if (Input.GetKey(PowerUpUseButtons.Slot4UseButton)) {
                // nitro & grease & glue
            }

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