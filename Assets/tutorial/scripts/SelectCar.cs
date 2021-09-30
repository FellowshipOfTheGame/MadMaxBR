using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;

[Serializable]
public class Car
{
    public String name;
    public GameObject car;
    public GameObject carBody;
    public String carBodyName;
    public Texture[] materials;
    public Vector3 lookAtFixVector;
}

public class SelectCar : MonoBehaviour
{
    public GameObject lookAt;
    public Car[] cars;
    public string gameCene;
    public CinemachineFreeLook freeLookCamera;

    private int selectedId = 0;
    private int selectedIdColor = 0;
    private MeshRenderer m_Renderer;

    private void Awake()
    {
        selectedId = PlayerPrefs.GetInt("selectedId");
        selectedIdColor = PlayerPrefs.GetInt("selectedIdColor");
        ShowCar();
        SetCarColor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftCar();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightCar();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpColor();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DownColor();
        }

        freeLookCamera.m_XAxis.Value = 0.05f;
    }

    public void LoadGame()
    {
        PlayerPrefs.SetInt("selectedIdColor", selectedIdColor);//grava no salve
        PlayerPrefs.SetInt("SelecedCar", selectedId);//grava no salve
        SceneManager.LoadScene(gameCene, LoadSceneMode.Single);
    }

    private void UpColor()
    {
        if (selectedIdColor >= (cars[selectedId].materials.Length - 1))
        {
            selectedIdColor = 0;//zera o contador
        }
        else
        {
            selectedIdColor++;
        }
        PlayerPrefs.SetInt("selectedIdColor", selectedIdColor);//grava no salve
        SetCarColor();
    }

    private void DownColor()
    {

        if (selectedIdColor <= 0)
        {
            selectedIdColor = (cars[selectedId].materials.Length - 1);//coloca o valor maximo do vetor
        }
        else
        {
            selectedIdColor--;
        }
        PlayerPrefs.SetInt("selectedIdColor", selectedIdColor);//grava no salve
        SetCarColor();
    }

    private void SetCarColor()
    {
        m_Renderer = cars[selectedId].carBody.GetComponent<MeshRenderer>();
        for (int i = 0; i < m_Renderer.sharedMaterials.Length; i++)
        {
            Debug.Log(m_Renderer.sharedMaterials[i].name);
            if (m_Renderer.sharedMaterials[i].name == cars[selectedId].carBodyName)
            {
                m_Renderer.sharedMaterials[i].mainTexture = cars[selectedId].materials[selectedIdColor];
            }
        }
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

    private void RightCar()
    {
        if (selectedId >= (cars.Length - 1))
        {
            selectedId = 0;//zera o contador
        }
        else
        {
            selectedId++;
        }
        PlayerPrefs.SetInt("selectedId", selectedId);//grava no salve
        ShowCar();//carrega
    }

    private void ShowCar()
    {
        selectedIdColor = 0;
        lookAt.transform.position = new Vector3(cars[selectedId].car.transform.position.x + cars[selectedId].lookAtFixVector.x, cars[selectedId].car.transform.position.y + cars[selectedId].lookAtFixVector.y, cars[selectedId].car.transform.position.z + cars[selectedId].lookAtFixVector.z);
        SetCarColor();
    }
}
