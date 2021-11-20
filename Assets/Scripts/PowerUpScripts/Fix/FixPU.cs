using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPU : MonoBehaviour {
    public void Activate() {
        GameObject targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        targetCar.GetComponent<VehicleData>().AddHealth(500);
    }
}
