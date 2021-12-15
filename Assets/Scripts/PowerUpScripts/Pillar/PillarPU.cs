using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarPU : MonoBehaviour {
    [SerializeField] private KeyCode useButton = KeyCode.X;
    [SerializeField] private int PillarsQuantity; // quantity of pillars to be used
    [SerializeField] private GameObject PillarPrefab;
    [SerializeField] private GameObject PillarHUD;

    private int remainingPillars;

    public PowerUpData PowerUpInfo;

    public int RemainingPillars { get { return remainingPillars; } }

    public void Activate() {
        remainingPillars = PillarsQuantity;
        PillarHUD.SetActive(true);
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        PillarHUD.SetActive(false);
        this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Pillar);
    }

    public void Update() {
        if (remainingPillars != 0) {
            if (Input.GetKeyDown(useButton)) {
                Instantiate(PillarPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
                remainingPillars--;
            }
        }
        if (remainingPillars == 0) { // if used all mines availabe
            Deactivate();
        }
    }
}
