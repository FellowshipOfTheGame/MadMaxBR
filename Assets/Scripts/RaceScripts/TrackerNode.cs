using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class calculates de distance of each car from an specific point
public class TrackerNode : MonoBehaviour {
    // Update is called once per frame
    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Car")) {
            col.GetComponent<VehicleRaceData>().TrackerNode = this;
        }
    }

    public float GetDistance(GameObject car) {
        return Vector3.Distance(this.transform.position, car.transform.position);
    }
}
