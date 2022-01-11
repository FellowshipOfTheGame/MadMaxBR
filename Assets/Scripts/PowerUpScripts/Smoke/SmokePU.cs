using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible to implement the Smoke PowerUp logic.
/// </summary>
public class SmokePU : PowerUpBase {
    [SerializeField] private float maxSmokeAmount; // maximum amount of smoke
    [SerializeField] private float usePerSecond; // use of smoke per second in percentage

    private GameObject SmokeHUD; // smoke hud
    private float curSmokeAmount; // amount of car smoke
    private GameObject targetCar; // the car this script is attached

    public PowerUpData PowerUpInfo;

    public float MaxSmokeAmount { get { return maxSmokeAmount; } }

    public float CurSmokeAmount { get { return curSmokeAmount; } }
    private void Awake() {
        if (this.transform.parent.gameObject.transform.parent.gameObject.CompareTag("Player")) {
            SmokeHUD = PlayerDataDisplayer.Instance.SmokeHUD;
        } else {
            SmokeHUD = null;
        }
    }
    public override void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        curSmokeAmount = maxSmokeAmount;
        if (SmokeHUD != null) {
            SmokeHUD.SetActive(true);
        }
    }

    public override void Deactivate() {
        this.gameObject.SetActive(false);
        if (SmokeHUD != null) {
            SmokeHUD.SetActive(false);
        }
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Smoke);
    }

    public override void UsePowerUp(bool useActive) {
        if (curSmokeAmount != 0) {
            if (useActive) {
                targetCar.GetComponent<VehicleData>().SetInvulnerability(true);
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                curSmokeAmount = Mathf.MoveTowards(curSmokeAmount, 0f, Time.deltaTime * maxSmokeAmount * usePerSecond / 100);
            } else {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                targetCar.GetComponent<VehicleData>().SetInvulnerability(false);
            }
        }
    }

    public override void Update() {
        if (curSmokeAmount == 0) {
            Deactivate();
        }
    }
}
