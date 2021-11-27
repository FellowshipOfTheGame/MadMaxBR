using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NitroPU : MonoBehaviour {
    public float MaxNitroAmount; // maximum amount of nitro
    public float UsePerSecond; // use of nitro per second in percentage
    public GameObject NitroHUD; // nitro HUD

    private float curNitroAmount; // amount of car nitro
    private GameObject targetCar; // the car this script is attached

    public void UpdateNitroAmount(float amount) {
        curNitroAmount = amount;
    }

    public float GetNitroAmount() {
        return curNitroAmount;
    }

    public void Activate() {
        curNitroAmount = MaxNitroAmount; // set maximum value of nitro amount
        NitroHUD.SetActive(true);
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached
    }

    private void Update() {
        if (curNitroAmount == 0) {
            this.gameObject.SetActive(false);
            NitroHUD.SetActive(false);
            targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Nitro);
        } else {
            if (Input.GetKey(KeyCode.LeftShift)) {
                // CarController.IsNitroActive = true;
                curNitroAmount = Mathf.MoveTowards(curNitroAmount, 0f, Time.deltaTime * MaxNitroAmount * UsePerSecond / 100);
            }
        }
    }
}
