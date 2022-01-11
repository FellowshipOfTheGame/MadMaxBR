using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsCollision : MonoBehaviour {
    private GameObject car; // the car this script is attached
    private float ThornsDamage; // the thorns PowerUp script

    public void Start() {
        car = this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
        ThornsDamage = this.gameObject.GetComponentInParent<ThornsPU>().ThornsDamage;
    }

    /// <summary>
    /// Causes damage to a car based on the relative speed of the colliding cars.
    /// </summary>
    /// <param name="other">The other car.</param>
    public void OnTriggerEnter(Collider other) {
        //Debug.Log("collision with " + other.gameObject.name);
        if (other.gameObject.CompareTag("AI") || other.gameObject.CompareTag("Player")) { // if car collides with other car, be it an AI or Player
            GameObject otherCar = other.gameObject; // get object of the car that got hit
            float collisionDamageModifier = Mathf.Abs(car.GetComponent<CarController>().CurrentSpeed - otherCar.GetComponent<CarController>().CurrentSpeed);
            otherCar.GetComponentInParent<VehicleData>().ReceiveDamage(ThornsDamage + collisionDamageModifier, car); // decreases health of the car
            Debug.Log(other.gameObject.name + " Received " + ThornsDamage + " damage");
        }
    }   
}
