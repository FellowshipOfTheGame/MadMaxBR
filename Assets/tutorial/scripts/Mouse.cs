using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    [Range(10.0f,200.0f)]
    public float velocidadeDeRotacao = 50;
    public float rotYTemp;
    public float rotXTemp;

    void Update()
    {
        rotYTemp = Input.GetAxis ("Mouse X") * velocidadeDeRotacao * 5;
        // rotXTemp = Input.GetAxis ("Mouse Y") * velocidadeDeRotacao;
        // transform.Rotate(new Vector3((rotXTemp*-1) * Time.deltaTime, rotYTemp * Time.deltaTime, 0));
        transform.Rotate(new Vector3(0, rotYTemp * Time.deltaTime, 0));
    }
}
