using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Alvo : MonoBehaviour
{
    public float velocidade;
    public float TempoVida = 5;  // Tempo que o avião vai ficar na Cena.
    public float currentTimeToLive = 0; //Cronometro.
    // Update is called once per frame
    void Update()
    {
        currentTimeToLive += Time.deltaTime;  //Cronometro vai Receber o TEMPO.
        if (currentTimeToLive > TempoVida) //Se Cronometro for Maior que Tempo do avião.
            Destroy(gameObject);    //avião Vai Ser Destruidao.
        /////////////////
        transform.Translate(velocidade * Time.deltaTime, 0, 0);
    }
}
