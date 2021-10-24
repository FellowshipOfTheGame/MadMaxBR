using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPoint : MonoBehaviour {
    private void OnTriggerEnter(Collider col) {
        /*
        if (col.CompareTag("Player") || col.CompareTag("AI")) {
            col.GetComponent<VehicleRaceData>().TriggerPoint = this;
        }
        */
    }
}
