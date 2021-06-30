using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum PowerUpType {
    Attack,
    Deffense,
    Trap,
    Utility
}
public enum ActivationType {
    Manual,
    Automatic
}
public enum DurationType {
    Permanent,
    Time,
    Resource
}

[CreateAssetMenu(fileName = "New PowerUp", menuName = "PowerUp")]
public class Power : ScriptableObject {
    public String Name;
    public String Description;
    public String Tag;
    public Image Icon; // sprite that represents powerup
    public int Id; // identification number that starts on 0
    public PowerUpType PowerUpType;
    public ActivationType ActivationType;
    public DurationType DurationType;

    public void Activate() {

    }
}
