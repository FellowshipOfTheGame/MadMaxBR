using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    public int SelecedCar;
    public GameObject[] cars;

    private void Awake()
    {
        SelecedCar = PlayerPrefs.GetInt("SelecedCar");
        Instantiate(cars[SelecedCar], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    }
}
