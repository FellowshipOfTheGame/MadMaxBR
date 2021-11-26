using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPU : MonoBehaviour {
    private Timer stopwatch;
    // percentage of life healed per second
    public float HealPerSecond;
    // duration of power up in Seconds
    public float DurationSec;
    // car with the powerUp
    private GameObject targetCar;
    // number of times the car was healed while the powerUp is active
    private int timesHealed;
    void Awake() {
        stopwatch = gameObject.AddComponent<Timer>();
    }

    public void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        stopwatch.ResetTimer();
        timesHealed = 0;
    }

    private void Update() {
        if (timesHealed < DurationSec) {
            if (stopwatch.GetSeconds() >= 1) {
                targetCar.GetComponent<VehicleData>().AddHealth(targetCar.GetComponent<VehicleData>().MaxCarHealth * HealPerSecond / 100);
                timesHealed++;
                stopwatch.ResetTimer();
            }
        } else {
            // unequip powerup
            this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Fix);
            // set powerup gameObject inactive
            this.gameObject.SetActive(false);
        }
    }
}
