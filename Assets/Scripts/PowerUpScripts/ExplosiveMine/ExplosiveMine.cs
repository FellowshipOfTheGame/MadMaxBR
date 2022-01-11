using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : MonoBehaviour {
    // raw damage of mine
    [SerializeField] private float MineDamage;
    // time in milliseconds for mine to be activated
    [SerializeField] private float MillisecondsToActivate;
    // time in milliseconds for mine to explode after detection
    [SerializeField] private float MillisecondsToExplode;
    // explosion effect
    [SerializeField] private GameObject ExplosionEffect;
    // mine timer
    private Timer stopwatch;
    // car that planted this mine
    private GameObject owner;
    // car that was detected
    private GameObject otherCar;

    private bool isActive;
    private bool hasDetected;

    public GameObject Owner {
        set {
            owner = value;
        }
    }

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
                otherCar.GetComponentInParent<VehicleData>().ReceiveDamage(MineDamage, owner); // decreases health of the car
                Destroy(this.transform.parent.gameObject);
            }
        }
    }

}
