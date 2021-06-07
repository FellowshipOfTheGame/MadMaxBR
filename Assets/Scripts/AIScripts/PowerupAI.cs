using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupAI : MonoBehaviour
{
    [SerializeField] private GameObject nitro;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject fix;
    [SerializeField] private GameObject thorns;

    private VehicleData vehicleData;
    private NitroPU nitroPowerup;
    private ShieldPU shieldPowerup;
    private FixPU fixPowerup;
    private ThornsPU thornsPowerup;

    private bool hasNitro = false;
    private bool hasShield = false;
    private bool hasFix = false;
    private bool hasThorns = false;

    private void Start()
    {
        vehicleData = GetComponent<VehicleData>();
        nitroPowerup = nitro.GetComponent<NitroPU>();
        shieldPowerup = shield.GetComponent<ShieldPU>();
        fixPowerup = fix.GetComponent<FixPU>();
        thornsPowerup = thorns.GetComponent<ThornsPU>();
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("NitroPU"))
        {
            if (vehicleData.PowerUpSlotFree(PowerUpName.Nitro))
            {
                vehicleData.FillPowerUpSlot(PowerUpName.Nitro);
                nitro.SetActive(true);
                nitroPowerup.Activate();
                collider.transform.parent.gameObject.SetActive(false);
                collider.transform.parent.gameObject.SetActive(false);
            }
        }
        if (collider.gameObject.CompareTag("ShieldPU"))
        {
            shield.SetActive(true);
            shieldPowerup.Activate();
            collider.transform.parent.gameObject.SetActive(false);
        }
        if (collider.gameObject.CompareTag("FixPU"))
        {
            fix.SetActive(true);
            fixPowerup.Activate();
            fix.SetActive(false);
            collider.transform.parent.gameObject.SetActive(false);
        }
        if (collider.gameObject.CompareTag("ThornsPU"))
        {
            if (vehicleData.PowerUpSlotFree(PowerUpName.Thorns))
            {
                vehicleData.FillPowerUpSlot(PowerUpName.Thorns);
                thorns.SetActive(true);
                thornsPowerup.Activate();
                collider.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
