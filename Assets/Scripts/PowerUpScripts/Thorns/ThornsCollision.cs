using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ThornsCollision : MonoBehaviour {
    private GameObject car; // the car this script is attached

    private void Start() {
        car = this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
    }
    /// <summary>
    /// Causes damage based on the relative speed of cars when the thorns first touch a vehicle.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.CompareTag("Car")) {
            Debug.Log("Bateu no carro com espinho");
        }
        Debug.Log("contatos1: " + collision.GetContacts(collision.contacts));
        Debug.Log("contatos2: " + collision.contacts);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("collision with " + other.gameObject.name);
        if (other.gameObject.CompareTag("IA")) {
            GameObject otherCar = other.transform.parent.transform.parent.gameObject; // get object of the car that got hit
            if (otherCar != null) {
                Debug.Log("otherCar = " + otherCar.gameObject.name);
            }
            float baseCollisionDamage = 100;
            float collisionDamageModifier = Mathf.Abs(car.GetComponent<CarController>().CurrentSpeed - otherCar.GetComponent<CarController>().CurrentSpeed);
            otherCar.GetComponentInParent<VehicleData>().ReceiveDamage(baseCollisionDamage/* + collisionDamageModifier*/); // decreases health of the car
            Debug.Log(other.gameObject.name + " Received " + baseCollisionDamage + " damage");
        }
        //Debug.Log(other.attachedRigidbody.mass);
    }   

    /// <summary>
    /// Causes damage over time when the thorns are touching 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision) {
        Debug.Log(collision.transform.gameObject.name);
    }

    private void OnTriggerStay(Collider other) {
        Debug.Log(other.name);
    }
}
