using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarPU : PowerUpBase {
    [SerializeField] private int PillarsQuantity; // quantity of pillars to be used
    [SerializeField] private GameObject PillarPrefab;
    
    private GameObject PillarHUD;
    private int remainingPillars;

    public PowerUpData PowerUpInfo;

    public int RemainingPillars { get { return remainingPillars; } }
    private void Awake() {
        if (this.transform.parent.gameObject.transform.parent.gameObject.CompareTag("Player")) {
            PillarHUD = PlayerDataDisplayer.Instance.PillarCount;
        } else {
            PillarHUD = null;
        }
    }
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
                GameObject pillar = Instantiate(PillarPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
                pillar.GetComponent<PillarCollision>().Owner = this.gameObject;
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
