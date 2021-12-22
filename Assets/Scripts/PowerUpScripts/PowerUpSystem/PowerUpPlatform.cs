using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class responsible to implement the logic of Power Up Plataforms.
/// </summary>
public class PowerUpPlatform : MonoBehaviour {
    /// <summary>
    /// List of objects to graphically represent powerups.
    /// </summary>
    public PowerUpBarrelModels BarrelModels;
    /// <summary>
    /// Time (in seconds) necessary to generate another powerUp.
    /// </summary>
    [SerializeField] private float CooldownTime;

    [SerializeField] private bool isRandom;
    [SerializeField] private int powerUpNum;
    [SerializeField] private bool IsOnCooldown = true;

    private Timer platformTimer;
    private GameObject platformHitBox;
    private GameObject instantiatedBarrel;

    // Start is called before the first frame update
    void Start() {
        //platformHitBox = this.gameObject.transform.GetChild(0).gameObject;
        platformHitBox = GetComponentInChildren<CapsuleCollider>().gameObject;
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        platformTimer = this.gameObject.AddComponent<Timer>();
        ActivatePowerUpPlatform();
    }

    // Update is called once per frame
    void Update() {
        if (platformTimer.GetTimeInSeconds() >= CooldownTime) {
            IsOnCooldown = false;
        }
        if (!platformHitBox.activeSelf && !IsOnCooldown) {
            ActivatePowerUpPlatform();
        }
    }

    /// <summary>
    /// Makes platform invisible and unreachable.
    /// </summary>
    public void DeactivatePowerUpPlatform() {
        IsOnCooldown = true;
        platformTimer.ResetTimer();
        platformHitBox.SetActive(false);
        Destroy(instantiatedBarrel);
    }

    /// <summary>
    /// Makes platform visible and activable with a random powerUp.
    /// </summary>
    public void ActivatePowerUpPlatform() {
        int powerUpNumber;
        if (isRandom) {
            powerUpNumber = Random.Range(0, BarrelModels.Barrels.Count);
        } else {
            powerUpNumber = powerUpNum;
        }

        platformHitBox.transform.tag = BarrelModels.Barrels[powerUpNumber].name;
        platformHitBox.SetActive(true);
        instantiatedBarrel = Instantiate(BarrelModels.Barrels[powerUpNumber], platformHitBox.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
    }
}
