using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarPU : PowerUpBase {
    [SerializeField] private KeyCode useButton = KeyCode.X;
    [SerializeField] private int PillarsQuantity; // quantity of pillars to be used
    [SerializeField] private GameObject PillarPrefab;
    [SerializeField] private GameObject PillarHUD;

    private int remainingPillars;

    public PowerUpData PowerUpInfo;

    public int RemainingPillars { get { return remainingPillars; } }

    public override void Activate() {
        remainingPillars = PillarsQuantity;
        if (PillarHUD != null) {
            PillarHUD.SetActive(true);
        }
    }

    public override void Deactivate() {
        this.gameObject.SetActive(false);
        if (PillarHUD != null) {
            PillarHUD.SetActive(false);
        }
        this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Pillar);
    }

    public override void UsePowerUp(bool useActive) {
        if (useActive) {
            if (remainingPillars != 0) {
                Instantiate(PillarPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
                remainingPillars--;
            }
        }
    }

    public override void Update() {
        if (remainingPillars == 0) { // if used all mines availabe
            Deactivate();
        }
    }
}
