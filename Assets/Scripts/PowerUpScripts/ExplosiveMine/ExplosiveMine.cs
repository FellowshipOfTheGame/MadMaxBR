using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : MonoBehaviour {
    private Timer stopwatch;
    // raw damage of mine
    public float MineDamage;
    // time in seconds for mine to be activated
    public float ActivationTime;
    
    private bool isActive;
    void Awake() {
        stopwatch = gameObject.AddComponent<Timer>();
        isActive = false;
    }

    private void Update() {
        if (!isActive) {
            if (stopwatch.GetSeconds() >= ActivationTime) {
                isActive = true;
            }
        }
    }

    /// <summary>
    /// Makes the mine explode when a car enter in its detection zone.
    /// </summary>
    /// <param name="other">The other car.</param>
    private void OnTriggerEnter(Collider other) {
        if (isActive) {
            if (other.gameObject.CompareTag("AI") || other.gameObject.CompareTag("Player")) { // verify if object colliding is a car                                                                //GameObject otherCar = other.transform.parent.transform.parent.gameObject; // get object of the car that got hit
                GameObject otherCar = other.gameObject; // get object of the car that got hit
                if (otherCar != null) {
                    Debug.Log("otherCar = " + otherCar.gameObject.name);
                }
                otherCar.GetComponentInParent<VehicleData>().ReceiveDamage(MineDamage); // decreases health of the car
                Destroy(this.transform.parent.gameObject);
            }
        }
        //Debug.Log(other.attachedRigidbody.mass);
    }
}
