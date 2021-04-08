using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroPU : MonoBehaviour {

    [HideInInspector]
    public float NitroAmount;

    private GameObject NitroBar;
    public GameObject NitroUI;

    public void UpdateNitroBar(float amount) {
        if (amount == 0) {
            NitroUI.SetActive(false);
        }
        NitroBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, amount * 2);
    }

    public void Activate() {
        NitroAmount = 100;
        NitroBar = NitroUI.transform.GetChild(0).gameObject; // gets the nitro bar of UI that represents the amount of nitro available
        NitroBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, NitroAmount * 2);
        NitroUI.SetActive(true);
    }
}
