using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleData : MonoBehaviour {
    public float MaxCarHealth;
    public float MaxCarShield;

    private bool[] PowerUpsActive;

    private float CurCarHealth;
    private float CurCarShield;

    // Start is called before the first frame update
    void Start() {
        CurCarHealth = MaxCarHealth;
        CurCarShield = 0;
    }
    
    // Update is called once per frame
    void Update() {
        
    }

    public void SetCurrentHealth(float val) {
        CurCarShield = val;
    }

    public float GetCurrentHealth() {
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
