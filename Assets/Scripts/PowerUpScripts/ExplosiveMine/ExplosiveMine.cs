using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : MonoBehaviour {
    private Timer stopwatch;
    // raw damage of mine
    public float MineDamage;
    // time in milliseconds for mine to be activated
    public float MillisecondsToActivate;
    // time in milliseconds for mine to explode after detection
    public float MillisecondsToExplode;
    // 
    public GameObject ExplosionEffect;

    private GameObject otherCar; // car that was detected

    private bool isActive;
    private bool hasDetected;

    private void Start() {
        stopwatch = gameObject.AddComponent<Timer>();
        isActive = false;
        hasDetected = false;
        Debug.Log("MillisecondsToExplode: " + MillisecondsToExplode);
        Debug.Log("TimeToActivate: " + MillisecondsToActivate);
    }

    private void Update() {
        //Debug.Log(stopwatch.GetSeconds() + stopwatch.GetMilliseconds() / 1000);
        if (!isActive) {
            if (stopwatch.GetSeconds() * 1000 + stopwatch.GetMilliseconds() >= MillisecondsToActivate) {
                float a = stopwatch.GetSeconds() * 1000 + stopwatch.GetMilliseconds();
                Debug.Log("Ativou em " + a);
                isActive = true;
            }
        }
    }

    /// <summary>
    /// Makes the mine explode when a car enter in its detection zone.
    /// </summary>
    /// <param name="other">The other car.</param>
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("AI") || other.gameObject.CompareTag("Player")) { // verify if object colliding is a car
            Debug.Log("Detectou");
            hasDetected = true;
            stopwatch.ResetTimer();
        }
        /*
        if (isActive) {
            Debug.Log("Active");
            if (!hasDetected) {
                if (other.gameObject.CompareTag("AI") || other.gameObject.CompareTag("Player")) { // verify if object colliding is a car
                    Debug.Log("Detectou");
                    hasDetected = true;
                    stopwatch.ResetTimer();
                }
            } else {
                float a = stopwatch.GetSeconds() * 1000 + stopwatch.GetMilliseconds();
                Debug.Log(a + " >= " + MillisecondsToExplode);
                if (stopwatch.GetSeconds() * 1000 + stopwatch.GetMilliseconds() >= MillisecondsToExplode) {
                    float ab = stopwatch.GetSeconds() * 1000 + stopwatch.GetMilliseconds();
                    Debug.Log("Explodiu em " + ab);
                    GameObject otherCar = other.gameObject; // get object of the car that was detected
                    Instantiate(ExplosionEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
                    otherCar.GetComponentInParent<VehicleData>().ReceiveDamage(MineDamage); // decreases health of the car
                    Destroy(this.transform.parent.gameObject);
                }
            }
        }
        */
    }

    private void OnTriggerStay(Collider other) {
        if (isActive) {
            if (stopwatch.GetSeconds() * 1000 + stopwatch.GetMilliseconds() >= MillisecondsToExplode && (other.gameObject.CompareTag("AI") || other.gameObject.CompareTag("Player"))) { // verify if object colliding is a car
                float ab = stopwatch.GetSeconds() * 1000 + stopwatch.GetMilliseconds();
                Debug.Log("Explodiu em " + ab);
                GameObject otherCar = other.gameObject; // get object of the car that was detected
                Instantiate(ExplosionEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
                otherCar.GetComponentInParent<VehicleData>().ReceiveDamage(MineDamage); // decreases health of the car
                Destroy(this.transform.parent.gameObject);
            }
        }
    }

}
