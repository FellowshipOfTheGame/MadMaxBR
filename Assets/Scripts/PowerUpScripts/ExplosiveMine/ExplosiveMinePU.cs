using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMinePU : PowerUpBase {
    [SerializeField] private KeyCode useButton = KeyCode.X;
    [SerializeField] private int MinesQuantity; // quantity of mines to be used
    [SerializeField] private GameObject MinePrefab;
    [SerializeField] private GameObject ExplosiveMineHUD;

    private int remainingMines;

    public PowerUpData PowerUpInfo;

    public int RemainingMines { get { return remainingMines; } }

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
                Instantiate(MinePrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
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
