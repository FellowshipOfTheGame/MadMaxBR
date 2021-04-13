using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseController : MonoBehaviour
{
    public int modelo;
    public GameObject plane0;
    public GameObject plane1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        modelo = PlayerPrefs.GetInt("modelo");
        if (modelo == 0)
        {
            Instantiate(plane0, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else if (modelo == 1)
        {
            Instantiate(plane1, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
