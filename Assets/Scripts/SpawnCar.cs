using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SpawnCar : MonoBehaviour
{
    private void Awake()
    {
        RaceManager.Instance.NomeDoCarro = (CarName) PlayerPrefs.GetInt("selectedId");
        RaceManager.Instance.CorDoCarro = (CarColor) PlayerPrefs.GetInt("selectedIdColor");
    }
}
