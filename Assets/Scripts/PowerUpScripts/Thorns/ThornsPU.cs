using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsPU : MonoBehaviour {
    private Timer stopwatch;
    [SerializeField] private float DurationSec;
    [SerializeField] private float ThornCollisionDamage;

    public PowerUpData PowerUpInfo;
    /// <summary>
    /// Base damage of thorns on collision.
    /// </summary>
    public float ThornsDamage { get { return ThornCollisionDamage; } }
    /// <summary>
    /// Maximum time in seconds this powerup can be active.
    /// </summary>
    public float MaxTime { get { return DurationSec; } }
    
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
    public float GetRunningTime() {
        return stopwatch.GetTimeInSeconds();
    }

    private void Update() {
        if (stopwatch.GetTimeInSeconds() >= DurationSec) {
            Deactivate();
        }
    }
}
