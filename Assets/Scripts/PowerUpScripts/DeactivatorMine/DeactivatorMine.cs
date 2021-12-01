using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatorMine : MonoBehaviour {
    private Timer stopwatch;
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
    /// Randomly get the index of a filled powerup slot of a car to be be deactivated.
    /// Return 0 if no powerup slot is filled.
    /// </summary>
    /// <param name="car"></param>
    /// <returns></returns>
    private int DrawSlotIndex(GameObject car) {
        int index = 0;
        int randval = Random.Range(1, 5);
        for (int i = 0; i < 4; i++) {
            if (car.GetComponent<VehicleData>().GetPowerUpSlotValue(1 + ((randval + i) % 4)) != -1) {
                index = 1 + ((randval + i) % 4);
                break;
            }
        }
        return index;
    }

    /// <summary>
    /// Makes the mine explode when a car enter in its detection zone.
    /// </summary>
    /// <param name="other">The other car.</param>
    private void OnTriggerEnter(Collider other) {
        if (isActive) {
            if (other.gameObject.CompareTag("AI") || other.gameObject.CompareTag("Player")) { // verify if object colliding is a car                                                                //GameObject otherCar = other.transform.parent.transform.parent.gameObject; // get object of the car that got hit
                GameObject otherCar = other.gameObject; // get object of the car that got hit
                otherCar.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(DrawSlotIndex(otherCar));
                Destroy(this.transform.parent.gameObject);
            }
        }
    }
}
