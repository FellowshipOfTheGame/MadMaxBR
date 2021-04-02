using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Rotacao : MonoBehaviour
{
    public float velocidade;
    void Update()
    {
        //rolacionar
        transform.Rotate(new Vector3(0, 0, velocidade));
    }
}
