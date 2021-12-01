using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreasePU : MonoBehaviour {
    public float MaxGreaseAmount; // maximum amount of grease
    public float UsePerSecond; // use of grease per second in percentage
    public GameObject GreaseHUD; // grease hud

    private float curGreaseAmount; // amount of Grease
    private GameObject targetCar; // the car this script is attached

    public GameObject LiquidSpiller;

    public float GetGreaseAmount() {
        return curGreaseAmount;
    }

    // Update is called once per frame
    private void Update() {
        if (curGreaseAmount == 0) {
            this.gameObject.SetActive(false);
            GreaseHUD.SetActive(false);
            LiquidSpiller.GetComponent<ParticleSystem>().Stop();
            targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Grease);
        } else {
            if (Input.GetKey(KeyCode.LeftShift)) {
                LiquidSpiller.GetComponent<ParticleSystem>().Play();
                curGreaseAmount = Mathf.MoveTowards(curGreaseAmount, 0f, Time.deltaTime * MaxGreaseAmount * UsePerSecond / 100);
            } else {
                LiquidSpiller.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    public void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        curGreaseAmount = MaxGreaseAmount;
        GreaseHUD.SetActive(true);
    }
}
