using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour {

    public GameObject Player;

    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player") || col.CompareTag("AI")) {
            Debug.Log("Entrou em " + Time.fixedTime);
            Debug.Log("Velocidade: " + Player.GetComponent<Rigidbody>().velocity.magnitude * 2);
        }
        
    }
}
