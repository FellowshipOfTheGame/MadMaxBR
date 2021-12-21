using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsPU : MonoBehaviour {
    private Timer stopwatch;
    [SerializeField] private float durationSec;
    [SerializeField] private float thornCollisionDamage;
    [SerializeField] private float collisionDamageReduction;

    private GameObject targetCar; // the car this script is attached

    public PowerUpData PowerUpInfo;
    /// <summary>
    /// Base damage of thorns on collision.
    /// </summary>
    public float ThornsDamage { get { return thornCollisionDamage; } }
    /// <summary>
    /// Maximum time in seconds this powerup can be active.
    /// </summary>
    public float MaxTime { get { return durationSec; } }
    /// <summary>
    /// Collision damage reduction in percentage for the car this powerup is active.
    /// </summary>
    public float CollisionDamageReduction { get { return collisionDamageReduction; } }

    public GameObject ThornsHUD;

    void Awake() {
        stopwatch = gameObject.AddComponent<Timer>();
    }

    public void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        targetCar.GetComponent<VehicleData>().SetThornsArmor(true);
        stopwatch.ResetTimer();
        if (ThornsHUD != null) {
            ThornsHUD.SetActive(true);
        }
    }

    public void Deactivate() {
        targetCar.GetComponent<VehicleData>().SetThornsArmor(false);
        // unequip powerup
        this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Thorns);
        // set powerup gameObject inactive
        this.gameObject.SetActive(false);
        // set thorns hud
        if (ThornsHUD != null) {
            ThornsHUD.SetActive(false);
        }
    }
    /// <summary>
    /// Returns time in seconds that this powerup has been active.
    /// </summary>
    public float GetRunningTime() {
        return stopwatch.GetTimeInSeconds();
    }

    private void Update() {
        if (stopwatch.GetTimeInSeconds() >= durationSec) {
            Deactivate();
        }
    }
}
