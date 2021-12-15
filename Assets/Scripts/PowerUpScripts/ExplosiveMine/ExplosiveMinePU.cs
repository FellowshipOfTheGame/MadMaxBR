using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMinePU : MonoBehaviour {
    [SerializeField] private KeyCode useButton = KeyCode.X;
    [SerializeField] private int MinesQuantity; // quantity of mines to be used
    [SerializeField] private GameObject MinePrefab;
    [SerializeField] private GameObject ExplosiveMineHUD;

    private int remainingMines;

    public PowerUpData PowerUpInfo;

    public int RemainingMines { get { return remainingMines; } }

    public void Activate() {
        remainingMines = MinesQuantity;
        ExplosiveMineHUD.SetActive(true);
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        ExplosiveMineHUD.SetActive(false);
        this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.ExplosiveMine);
    }

    public void Update() {
        if (remainingMines != 0) {
            if (Input.GetKeyDown(useButton)) {
                Instantiate(MinePrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
                remainingMines--;
            }
        } 
        if (remainingMines == 0) { // if used all mines availabe
            Deactivate();
        }
    }
}
