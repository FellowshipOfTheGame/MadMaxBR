using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarCollision : MonoBehaviour {
    private GameObject owner;
    public float ThornsDamage;

    public GameObject Owner { 
        set {
            owner = value;
        } 
    }

    private void OnCollisionEnter(Collision other) {
        //Debug.Log(collision.gameObject.name + " collided with pillar");
        if (other.gameObject.CompareTag("AI") || other.gameObject.CompareTag("Player")) { // if car collides with other car, be it an AI or Player
            GameObject otherCar = other.gameObject; // get object of the car that got hit
            float collisionDamageModifier = otherCar.GetComponent<CarController>().CurrentSpeed;
            otherCar.GetComponentInParent<VehicleData>().ReceiveDamage(ThornsDamage + collisionDamageModifier, owner); // decreases health of the car
            Debug.Log(other.gameObject.name + " Received " + ThornsDamage + " damage");
        }
    }
}
