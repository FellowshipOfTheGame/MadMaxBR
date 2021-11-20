using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerUpName {
    Nitro,
    Shield,
    Thorns,
    ExplosiveMine
}

public enum PowerUpType {
    Attack,
    Deffense,
    Trap,
    Utility
}
public enum UseType {
    Manual,
    Automatic
}
public enum DurationType {
    Permanent,
    Time,
    Resource
}

[CreateAssetMenu(fileName = "New PowerUp", menuName = "PowerUp")]
public class PowerUpData : ScriptableObject {
    public String Name;
    public String Description;
    public String Tag;
    public Image Icon; // sprite that represents powerup
    public int Id; // identification number that starts on 0
    public PowerUp PowerUpScript;
    public PowerUpType PowerUpType;
    public UseType UseType;
    public DurationType DurationType;

    public void Activate() {

    }
}
