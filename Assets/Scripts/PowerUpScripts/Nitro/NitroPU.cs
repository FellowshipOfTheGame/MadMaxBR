using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroPU : MonoBehaviour {
    public GameObject NitroObject;

    private float NitroAmount; // amount of car nitro [0-100]
    private GameObject NitroBar;

    public void UpdateNitroAmount(float amount) {
        NitroAmount = amount;
        //NitroBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, amount * 2);
    }

    public float GetNitroAmount() {
        return NitroAmount;
    }

    public void Activate() {
        NitroAmount = 100;
        GameObject targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached
    }

    private void Update() {
        if (GetNitroAmount() == 0) {
            NitroObject.SetActive(false);
            gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Nitro);
        }
    }
}
