using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfPointTriggerManager : MonoBehaviour {

    public GameObject LapCompleteTrigger;
    public GameObject HalfLapTrigger;

    private void OnTriggerEnter(Collider other) {
        LapCompleteTrigger.SetActive(true);
        HalfLapTrigger.SetActive(false);
    }
}
