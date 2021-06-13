using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;

[Serializable]
public class Car
{
    public String name;
    public GameObject car;
}

public class SelectCar : MonoBehaviour
{
    public GameObject lookAt;
    public Car[] cars;
    public string gameCene;
    public CinemachineFreeLook freeLookCamera;

    public int selectedId = 0;// tornar privado é apenas debug


    private void Awake()
    {
        selectedId = PlayerPrefs.GetInt("SelecedCar");
        ShowCar();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftCar();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RigthCar();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(gameCene, LoadSceneMode.Single);
        }
        freeLookCamera.m_XAxis.Value = 0.05f;
    }


    private void LeftCar()
    {
        if (selectedId <= 0)
        {
            selectedId = (cars.Length - 1);//coloca o valor maximo do vetor
        }
        else
        {
            selectedId--;
        }

        PlayerPrefs.SetInt("SelecedCar", selectedId);//grava no salve
        ShowCar();//carrega
    }
    private void RigthCar()
    {
        if (selectedId >= (cars.Length - 1))
        {
            selectedId = 0;//zera o contador
        }
        else
        {
            selectedId++;
        }
        PlayerPrefs.SetInt("SelecedCar", selectedId);//grava no salve
        ShowCar();//carrega
    }

    private void ShowCar()
    {
        lookAt.transform.position = new Vector3(cars[selectedId].car.transform.position.x, cars[selectedId].car.transform.position.y, cars[selectedId].car.transform.position.z);
    }
}
