using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPU : PowerUpBase {
    // percentage of life healed per second
    [SerializeField] private float HealPerSecond;
    // duration of power up in Seconds
    [SerializeField] private float DurationSec;

    private Timer stopwatch;
    // car with the powerUp
    private GameObject targetCar;
    // number of times the car was healed while the powerUp is active
    private int timesHealed;

    public PowerUpData PowerUpInfo;
    void Awake() {
        stopwatch = gameObject.AddComponent<Timer>();
    }

    public override void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        stopwatch.ResetTimer();
        timesHealed = 0;
    }

    public override void Deactivate() {
        // unequip powerup
        this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Fix);
        // set powerup gameObject inactive
        this.gameObject.SetActive(false);
    }

    public override void Update() {
        if (timesHealed < DurationSec) {
            if (stopwatch.GetSeconds() >= 1) {
                targetCar.GetComponent<VehicleData>().AddHealth(targetCar.GetComponent<VehicleData>().MaxCarHealth * HealPerSecond / 100);
                timesHealed++;
                stopwatch.ResetTimer();
            }
        } else {
            Deactivate();
        }
    }
}
