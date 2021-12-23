using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShieldPU : PowerUpBase {
    private GameObject targetCar;
    // shield amount in percentage
    [SerializeField] private float ShieldAmountPercentage;
    // the hud shield bar
    [SerializeField] private GameObject ShieldHUD;

    public PowerUpData PowerUpInfo;
    public override void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        if (ShieldHUD != null) {
            ShieldHUD.SetActive(true);
        }
        targetCar.GetComponent<VehicleData>().AddShield(targetCar.GetComponent<VehicleData>().MaxCarShield * ShieldAmountPercentage / 100);
    }

    public override void Deactivate() {
        if (ShieldHUD != null) {
            ShieldHUD.SetActive(false);
        }
        // unequip powerup
        this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Shield);
        // set powerup gameObject inactive
        this.gameObject.SetActive(false);
    }

    public override void Update() {
        if (targetCar.GetComponent<VehicleData>().GetCurrentShield() <= 0) {
            Deactivate();
        }
    }
}
