using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMinePU : MonoBehaviour {
    public int MinesQuantity; // quantity of mines to be used
    public GameObject MinePrefab;
    //public GameObject MineSpawner;

    private int RemainingMines;
    public void Activate() {
        RemainingMines = MinesQuantity;
    }

    public void Update() {
        if (RemainingMines != 0) {
            if (Input.GetKeyDown(KeyCode.Z)) {
                Debug.Log("USED A MINE.");
                Instantiate(MinePrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
                RemainingMines--;
            }
        } 
        if (RemainingMines == 0) { // if used all mines availabe
            this.gameObject.SetActive(false);
            Debug.Log("USED ALL MINES.");
            this.gameObject.transform.parent.GetComponentInParent<VehicleData>().EmptyPowerUpSlot(PowerUpName.ExplosiveMine);
        }
    }
}
