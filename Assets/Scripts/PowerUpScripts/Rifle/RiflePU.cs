using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePU : MonoBehaviour {
    [SerializeField] private GameObject MachineGunHUD; // MachineGun HUD
    [SerializeField] private GameObject WeaponManager;

    private GameObject targetCar; // the car this script is attached

    public PowerUpData PowerUpInfo;

    public void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached
        MachineGunHUD.SetActive(true);
        WeaponManager.GetComponent<Weapon>().PegarPoweUpArma(1);
        WeaponManager.GetComponent<WeaponMovement>().StartMovementUp();
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        MachineGunHUD.SetActive(false);
        WeaponManager.GetComponent<Weapon>().PegarPoweUpArma(1);
        WeaponManager.GetComponent<Weapon>().CanShoot = false;
        WeaponManager.GetComponent<WeaponMovement>().StartMovementDown();
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Rifle);
    }

    public float GetBulletAmount() {
        return WeaponManager.GetComponent<Weapon>().GetMunicao();
    }

    private void Update() {
        if (WeaponManager.GetComponent<WeaponMovement>().IsAtMaxHeight) {
            WeaponManager.GetComponent<Weapon>().CanShoot = true;
        }
        if (WeaponManager.GetComponent<Weapon>().GetMunicao() == 0) {
            Deactivate();
        }
    }
}
