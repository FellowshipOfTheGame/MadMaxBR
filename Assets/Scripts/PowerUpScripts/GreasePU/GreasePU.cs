using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreasePU : MonoBehaviour {
    public float MaxGreaseAmount; // maximum amount of grease
    public float UsePerSecond; // use of grease per second in percentage
    public GameObject GreaseHUD; // grease hud
    public GameObject LiquidSpiller;

    private float curGreaseAmount; // amount of Grease
    private GameObject targetCar; // the car this script is attached

    public void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        curGreaseAmount = MaxGreaseAmount;
        GreaseHUD.SetActive(true);
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        GreaseHUD.SetActive(false);
        LiquidSpiller.GetComponent<ParticleSystem>().Stop();
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Grease);
    }

    public float GetGreaseAmount() {
        return curGreaseAmount;
    }

    // Update is called once per frame
    private void Update() {
        if (curGreaseAmount == 0) {
            Deactivate();
        } else {
            if (Input.GetKey(KeyCode.LeftShift)) {
                if (!LiquidSpiller.GetComponent<ParticleSystem>().isPlaying) {
                    LiquidSpiller.GetComponent<ParticleSystem>().Play();
                }
                curGreaseAmount = Mathf.MoveTowards(curGreaseAmount, 0f, Time.deltaTime * MaxGreaseAmount * UsePerSecond / 100);
            } else {
                if (!LiquidSpiller.GetComponent<ParticleSystem>().isStopped) {
                    LiquidSpiller.GetComponent<ParticleSystem>().Stop();
                }
            }
        }
    }
}
