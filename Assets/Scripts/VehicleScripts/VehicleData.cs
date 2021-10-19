using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script responsible to store information about the car such health, shield, powerUps, etc.
/// </summary>
public class VehicleData : MonoBehaviour {
    public float MaxCarHealth;
    public float MaxCarShield;

    [HideInInspector]
    //public PowerUp[] powerUps;

    /// <summary>
    /// Can receive one of the following Attack Power Up codes:
    ///     0 -> machine gun,
    ///     1 -> rocket-launcher,
    ///     2 -> thorns,
    /// </summary>
    private int powerUpSlot1;
    /// <summary>
    /// Can receive one of the following Defense Power Up codes:
    ///     3 -> smoke,
    /// </summary>
    private int powerUpSlot2;
    /// <summary>
    /// Can receive one of the following Trap Power Up codes:
    ///     4 -> explosive mine,
    ///     5 -> deactivator mine,
    /// </summary>
    private int powerUpSlot3;
    /// <summary>
    /// Can receive one of the following Utility Power Up codes:
    ///     6 -> nitro,
    ///     7 -> grease,
    ///     8 -> glue,
    /// </summary>
    private int powerUpSlot4;

    private float CurCarHealth;
    private float CurCarShield;

    private GameObject playerPowerUps; // stores the object containing the powerUp objects of the car

    /// <summary>
    /// Returns the slot value of a given slotIndex.
    /// </summary>
    /// <param name="slotIndex">The index of the PowerUp Slot desired.</param>
    /// <returns></returns>
    public int GetPowerUpSlotValue(int slotIndex) {
        if (slotIndex == 1) {
            return powerUpSlot1;
        } else if (slotIndex == 2) {
            return powerUpSlot2;
        } else if (slotIndex == 3) {
            return powerUpSlot3;
        } else if (slotIndex == 4) {
            return powerUpSlot4;
        } else {
            Debug.LogError("slotIndex of GetPowerUpSlotValue is wrong");
            return -2; // error
        }
    }
    /// <summary>
    /// Verify if the slot that allocates powerUp is free.
    /// </summary>
    /// <param name="powerUp">The powerUp that needs to be verified.</param>
    /// <returns></returns>
    public bool PowerUpSlotFree(PowerUpName powerup) {
        if (powerup == PowerUpName.Nitro) {
            if (powerUpSlot4 == -1) {
                return true;
            }
            return false;
        }
        if (powerup == PowerUpName.Thorns) {
            if (powerUpSlot1 == -1) {
                return true;
            }
            return false;
        }
        Debug.LogError("slotIndex of GetPowerUpSlotValue is wrong");
        return false; // error
    }
    /// <summary>
    /// Logically stores a received powerUp in its respective slot.
    /// </summary>
    /// <param name="powerUp">The PowerUp to be inserted.</param>
    public void FillPowerUpSlot(PowerUpName powerup) {
        if (powerup == PowerUpName.Nitro) {
            powerUpSlot4 = 6;
        }
        if (powerup == PowerUpName.Thorns) {
            powerUpSlot1 = 6;
        }
    }
    /// <summary>
    /// Logically removes a powerUp from its respective slot.
    /// </summary>
    /// <param name="powerUp">The PowerUp to be removed.</param>
    public void EmptyPowerUpSlot(PowerUpName powerup) {
        if (powerup == PowerUpName.Nitro) {
            powerUpSlot4 = -1;
        }
        if (powerup == PowerUpName.Thorns) {
            powerUpSlot1 = -1;
        }
    }
    // Start is called before the first frame update
    void Start() {
        CurCarHealth = MaxCarHealth;
        CurCarShield = 0;
        //playerPowerUps = gameObject.transform.GetChild(8).gameObject;
        powerUpSlot1 = -1;
        powerUpSlot2 = -1;
        powerUpSlot3 = -1;
        powerUpSlot4 = -1;
    }
    // Update is called once per frame
    void Update() {
        //SetCurrentHealth(MaxCarHealth);
        if (CurCarHealth <= 0) {
            Destroy(this.gameObject);
        }
        //if (playerPowerUps.transform.GetChild(0).gameObject.activeSelf) { // if nitro power up is active

        //}
        ReceiveDamage(1);
        Debug.Log(GetCurrentHealth());
    }

    public void SetCurrentHealth(float val) {
        CurCarHealth = val;
    }

    public void AddHealth(float val) {
        CurCarHealth += val;
        if (CurCarHealth > MaxCarHealth) {
            CurCarHealth = MaxCarHealth;
        }
    }

    public float GetCurrentHealth() {
        if (CurCarHealth < 0) {
            return 0;
        }
        return CurCarHealth;
    }

    public void SetCurrentShield(float val) {
        CurCarShield = val;
    }

    public float GetCurrentShield() {
        return CurCarShield;
    }

    public void ReceiveDamage(float damage) {
        if (CurCarShield > 0) {
            if (damage < CurCarShield) {
                CurCarShield -= damage;
                damage = 0;
            } else {
                damage -= CurCarShield;
                CurCarShield = 0;
            }
        }
        CurCarHealth -= damage;
    }
}
