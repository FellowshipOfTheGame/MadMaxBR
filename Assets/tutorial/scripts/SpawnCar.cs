using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SpawnCar : MonoBehaviour
{
    public GameObject modelos;
    public int selectedId;
    public int selectedIdColor;
    private MeshRenderer m_Renderer;

    public Car[] cars;

    private void Awake()
    {
        selectedId = PlayerPrefs.GetInt("selectedId");
        selectedIdColor = PlayerPrefs.GetInt("selectedIdColor");
        SetCarColor();
        Instantiate(cars[selectedId].car, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Destroy(modelos, 0.01f);
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
}
