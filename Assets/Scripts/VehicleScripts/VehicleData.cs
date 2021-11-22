using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script responsible to store information about the car such health, shield, powerUps, etc.
/// </summary>
public class VehicleData : MonoBehaviour {
    public string RunnerName;
    public float MaxCarHealth;
    public float MaxCarShield;

    private float curCarHealth;
    private float curCarShield;

    private int killsCount;

    public bool isDead;

    [HideInInspector]
    //public PowerUp[] powerUps;

    /// <summary>
    /// Attack Power Up Slot. 
    /// Can receive one of the following Attack Power Up codes:
    ///     0 -> machine gun,
    ///     1 -> rocket-launcher,
    ///     2 -> thorns,
    /// </summary>
    private int powerUpSlot1;
    /// <summary>
    /// Defense Power Up Slot. 
    /// Can receive one of the following Defense Power Up codes:
    ///     3 -> shield,
    ///     4 -> fixing,
    ///     5 -> smoke,
    /// </summary>
    private int powerUpSlot2;
    /// <summary>
    /// Can receive one of the following Trap Power Up codes:
    ///     6 -> explosive mine,
    ///     7 -> deactivator mine,
    /// </summary>
    private int powerUpSlot3;
    /// <summary>
    /// Can receive one of the following Utility Power Up codes:
    ///     8 -> nitro,
    ///     9 -> grease,
    ///     10 -> glue,
    /// </summary>
    private int powerUpSlot4;

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
    /// Returns the value of a a power up slot that stores the given powerup.
    /// </summary>
    /// <param name="powerup">PowerUp Name</param>
    /// <returns></returns>
    public int GetPowerUpSlotValue(PowerUpName powerup) {
        if (powerup == PowerUpName.Thorns) {
            return powerUpSlot1;
        }
        if (powerup == PowerUpName.Shield || powerup == PowerUpName.Fix) {
            return powerUpSlot2;
        }
        if (powerup == PowerUpName.ExplosiveMine) {
            return powerUpSlot3;
        }
        if (powerup == PowerUpName.Nitro) {
            return powerUpSlot4;
        }
        Debug.LogError("slotIndex of GetPowerUpSlotValue is wrong");
        return -1; // error 
    }
    /// <summary>
    /// Verify if the slot that allocates powerUp is free.
    /// </summary>
    /// <param name="powerUp">The powerUp that needs to be verified.</param>
    /// <returns></returns>
    public bool PowerUpSlotFree(PowerUpName powerup) {
        if (powerup == PowerUpName.Thorns) {
            if (powerUpSlot1 == -1) {
                return true;
            }
            return false;
        }
        if (powerup == PowerUpName.Shield || powerup == PowerUpName.Fix) {
            if (powerUpSlot2 == -1) {
                return true;
            }
            return false;
        }
        if (powerup == PowerUpName.ExplosiveMine) {
            if (powerUpSlot3 == -1) {
                return true;
            }
            return false;
        }
        if (powerup == PowerUpName.Nitro) {
            if (powerUpSlot4 == -1) {
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
        switch ((int)powerup) {
            case (int)PowerUpName.Thorns:
                powerUpSlot1 = (int)PowerUpName.Thorns;
                break;
            case (int)PowerUpName.Shield:
                powerUpSlot2 = (int)PowerUpName.Shield;
                break;
            case (int)PowerUpName.Fix:
                powerUpSlot2 = (int)PowerUpName.Fix;
                break;
            case (int)PowerUpName.ExplosiveMine:
                powerUpSlot3 = (int)PowerUpName.ExplosiveMine;
                break;
            case (int)PowerUpName.Nitro:
                powerUpSlot4 = (int)PowerUpName.Nitro;
                break;
        }
        /*
        if (powerup == PowerUpName.Thorns) {
            powerUpSlot1 = (int)PowerUpName.Thorns;
        }
        if (powerup == PowerUpName.Shield) {
            powerUpSlot2 = (int)PowerUpName.Shield;
        }
        if (powerup == PowerUpName.Fix) {
            powerUpSlot2 = (int)PowerUpName.Fix;
        }
        if (powerup == PowerUpName.ExplosiveMine) {
            powerUpSlot3 = (int)PowerUpName.ExplosiveMine;
        }
        if (powerup == PowerUpName.Nitro) {
            powerUpSlot4 = (int)PowerUpName.Nitro;
        }
        */
    }
    /// <summary>
    /// Logically removes a powerUp from its respective slot.
    /// </summary>
    /// <param name="powerUp">The PowerUp to be removed.</param>
    public void EmptyPowerUpSlot(PowerUpName powerup) {
        if (powerup == PowerUpName.Thorns) {
            powerUpSlot1 = -1;
        }
        if (powerup == PowerUpName.Shield || powerup == PowerUpName.Fix) {
            powerUpSlot2 = -1;
        }
        if (powerup == PowerUpName.ExplosiveMine) {
            powerUpSlot3 = -1;
        }
        if (powerup == PowerUpName.Nitro) {
            powerUpSlot4 = -1;
        }
    }

    public void SumKillsCount() {
        killsCount++;
    }

    public int GetKillsCount() {
        return killsCount;
    }

    public bool IsDead() {
        return isDead;
    }

    // Start is called before the first frame update
    public void Start() {
        curCarHealth = MaxCarHealth*10/100;
        curCarShield = 0;
        // setup number of kills
        killsCount = 0;
        isDead = false;
        //playerPowerUps = gameObject.transform.GetChild(8).gameObject;
        powerUpSlot1 = -1;
        powerUpSlot2 = -1;
        powerUpSlot3 = -1;
        powerUpSlot4 = -1;
    }
    // Update is called once per frame
    public void Update() {
        //SetCurrentHealth(MaxCarHealth);
        if (curCarHealth <= 0) {
            Die();
        }
        //if (playerPowerUps.transform.GetChild(0).gameObject.activeSelf) { // if nitro power up is active

        //}
    }

    public void SetCurrentHealth(float val) {
        curCarHealth = val;
    }

    public void AddHealth(float val) {
        curCarHealth += val;
        if (curCarHealth > MaxCarHealth) {
            curCarHealth = MaxCarHealth;
        }
    }

    public float GetCurrentHealth() {
        if (curCarHealth < 0) {
            return 0;
        }
        return curCarHealth;
    }

    public void SetCurrentShield(float val) {
        curCarShield = val;
    }

    public void AddShield(float val) {
        curCarShield += val;
        if (curCarShield > MaxCarShield) {
            curCarShield = MaxCarShield;
        }
    }

    public float GetCurrentShield() {
        return curCarShield;
    }

    public void ReceiveDamage(float damage) {
        if (curCarShield > 0) {
            if (damage < curCarShield) {
                curCarShield -= damage;
                damage = 0;
            } else {
                damage -= curCarShield;
                curCarShield = 0;
            }
        }
        curCarHealth -= damage;
    }

    public void Die() {
        this.gameObject.SetActive(false);
        isDead = true;
    }
}
