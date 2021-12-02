using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePU : MonoBehaviour {
    public float MaxBulletAmount; // maximum amount of bullets
    public float UsePerSecondRaw; // use of nitro per second
    public GameObject MachineGunHUD; // MachineGun HUD

    private float bulletAmount; // amount of car nitro
    private GameObject targetCar; // the car this script is attached

    public void Activate() {
        bulletAmount = MaxBulletAmount; // set maximum bullet amount
        MachineGunHUD.SetActive(true);
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        MachineGunHUD.SetActive(false);
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Rifle);
    }

    public float GetBulletAmount() {
        return (int)bulletAmount;
    }

    private void Update() {
        if (bulletAmount == 0) {
            Deactivate();
        } else {
            if (Input.GetKey(KeyCode.Mouse0)) {
                bulletAmount = Mathf.MoveTowards(bulletAmount, 0f, Time.deltaTime * UsePerSecondRaw);
            }
        }
    }
}