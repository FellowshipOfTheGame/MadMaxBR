using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthText : MonoBehaviour {
    public GameObject Car;

    // Update is called once per frame
    void Update() {
        this.gameObject.GetComponent<TextMesh>().text = "" + Car.GetComponent<VehicleData>().GetCurrentHealth();
    }
}
