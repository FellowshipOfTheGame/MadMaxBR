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

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("MachineGunPU")) { // MachineGun is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.MachineGun)) {
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.MachineGun);
                MachineGun.SetActive(true);
                MachineGun.GetComponent<MachineGunPU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores smoke isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.MachineGun) == (int)PowerUpName.MachineGun) {
                    MachineGun.GetComponent<MachineGunPU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("RiflePU")) { // MachineGun is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.Rifle)) {
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.Rifle);
                Rifle.SetActive(true);
                Rifle.GetComponent<RiflePU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores smoke isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.Rifle) == (int)PowerUpName.Rifle) {
                    Rifle.GetComponent<RiflePU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("ThornsPU")) { // Thorns is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.Thorns)) {
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.Thorns);
                Thorns.SetActive(true);
                Thorns.GetComponent<ThornsPU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores Thorns isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.Thorns) == (int)PowerUpName.Thorns) {
                    Thorns.GetComponent<ThornsPU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("ShieldPU")) { // Shield is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.Shield)) {
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.Shield);
                Shield.SetActive(true);
                Shield.GetComponent<ShieldPU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores shield isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.Shield) == (int)PowerUpName.Shield) {
                    Shield.GetComponent<ShieldPU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("FixPU")) { // Fix is not stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.Fix)) {
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.Fix);
                Fix.SetActive(true);
                Fix.GetComponent<FixPU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores shield isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.Fix) == (int)PowerUpName.Fix) {
                    Fix.GetComponent<FixPU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("SmokePU")) { // Smoke is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.Smoke)) {
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.Smoke);
                Smoke.SetActive(true);
                Smoke.GetComponent<SmokePU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores smoke isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.Smoke) == (int)PowerUpName.Smoke) {
                    Smoke.GetComponent<SmokePU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("ExplosiveMinePU")) { // Explosive Mine is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.ExplosiveMine)) { // if the ExplosiveMine slot is free
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.ExplosiveMine);
                ExplosiveMine.SetActive(true);
                ExplosiveMine.GetComponent<ExplosiveMinePU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform(); // deactivate the PowerUp platform
            } else { // if the power up slot that stores Explosive Mine isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.ExplosiveMine) == (int)PowerUpName.ExplosiveMine) {
                    ExplosiveMine.GetComponent<ExplosiveMinePU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("DeactivatorMinePU")) { // Explosive Mine is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.DeactivatorMine)) { // if the ExplosiveMine slot is free
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.DeactivatorMine);
                DeactivatorMine.SetActive(true);
                DeactivatorMine.GetComponent<DeactivatorMinePU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform(); // deactivate the PowerUp platform
            } else { // if the power up slot that stores Explosive Mine isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.DeactivatorMine) == (int)PowerUpName.DeactivatorMine) {
                    DeactivatorMine.GetComponent<DeactivatorMinePU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("PillarPU")) { // Pillar is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.Pillar)) { // if the ExplosiveMine slot is free
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.Pillar);
                Pillar.SetActive(true);
                Pillar.GetComponent<PillarPU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform(); // deactivate the PowerUp platform
            } else { // if the power up slot that stores Explosive Mine isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.Pillar) == (int)PowerUpName.Pillar) {
                    Pillar.GetComponent<PillarPU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("NitroPU")) { // Nitro is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.Nitro)) {
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.Nitro);
                Nitro.SetActive(true);
                Nitro.GetComponent<NitroPU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores nitro isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.Nitro) == (int)PowerUpName.Nitro) {
                    Nitro.GetComponent<NitroPU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("GreasePU")) { // Grease is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.Grease)) {
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.Grease);
                Grease.SetActive(true);
                Grease.GetComponent<GreasePU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores smoke isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.Grease) == (int)PowerUpName.Grease) {
                    Grease.GetComponent<GreasePU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

        if (collider.gameObject.CompareTag("GluePU")) { // Glue is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.Glue)) {
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.Glue);
                Glue.SetActive(true);
                Glue.GetComponent<GluePU>().Activate();
                collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
            } else { // if the power up slot that stores smoke isnt free
                if (gameObject.GetComponent<VehicleData>().GetPowerUpSlotValue(PowerUpName.Glue) == (int)PowerUpName.Glue) {
                    Glue.GetComponent<GluePU>().Activate();
                    collider.GetComponentInParent<PowerUpPlatform>().DeactivatePowerUpPlatform();
                }
            }
        }

    }
}
