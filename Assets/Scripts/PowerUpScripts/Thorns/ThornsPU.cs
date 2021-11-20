using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsPU : MonoBehaviour {
    private Timer stopwatch;
    // duration of power up in Minutes
    public float DurationMin;
    // durations of power up in Seconds
    public float DurationSec;
    // damage of thorns when it collides
    public float ThornCollisionDamage;

    void Awake() {
        stopwatch = gameObject.AddComponent<Timer>();
    }

    private void Update() {
        if (stopwatch.GetMinutes() >= DurationMin && stopwatch.GetSeconds() >= DurationSec) {
            this.gameObject.SetActive(false);
            Debug.Log("Deactivated Thorns after " + stopwatch.GetMinutes() + ":" + stopwatch.GetSeconds());
            //NitroObject.SetActive(false);
            this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Thorns);
        }
    }

    public void Activate() {
        stopwatch.ResetTimer();
    }
}
