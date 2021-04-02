using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriaBala : MonoBehaviour
{
    public GameObject municao;
    public GameObject cano;
    public void Tiro()
    {
       
       Instantiate(municao, new Vector3(cano.transform.position.x, cano.transform.position.y, cano.transform.position.z), Quaternion.identity);
     }
}
