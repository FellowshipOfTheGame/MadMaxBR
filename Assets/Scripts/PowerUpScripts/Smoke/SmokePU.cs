using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible to implement the Smoke PowerUp logic.
/// </summary>
public class SmokePU : MonoBehaviour {
    public float MaxSmokeAmount; // maximum amount of smoke
    public float UsePerSecond; // use of smoke per second in percentage
    public GameObject SmokeHUD; // smoke hud

    private float curSmokeAmount; // amount of car smoke
    private GameObject targetCar; // the car this script is attached

    public void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        curSmokeAmount = MaxSmokeAmount;
        SmokeHUD.SetActive(true);
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        SmokeHUD.SetActive(false);
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Smoke);
    }

    public float GetSmokeAmount() {
        return curSmokeAmount;
    }

    private void Update() {
        if (curSmokeAmount == 0) {
            Deactivate();
        } else {
            if (Input.GetKey(KeyCode.X)) {
                targetCar.GetComponent<VehicleData>().setSmokeActive(true);
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                curSmokeAmount = Mathf.MoveTowards(curSmokeAmount, 0f, Time.deltaTime * MaxSmokeAmount * UsePerSecond / 100);
            } else {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                targetCar.GetComponent<VehicleData>().setSmokeActive(false);

            }
        }
    }
}
