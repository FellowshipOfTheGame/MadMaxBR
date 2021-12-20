using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovement : MonoBehaviour {
    /// <summary>
    /// Time, in seconds, the pillar takes to go from minimum Y to maxim Y and vice-versa.
    /// </summary>
    public float MovementTimeInSeconds;
    /// <summary>
    /// Time the object have taken from the start of the movement to the current frame.
    /// </summary>
    private float movementTimer;
    /// <summary>
    /// Minimum Y value the pillar can have.
    /// </summary>
    private float minY = 1.827f;
    /// <summary>
    /// Maximum Y value the pillar can have.
    /// </summary>
    private float maxY = 2.764f;
    /// <summary>
    /// Controls whether the pillar is elevating or not.
    /// </summary>
    private bool isElevating;
    /// <summary>
    /// Controls whether the pillar is lowering or not.
    /// </summary>
    private bool isLowering;

    public void Start() {
        isElevating = false;
        isLowering = false;
    }

    public bool IsAtMaxHeight {
        get {
            if (this.gameObject.transform.position.y == maxY) {
                return true;
            } else {
                return false;
            }
        }
    }

    public bool IsAtMinHeight {
        get {
            if (this.gameObject.transform.position.y == minY) {
                return true;
            } else {
                return false;
            }
        }
    }
    private void MoveWeapon(float targetY) {
        float newY = Mathf.MoveTowards(this.gameObject.transform.position.y, targetY, Time.deltaTime / MovementTimeInSeconds);
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, newY, this.gameObject.transform.position.z);
    }
    /// <summary>
    /// Starts Weapon object movement upwards.
    /// </summary>
    public void StartMovementUp() {
        isElevating = true;
        movementTimer = 0;
    }
    /// <summary>
    /// Starts Weapon object movement downwards.
    /// </summary>
    public void StartMovementDown() {
        isLowering = true;
        movementTimer = 0;
    }
    public void Update() {
        movementTimer += Time.deltaTime;
        if (isElevating) {
            MoveWeapon(maxY);
            if (this.gameObject.transform.position.y == maxY) {
                isElevating = false;
                Debug.Log("seconds to elevate: " + movementTimer);
            }
        }
        if (isLowering) {
            MoveWeapon(minY);
            if (this.gameObject.transform.position.y == minY) {
                isLowering = false;
            }
        }
    }
}
