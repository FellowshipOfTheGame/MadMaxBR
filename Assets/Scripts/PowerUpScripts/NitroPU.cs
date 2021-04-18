using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroPU : MonoBehaviour {
    private float NitroAmount; // amount of car nitro [0-100]

    private GameObject NitroBar;
    //public GameObject NitroUI;

    public void UpdateNitroAmount(float amount) {
        NitroAmount = amount;
        //NitroBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, amount * 2);
    }

    public float GetNitroAmount() {
        return NitroAmount;
    }

    public void Activate() {
        NitroAmount = 100;
    }
}
