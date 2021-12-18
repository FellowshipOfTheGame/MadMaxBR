using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible to implement the Smoke PowerUp logic.
/// </summary>
public class SmokePU : MonoBehaviour {
    [SerializeField] private KeyCode useButton = KeyCode.C;
    [SerializeField] private float maxSmokeAmount; // maximum amount of smoke
    [SerializeField] private float usePerSecond; // use of smoke per second in percentage
    public GameObject SmokeHUD; // smoke hud

    private float curSmokeAmount; // amount of car smoke
    private GameObject targetCar; // the car this script is attached

    public PowerUpData PowerUpInfo;

    public float MaxSmokeAmount { get { return maxSmokeAmount; } }

    public float CurSmokeAmount { get { return curSmokeAmount; } }

    public void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        curSmokeAmount = maxSmokeAmount;
        SmokeHUD.SetActive(true);
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        SmokeHUD.SetActive(false);
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Smoke);
    }

    private void Update() {
        if (curSmokeAmount == 0) {
            Deactivate();
        } else {
            if (Input.GetKey(useButton)) {
                targetCar.GetComponent<VehicleData>().setSmokeActive(true);
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                curSmokeAmount = Mathf.MoveTowards(curSmokeAmount, 0f, Time.deltaTime * maxSmokeAmount * usePerSecond / 100);
            } else {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                targetCar.GetComponent<VehicleData>().setSmokeActive(false);

            }
        }
    }
}
