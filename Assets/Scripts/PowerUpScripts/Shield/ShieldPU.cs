using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShieldPU : MonoBehaviour {
    private GameObject targetCar;
    private void Update() {
        if (targetCar.GetComponent<VehicleData>().GetCurrentShield() <= 0) {
            // unequip powerup
            this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Shield);
            // set powerup gameObject inactive
            this.gameObject.SetActive(false);
        }
    }

    public void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        targetCar.GetComponent<VehicleData>().AddShield(200);
    }
}
