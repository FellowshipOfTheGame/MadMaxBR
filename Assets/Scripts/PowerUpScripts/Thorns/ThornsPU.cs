using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsPU : MonoBehaviour {
    private Timer stopwatch;
    // duration of power up in Minutes
    public float DurationMin;
    // duration of power up in Seconds
    public float DurationSec;
    // damage of thorns when it collides
    public float ThornCollisionDamage;

    public GameObject ThornsHUD;

    void Awake() {
        stopwatch = gameObject.AddComponent<Timer>();
    }

    public void Activate() {
        stopwatch.ResetTimer();
        ThornsHUD.SetActive(true);
    }

    public void Deactivate() {
        // unequip powerup
        this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Thorns);
        // set powerup gameObject inactive
        this.gameObject.SetActive(false);
        // set thorns hud
        ThornsHUD.SetActive(false);
    }
    /// <summary>
    /// Returns time in seconds that this powerup has been active.
    /// </summary>
    /// <returns></returns>
    public float GetRunningTime() {
        return stopwatch.GetMinutes() * 60 + stopwatch.GetSeconds() + stopwatch.GetMilliseconds() / 1000;
    }
    /// <summary>
    /// Returns max time this powerup can be active.
    /// </summary>
    /// <returns></returns>
    public float GetMaxTime() {
        return DurationMin * 60 + DurationSec;
    }

    private void Update() {
        if (stopwatch.GetMinutes() >= DurationMin && stopwatch.GetSeconds() >= DurationSec) {
            Deactivate();
        }
    }
}
