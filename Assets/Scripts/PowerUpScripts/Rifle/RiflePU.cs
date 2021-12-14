using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePU : MonoBehaviour {
    public GameObject MachineGunHUD; // MachineGun HUD
    public Weapon WeaponManager;

    private GameObject targetCar; // the car this script is attached

    public void Activate() {
        MachineGunHUD.SetActive(true);
        WeaponManager.PegarPoweUpArma(1);
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        MachineGunHUD.SetActive(false);
        WeaponManager.PegarPoweUpArma(0);
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Rifle);
    }

    public float GetBulletAmount() {
        return WeaponManager.GetMunicao();
    }

    private void Update() {
        if (WeaponManager.GetMunicao() == 0) {
            Deactivate();
        }
    }
}
