using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatorMinePU : MonoBehaviour {
    public int MinesQuantity; // quantity of mines to be used
    public GameObject MinePrefab;
    public GameObject DeactivatorMineHUD;

    private int RemainingMines;
    public void Activate() {
        RemainingMines = MinesQuantity;
        DeactivatorMineHUD.SetActive(true);
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        DeactivatorMineHUD.SetActive(false);
        this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.DeactivatorMine);
    }

    public int GetRemainingMines() {
        return RemainingMines;
    }

    public void Update() {
        if (RemainingMines != 0) {
            if (Input.GetKeyDown(KeyCode.C)) {
                Instantiate(MinePrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
                RemainingMines--;
            }
        } 
        if (RemainingMines == 0) { // if used all mines availabe
            Deactivate();
        }
    }
}
