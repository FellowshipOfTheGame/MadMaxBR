using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible to activate a PowerUp object when a car this script is attached collides with a PowerUp Trigger.
/// </summary>
public class PowerUpActivation : MonoBehaviour {

    public GameObject Nitro;
    public GameObject Shield;

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("NitroPU")) {
            Nitro.SetActive(true);
            Nitro.GetComponent<NitroPU>().Activate();
        }
        if (collider.gameObject.CompareTag("ShieldPU")) {
            Shield.SetActive(true);
            Shield.GetComponent<ShieldPU>().Activate();
        }
    }
}
