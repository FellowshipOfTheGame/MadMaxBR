using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible to activate a PowerUp object when a car with this script attached collides with a PowerUp Trigger.
/// </summary>
public class PowerUpActivation : MonoBehaviour {

    public GameObject MachineGun;
    public GameObject Rifle;
    public GameObject Thorns;
    public GameObject Shield;
    public GameObject Fix;
    public GameObject Smoke;
    public GameObject ExplosiveMine;
    public GameObject DeactivatorMine;
    public GameObject Pillar;
    public GameObject Nitro;
    public GameObject Grease;
    public GameObject Glue;
    
    private VehicleData vehicleData;
    private MachineGunPU machineGunPowerUp;
    private RiflePU riflePowerUp;
    private ThornsPU thornsPowerUp;
    private ShieldPU shieldPowerUp;
    private FixPU fixPowerUp;
    private SmokePU smokePowerUp;
    private ExplosiveMinePU explosiveMinePowerUp;
    private DeactivatorMinePU deactivatorMinePowerUp;
    private PillarPU pillarPowerUp;
    private NitroPU nitroPowerUp;
    private GreasePU greasePowerUp;
    private GluePU gluePowerUp;

    private void Awake()
    {
        vehicleData = GetComponent<VehicleData>();
        machineGunPowerUp = MachineGun.GetComponent<MachineGunPU>();
        riflePowerUp = Rifle.GetComponent<RiflePU>();
        thornsPowerUp = Thorns.GetComponent<ThornsPU>();
        shieldPowerUp = Shield.GetComponent<ShieldPU>();
        fixPowerUp = Fix.GetComponent<FixPU>();
        smokePowerUp = Smoke.GetComponent<SmokePU>();
        explosiveMinePowerUp = ExplosiveMine.GetComponent<ExplosiveMinePU>();
        deactivatorMinePowerUp = DeactivatorMine.GetComponent<DeactivatorMinePU>();
        pillarPowerUp = Pillar.GetComponent<PillarPU>();
        nitroPowerUp = Nitro.GetComponent<NitroPU>();
        greasePowerUp = Grease.GetComponent<GreasePU>();
        gluePowerUp = Glue.GetComponent<GluePU>();
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("MachineGunPU")) { // MachineGun is stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.MachineGun)) {
                vehicleData.FillPowerUpSlot(PowerUpName.MachineGun);
                MachineGun.SetActive(true);
                machineGunPowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores smoke isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.MachineGun) == (int)PowerUpName.MachineGun) {
                    machineGunPowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("RiflePU")) { // MachineGun is stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.Rifle)) {
                vehicleData.FillPowerUpSlot(PowerUpName.Rifle);
                Rifle.SetActive(true);
                riflePowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores smoke isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.Rifle) == (int)PowerUpName.Rifle) {
                    riflePowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("ThornsPU")) { // Thorns is stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.Thorns)) {
                vehicleData.FillPowerUpSlot(PowerUpName.Thorns);
                Thorns.SetActive(true);
                thornsPowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores Thorns isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.Thorns) == (int)PowerUpName.Thorns) {
                    thornsPowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("ShieldPU")) { // Shield is stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.Shield)) {
                vehicleData.FillPowerUpSlot(PowerUpName.Shield);
                Shield.SetActive(true);
                shieldPowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores shield isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.Shield) == (int)PowerUpName.Shield) {
                    shieldPowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("FixPU")) { // Fix is not stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.Fix)) {
                vehicleData.FillPowerUpSlot(PowerUpName.Fix);
                Fix.SetActive(true);
                fixPowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores shield isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.Fix) == (int)PowerUpName.Fix) {
                    fixPowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("SmokePU")) { // Smoke is stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.Smoke)) {
                vehicleData.FillPowerUpSlot(PowerUpName.Smoke);
                Smoke.SetActive(true);
                smokePowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores smoke isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.Smoke) == (int)PowerUpName.Smoke) {
                    smokePowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("ExplosiveMinePU")) { // Explosive Mine is stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.ExplosiveMine)) { // if the ExplosiveMine slot is free
                vehicleData.FillPowerUpSlot(PowerUpName.ExplosiveMine);
                ExplosiveMine.SetActive(true);
                explosiveMinePowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform(); // deactivate the PowerUp platform
            } else { // if the power up slot that stores Explosive Mine isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.ExplosiveMine) == (int)PowerUpName.ExplosiveMine) {
                    explosiveMinePowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("DeactivatorMinePU")) { // Explosive Mine is stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.DeactivatorMine)) { // if the ExplosiveMine slot is free
                vehicleData.FillPowerUpSlot(PowerUpName.DeactivatorMine);
                DeactivatorMine.SetActive(true);
                deactivatorMinePowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform(); // deactivate the PowerUp platform
            } else { // if the power up slot that stores Explosive Mine isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.DeactivatorMine) == (int)PowerUpName.DeactivatorMine) {
                    deactivatorMinePowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("PillarPU")) { // Pillar is stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.Pillar)) { // if the ExplosiveMine slot is free
                vehicleData.FillPowerUpSlot(PowerUpName.Pillar);
                Pillar.SetActive(true);
                pillarPowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform(); // deactivate the PowerUp platform
            } else { // if the power up slot that stores Explosive Mine isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.Pillar) == (int)PowerUpName.Pillar) {
                    pillarPowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("NitroPU")) { // Nitro is stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.Nitro)) {
                vehicleData.FillPowerUpSlot(PowerUpName.Nitro);
                Nitro.SetActive(true);
                nitroPowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores nitro isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.Nitro) == (int)PowerUpName.Nitro) {
                    nitroPowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("GreasePU")) { // Grease is stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.Grease)) {
                vehicleData.FillPowerUpSlot(PowerUpName.Grease);
                Grease.SetActive(true);
                greasePowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores smoke isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.Grease) == (int)PowerUpName.Grease) {
                    greasePowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("GluePU")) { // Glue is stored in a slot
            if (vehicleData.PowerUpSlotFree(PowerUpName.Glue)) {
                vehicleData.FillPowerUpSlot(PowerUpName.Glue);
                Glue.SetActive(true);
                gluePowerUp.Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores smoke isnt free
                if (vehicleData.GetPowerUpSlotValue(PowerUpName.Glue) == (int)PowerUpName.Glue) {
                    gluePowerUp.Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

    }
}
