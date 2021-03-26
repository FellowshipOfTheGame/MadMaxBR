using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpColor : MonoBehaviour {

    //[SerializeField] public int Color[5] = {"red",}; 
    public GameObject PowerUp;
    public GameObject PowerUpTrigger;
    public GameObject Car;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter() {
        PowerUp.SetActive(false);
        var clr = Car.GetComponent<Renderer>();
        clr.material.SetColor("_Color", Color.red);
    }
}
