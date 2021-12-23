using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBase : MonoBehaviour {
    /// <summary>
    /// Called when a car passes through a Power-up platform.
    /// </summary>
    public virtual void Activate() {

    }
    /// <summary>
    /// Called when the Power-up of a car ends.
    /// </summary>
    public virtual void Deactivate() {

    }
    /// <summary>
    /// Called when the player or the AI uses a Power-up with manual activation.
    /// </summary>
    /// <param name="useActive"></param>
    public virtual void UsePowerUp(bool useActive) {
        
    }
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    public virtual void Start() {
        
    }
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    public virtual void Update() {
        
    }
}
