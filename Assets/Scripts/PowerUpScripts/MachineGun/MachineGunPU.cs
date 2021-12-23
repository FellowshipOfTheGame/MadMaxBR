using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunPU : PowerUpBase {
    [SerializeField] private GameObject MachineGunHUD; // MachineGun HUD text
    [SerializeField] private GameObject WeaponManager;

    private GameObject targetCar; // the car this script is attached

    public PowerUpData PowerUpInfo;

    public int BulletAmount { get { return WeaponManager.GetComponent<Weapon>().GetMunicao(); } }

    public override void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached
        if (MachineGunHUD != null) {
            MachineGunHUD.SetActive(true);
        }
        WeaponManager.GetComponent<Weapon>().PegarPoweUpArma(2);
        WeaponManager.GetComponent<WeaponMovement>().StartMovementUp();
    }

    public override void Deactivate() {
        this.gameObject.SetActive(false);
        if (MachineGunHUD != null) {
            MachineGunHUD.SetActive(false);
        }
        WeaponManager.GetComponent<Weapon>().PegarPoweUpArma(2);
        WeaponManager.GetComponent<Weapon>().CanShoot = false;
        WeaponManager.GetComponent<WeaponMovement>().StartMovementDown();
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.MachineGun);
    }

    public override void UsePowerUp(bool useActive) {
        WeaponManager.GetComponent<Weapon>().Shooting = useActive;
    }

    public override void Update() {
        if (WeaponManager.GetComponent<WeaponMovement>().IsAtMaxHeight) {
            WeaponManager.GetComponent<Weapon>().CanShoot = true;
        }
        if (WeaponManager.GetComponent<Weapon>().GetMunicao() == 0) {
            Deactivate();
        }
    }
}
