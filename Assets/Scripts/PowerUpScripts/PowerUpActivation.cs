using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpActivation : MonoBehaviour {

    public GameObject Nitro;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("NitroPU")) {
            Nitro.SetActive(true);
            Nitro.GetComponent<NitroPU>().Activate();
        }
        if (collider.gameObject.CompareTag("SmokePU")) {

        }
    }
}
