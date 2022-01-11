using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMinePU : PowerUpBase {
    [SerializeField] private int MinesQuantity; // quantity of mines to be used
    [SerializeField] private GameObject MinePrefab;
    
    private GameObject ExplosiveMineHUD;
    private int remainingMines;

    public PowerUpData PowerUpInfo;

    public int RemainingMines { get { return remainingMines; } }
    private void Awake() {
        if (this.transform.parent.gameObject.transform.parent.gameObject.CompareTag("Player")) {
            ExplosiveMineHUD = PlayerDataDisplayer.Instance.ExplosiveMineCount;
        } else {
            ExplosiveMineHUD = null;
        }
    }
    public override void Activate() {
        remainingMines = MinesQuantity;
        if (ExplosiveMineHUD != null) {
            ExplosiveMineHUD.SetActive(true);
        }
    }

    public override void Deactivate() {
        this.gameObject.SetActive(false);
        if (ExplosiveMineHUD != null) {
            ExplosiveMineHUD.SetActive(false);
        }
        this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.ExplosiveMine);
    }

    public override void UsePowerUp(bool useActive) {
        if (useActive) {
            if (remainingMines != 0) {
                GameObject mine = Instantiate(MinePrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
                mine.GetComponentInChildren<ExplosiveMine>().Owner = this.gameObject;
                remainingMines--;
            }
        }
    }

    public override void Update() { 
        if (remainingMines == 0) { // if used all mines availabe
            Deactivate();
        }
    }
}
