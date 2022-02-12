using System;
using System.Collections;
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
        public KeyCode Slot1UseButton = KeyCode.Mouse0;
        public KeyCode Slot2UseButton = KeyCode.C;
        public KeyCode Slot3UseButton = KeyCode.X;
        public KeyCode Slot4UseButton = KeyCode.LeftShift;
    }

    private CarController m_Car; // the car controller we want to use
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private bool handbrake = false;
    private CarMovementAI carMovementAI;

    private VehicleData vehicleInfo;
    private GameObject playerPowerUps;

    public void SetAIControl(bool set) {
        if (set) {
            driverMode = DriverMode.AI;
        } else {
            driverMode = DriverMode.Player;
        }
    }

    private void Awake() {
        // get the car controller
        m_Car = GetComponent<CarController>();
        carMovementAI = GetComponent<CarMovementAI>();
        vehicleInfo = this.transform.GetComponentInChildren<VehicleData>();
        playerPowerUps = this.transform.GetComponentInChildren<PowerUp>().gameObject;
    }
    /// <summary>
    /// Verify if player has pressed any powerup button.
    /// </summary>
    private void VerifyPowerupPlayerInput() {
        if (vehicleInfo.GetPowerUpSlotValue(1) != -1) {
            if (vehicleInfo.GetPowerUpSlotValue(1) == (int)PowerUpName.MachineGun) {
                playerPowerUps.GetComponentInChildren<MachineGunPU>().UsePowerUp(Input.GetKey(PowerUpUseButtons.Slot1UseButton));
            } else if (vehicleInfo.GetPowerUpSlotValue(1) == (int)PowerUpName.Rifle) {
                playerPowerUps.GetComponentInChildren<RiflePU>().UsePowerUp(Input.GetKey(PowerUpUseButtons.Slot1UseButton));
            }
        }
        if (vehicleInfo.GetPowerUpSlotValue(2) == (int)PowerUpName.Smoke) {
            playerPowerUps.GetComponentInChildren<SmokePU>().UsePowerUp(Input.GetKey(PowerUpUseButtons.Slot2UseButton));
        }
        if (vehicleInfo.GetPowerUpSlotValue(3) != -1) {
            if (vehicleInfo.GetPowerUpSlotValue(3) == (int)PowerUpName.ExplosiveMine) {
                playerPowerUps.GetComponentInChildren<ExplosiveMinePU>().UsePowerUp(Input.GetKeyDown(PowerUpUseButtons.Slot3UseButton));
            } else if (vehicleInfo.GetPowerUpSlotValue(3) == (int)PowerUpName.DeactivatorMine) {
                playerPowerUps.GetComponentInChildren<DeactivatorMinePU>().UsePowerUp(Input.GetKeyDown(PowerUpUseButtons.Slot3UseButton));
            } else if (vehicleInfo.GetPowerUpSlotValue(3) == (int)PowerUpName.Pillar) {
                playerPowerUps.GetComponentInChildren<PillarPU>().UsePowerUp(Input.GetKeyDown(PowerUpUseButtons.Slot3UseButton));
            }
        }
        if (vehicleInfo.GetPowerUpSlotValue(4) != -1) {
            if (vehicleInfo.GetPowerUpSlotValue(4) == (int)PowerUpName.Grease) {
                playerPowerUps.GetComponentInChildren<GreasePU>().UsePowerUp(Input.GetKey(PowerUpUseButtons.Slot4UseButton));
            } else if (vehicleInfo.GetPowerUpSlotValue(4) == (int)PowerUpName.Glue) {
                playerPowerUps.GetComponentInChildren<GluePU>().UsePowerUp(Input.GetKey(PowerUpUseButtons.Slot4UseButton));
            } else if (vehicleInfo.GetPowerUpSlotValue(4) == (int)PowerUpName.Nitro) {
                playerPowerUps.GetComponentInChildren<NitroPU>().UsePowerUp(Input.GetKey(PowerUpUseButtons.Slot4UseButton));
            }
        }
    }

    private void VerifyPowerupAIInput()
    {
        if (vehicleInfo.GetPowerUpSlotValue(1) != -1)
        {
            if (vehicleInfo.GetPowerUpSlotValue(1) == (int)PowerUpName.MachineGun)
            {
                StartCoroutine(MachineGunActivator<MachineGunPU>());
            }
            else if (vehicleInfo.GetPowerUpSlotValue(1) == (int)PowerUpName.Rifle)
            {
                StartCoroutine(RifleActivator<RiflePU>());
            }
        }
        if (vehicleInfo.GetPowerUpSlotValue(2) == (int)PowerUpName.Smoke)
        {
            StartCoroutine(SmokeActivator<SmokePU>());
        }
        if (vehicleInfo.GetPowerUpSlotValue(3) != -1)
        {
            if (vehicleInfo.GetPowerUpSlotValue(3) == (int)PowerUpName.ExplosiveMine)
            {
                StartCoroutine(ExplosiveMineActivator<ExplosiveMinePU>());
            }
            else if (vehicleInfo.GetPowerUpSlotValue(3) == (int)PowerUpName.DeactivatorMine)
            {
                StartCoroutine(DeactivatorMineActivator<DeactivatorMinePU>());
            }
            else if (vehicleInfo.GetPowerUpSlotValue(3) == (int)PowerUpName.Pillar)
            {
                StartCoroutine(PillarActivator<PillarPU>());
            }
        }
        if (vehicleInfo.GetPowerUpSlotValue(4) != -1)
        {
            if (vehicleInfo.GetPowerUpSlotValue(4) == (int)PowerUpName.Grease)
            {
                StartCoroutine(GreaseActivator<GreasePU>());
            }
            else if (vehicleInfo.GetPowerUpSlotValue(4) == (int)PowerUpName.Glue)
            {
                StartCoroutine(GlueActivator<GluePU>());
            }
            else if (vehicleInfo.GetPowerUpSlotValue(4) == (int)PowerUpName.Nitro)
            {
                StartCoroutine(NitroActivator<NitroPU>());
            }
        }
    }

    private IEnumerator MachineGunActivator<T>() where T: MachineGunPU
    {
        T type = playerPowerUps.GetComponentInChildren<T>();
        type.UsePowerUp(true);
        yield return new WaitForFixedUpdate();
        type.UsePowerUp(false);
    }

    private IEnumerator RifleActivator<T>() where T : RiflePU
    {
        T type = playerPowerUps.GetComponentInChildren<T>();
        type.UsePowerUp(true);
        yield return new WaitForFixedUpdate();
        type.UsePowerUp(false);
    }

    private IEnumerator SmokeActivator<T>() where T : SmokePU
    {
        T type = playerPowerUps.GetComponentInChildren<T>();
        type.UsePowerUp(true);
        yield return new WaitForFixedUpdate();
        type.UsePowerUp(false);
    }

    private IEnumerator ExplosiveMineActivator<T>() where T : ExplosiveMinePU
    {
        T type = playerPowerUps.GetComponentInChildren<T>();
        type.UsePowerUp(true);
        yield return new WaitForFixedUpdate();
        type.UsePowerUp(false);
    }

    private IEnumerator DeactivatorMineActivator<T>() where T : DeactivatorMinePU
    {
        T type = playerPowerUps.GetComponentInChildren<T>();
        type.UsePowerUp(true);
        yield return new WaitForFixedUpdate();
        type.UsePowerUp(false);
    }

    private IEnumerator PillarActivator<T>() where T : PillarPU
    {
        T type = playerPowerUps.GetComponentInChildren<T>();
        type.UsePowerUp(true);
        yield return new WaitForFixedUpdate();
        type.UsePowerUp(false);
    }

    private IEnumerator GreaseActivator<T>() where T : GreasePU
    {
        T type = playerPowerUps.GetComponentInChildren<T>();
        type.UsePowerUp(true);
        yield return new WaitForFixedUpdate();
        if (type.CurGreaseAmount <= 0) 
            type.UsePowerUp(false);
    }

    private IEnumerator GlueActivator<T>() where T : GluePU
    {
        T type = playerPowerUps.GetComponentInChildren<T>();
        type.UsePowerUp(true);
        yield return new WaitForFixedUpdate();
        if (type.CurGlueAmount <= 0)
            type.UsePowerUp(false);
    }

    private IEnumerator NitroActivator<T>() where T : NitroPU
    {
        T type = playerPowerUps.GetComponentInChildren<T>();
        type.UsePowerUp(true);
        yield return new WaitForFixedUpdate();
        type.UsePowerUp(false);
    }

    private void Update() {
        if (driverMode == DriverMode.Player) {
            if (ControlActive) {
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");
                handbrake = Input.GetKeyDown(KeyCode.Space);
                VerifyPowerupPlayerInput();
            }
        } else if (driverMode == DriverMode.AI) {
            if (ControlActive) {
                horizontalInput = carMovementAI.steer;
                verticalInput = carMovementAI.throttle;
                handbrake = carMovementAI.brake;
                VerifyPowerupAIInput();
            }
        }
    }

    private void FixedUpdate() {
        // pass the input to the car!
        if (ControlActive) {
            
        }
        m_Car.Move(horizontalInput, verticalInput, verticalInput, handbrake);
        //m_Car.Move(h, v, v, 0f);
    }
}