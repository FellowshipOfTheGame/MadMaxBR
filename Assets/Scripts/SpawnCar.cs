using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SpawnCar : MonoBehaviour
{
    [SerializeField] private CarName carName;
    [SerializeField] private CarColor carColor;

    private void Awake()
    {
        carName = (CarName) PlayerPrefs.GetInt("selectedId");
        carColor = (CarColor) PlayerPrefs.GetInt("selectedIdColor");
        RaceManager.Instance.SpawnPlayer(null, carName, carColor, "Jurema");
    }
}
