using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible to activate a PowerUp object when a car this script is attached collides with a PowerUp Trigger.
/// </summary>
public class PowerUpActivation : MonoBehaviour {

    public GameObject Nitro;
    public GameObject Shield;
    public GameObject Fix;
    public GameObject Thorns;

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("NitroPU")) { // Nitro is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.Nitro)) { // if the nitro slot is free
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.Nitro);
                Nitro.SetActive(true);
                Nitro.GetComponent<NitroPU>().Activate();
                collider.transform.parent.gameObject.SetActive(false);
                collider.transform.parent.gameObject.SetActive(false); // deactivate the PowerUp platform
            }
        }
        if (collider.gameObject.CompareTag("ShieldPU")) { // Shield is not stored in a slot
            Shield.SetActive(true);
            Shield.GetComponent<ShieldPU>().Activate();
            collider.transform.parent.gameObject.SetActive(false); // deactivate the PowerUp platform
        }
        if (collider.gameObject.CompareTag("FixPU")) { // Fix is not stored in a slot
            Fix.SetActive(true);
            Fix.GetComponent<FixPU>().Activate();
            Fix.SetActive(false);
            collider.transform.parent.gameObject.SetActive(false); // deactivate the PowerUp platform
        }
        if (collider.gameObject.CompareTag("ThornsPU")) { // Thorns is stored in a slot
            if (gameObject.GetComponent<VehicleData>().PowerUpSlotFree(PowerUpName.Thorns)) { // if the nitro slot is free
                gameObject.GetComponent<VehicleData>().FillPowerUpSlot(PowerUpName.Thorns);
                Thorns.SetActive(true);
                Thorns.GetComponent<ThornsPU>().Activate();
                collider.transform.parent.gameObject.SetActive(false); // deactivate the PowerUp platform
            }
        }
    }
}
