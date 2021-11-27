using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPlatform : MonoBehaviour {
    /// <summary>
    /// List of materials to graphically represent powerups.
    /// </summary>
    public Material[] MaterialList; // 
    /// <summary>
    /// Time (in seconds) necessary to create another powerUp.
    /// </summary>
    public float CooldownTime;

    public bool isRandom;
    public int powerUpNum;
    public bool IsOnCooldown;

    private Timer platformTimer;
    private GameObject platformHitBox;
    private GameObject representation;

    // Start is called before the first frame update
    void Start() {
        platformHitBox = this.gameObject.transform.GetChild(1).gameObject;
        representation = this.gameObject.transform.GetChild(0).gameObject;
        IsOnCooldown = false;
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
    /// Makes platform visible.
    /// </summary>
    public void DeactivatePowerUpPlatform() {
        IsOnCooldown = true;
        platformTimer.ResetTimer();
        platformHitBox.SetActive(false);
        representation.SetActive(false);
    }

    /// <summary>
    /// Makes platform invisible.
    /// </summary>
    public void ActivatePowerUpPlatform() {
        int powerUpNumber;
        if (isRandom) {
            powerUpNumber = Random.Range(0, MaterialList.Length);
        } else {
            powerUpNumber = powerUpNum;
        }
        
        platformHitBox.transform.tag = MaterialList[powerUpNumber].name;
        Debug.Log(MaterialList[powerUpNumber].name);
        Debug.Log(platformHitBox.transform.tag);
        representation.GetComponent<MeshRenderer>().material = MaterialList[powerUpNumber];
        platformHitBox.SetActive(true);
        representation.SetActive(true);
    }
}
