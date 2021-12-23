using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatorMinePU : PowerUpBase {
    [SerializeField] private KeyCode useButton = KeyCode.X;
    [SerializeField] private int MinesQuantity; // quantity of mines to be used
    [SerializeField] private GameObject MinePrefab;
    [SerializeField] private GameObject DeactivatorMineHUD;

    private int remainingMines;

    public PowerUpData PowerUpInfo;

    public int RemainingMines { get { return remainingMines; } }
    
    public override void Activate() {
        remainingMines = MinesQuantity;
        if (DeactivatorMineHUD != null) {
            DeactivatorMineHUD.SetActive(true);
        }
    }

    public override void Deactivate() {
        this.gameObject.SetActive(false);
        if (DeactivatorMineHUD != null) {
            DeactivatorMineHUD.SetActive(false);
        }
        this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.DeactivatorMine);
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
