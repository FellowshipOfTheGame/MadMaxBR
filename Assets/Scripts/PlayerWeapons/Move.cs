using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float balaVelocidade;// Velocidade da Bala.
    public float TempoVida = 2;  // Tempo que A Bala vai ficar na Cena.
    private float currentTimeToLive = 0; //Cronometro.
    // Update is called once per frame
    void Update()
    {

        currentTimeToLive += Time.deltaTime;  //Cronometro vai Receber o TEMPO.

        if (currentTimeToLive > TempoVida) //Se Cronometro for Maior que Tempo da Bala.
            Destroy(gameObject);    //Bala Vai Ser Destruida.


        transform.Translate(Vector3.forward * balaVelocidade); //A BALA VAI ANDAR PARA FRENTE
        //transform.Translate(0, 0, balaVelocidade * Time.deltaTime);
    }
}
