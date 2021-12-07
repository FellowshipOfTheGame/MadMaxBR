using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarMovement : MonoBehaviour {
    /// <summary>
    /// Time, in seconds, the pillar waits to start.
    /// </summary>
    public float TimeToElevateInSeconds;
    /// <summary>
    /// Time, in seconds, the pillar stays on maximum height.
    /// </summary>
    public float TimeToLowerInSeconds;
    /// <summary>
    /// Time, in seconds, the pillar takes to go from minimum Y to maxim Y and vice-versa.
    /// </summary>
    public float MovementTimeInSeconds;
    /// <summary>
    /// Minimum Y value the pillar can have.
    /// </summary>
    private float minY;
    /// <summary>
    /// Maximum Y value the pillar can have.
    /// </summary>
    private float maxY;
    /// <summary>
    /// Controls whether the pillar is elevating or not.
    /// </summary>
    private bool isElevating;
    /// <summary>
    /// Controls whether the pillar is lowering or not.
    /// </summary>
    private bool isLowering;

    private Timer stopwatch;

    private void MovePillar(float targetY) {
        float newY = Mathf.MoveTowards(this.gameObject.transform.position.y, targetY, Time.deltaTime * MovementTimeInSeconds);
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, newY, this.gameObject.transform.position.z);
    }

    void Awake() {
        stopwatch = gameObject.AddComponent<Timer>();
    }

    public void Start() {
        minY = this.gameObject.transform.position.y - 4.2f;
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, minY, this.gameObject.transform.position.z);
        maxY = this.gameObject.transform.position.y + 5.1f;
        isElevating = false;
        isLowering = false;
        stopwatch.ResetTimer();
    }

    public void Update() {
        if (this.gameObject.transform.position.y == minY && !isElevating && !isLowering) { // if pillar is not moving at min Y
            if (stopwatch.GetSeconds() + stopwatch.GetMinutes() * 60 >= TimeToElevateInSeconds) {
                isElevating = true;
                stopwatch.ResetTimer();
            }
        } else if (this.gameObject.transform.position.y != maxY && isElevating) { // if pillar is elevating
            MovePillar(maxY);
        } else if (this.gameObject.transform.position.y == maxY && isElevating) { // if pillar reached maximum Y
            isElevating = false;
            stopwatch.ResetTimer();
        } else if (this.gameObject.transform.position.y == maxY && !isElevating && !isLowering) { // if pillar is not moving at max Y
            if (stopwatch.GetSeconds() + stopwatch.GetMinutes() * 60 >= TimeToLowerInSeconds) {
                isLowering = true;
                stopwatch.ResetTimer();
            }
        } else if (isLowering && this.gameObject.transform.position.y != minY) { // if pillar is lowering
            MovePillar(minY);
        } else if (isLowering && this.gameObject.transform.position.y == minY) { // if pillar reached min Y
            Destroy(this.gameObject);
        }
    }
}
