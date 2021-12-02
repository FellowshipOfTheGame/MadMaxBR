using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarPU : MonoBehaviour {
    public int PillarsQuantity; // quantity of mines to be used
    public GameObject PillarPrefab;
    public GameObject PillarHUD;

    private int RemainingPillars;
    public void Activate() {
        RemainingPillars = PillarsQuantity;
        PillarHUD.SetActive(true);
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        PillarHUD.SetActive(false);
        this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.ExplosiveMine);
    }

    public int GetRemainingPillars() {
        return RemainingPillars;
    }

    public void Update() {
        if (RemainingPillars != 0) {
            if (Input.GetKeyDown(KeyCode.C)) {
                Instantiate(PillarPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
                RemainingPillars--;
            }
        }
        if (RemainingPillars == 0) { // if used all mines availabe
            Deactivate();
        }
    }
}
