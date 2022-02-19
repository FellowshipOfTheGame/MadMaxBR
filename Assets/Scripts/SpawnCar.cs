using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SpawnCar : MonoBehaviour
{
    [SerializeField] public int selectedId;
    [SerializeField] public int selectedIdColor;
    public GameObject playerSpawnPoint;
    private MeshRenderer m_Renderer;
    private RaceManager raceManager;
    public Car[] cars;

    private void Awake()
    {
        selectedId = PlayerPrefs.GetInt("selectedId");
        selectedIdColor = PlayerPrefs.GetInt("selectedIdColor");
        cars[selectedId].car.SetActive(true);
        // GameObject player  = Instantiate(cars[selectedId].car, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        SetCarColor();
        Debug.Log("Passou");
        cars[selectedId].car.transform.position = new Vector3(playerSpawnPoint.transform.position.x, playerSpawnPoint.transform.position.y, playerSpawnPoint.transform.position.z);
        RaceManager.Instance.Player = cars[selectedId].car;
        RaceManager.Instance.Racers[RaceManager.Instance.Racers.Count - 1] = cars[selectedId].car;
        for (int i = 0; i < cars.Length; i++)
        {
            //if (i != selectedId)
            //{
               // GameObject.Destroy
                Debug.Log(cars[i].car.name);
            //}
        }
    }

    private void SetCarColor()
    {
        m_Renderer = cars[selectedId].carBody.GetComponent<MeshRenderer>();
        for (int i = 0; i < m_Renderer.sharedMaterials.Length; i++)
        {
            //           Debug.Log(m_Renderer.sharedMaterials[i].name);
            //          if (m_Renderer.sharedMaterials[i].name == cars[selectedId].carBodyName)
            //           {
            m_Renderer.sharedMaterials[i].mainTexture = cars[selectedId].materials[selectedIdColor];
            //        }
        }
    }
}
