    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AirCraft : MonoBehaviour {

    //Velocidade
     public float Speed;//Velocidade
     public float MaxSpeed;//Velocidade Máxima
     public float Acceleration;//Aceleração

    //Sustentação
     public float Lift;//Sustentação
     Vector3 LiftDirection;//Direção da força de sustentação
     public float PowerLift;//Sustentação Aplicada

    //Arrasto
     float ArrastoExtra;//Arrasto Adicional
     float ArrastoOriginal;//Arrasto Inicial
     public float Arrasto;//Arrasto Aplicado
     public float AumentoArrasto = 0.001f;//Aumento de Arrasto

     Rigidbody rb;

    //Velocidade de Rotação
     public float SpeedRotate;

    //Teclas
     bool Space;
     bool Shift;

    //Freio
     public bool Break;
     public float BreakEffect;//Efeito do Freio

     // Use this for initialization
     void Start () {
     rb = gameObject.GetComponent<Rigidbody> ();//Pegar Rigidbody

     ArrastoOriginal = rb.drag;//Pegar Arrasto Inicial

     }

     // Update is called once per frame
     void Update () {


     CalcularArrasto ();
     LiftPower ();


     if (Input.GetKeyDown (KeyCode.Space)) {
     Space = true;
     }
     if (Input.GetKeyUp (KeyCode.Space)) {
     Space = false;
     }


     if (Space == true) {

     if (Speed < MaxSpeed) {
     Speed += Acceleration;//Aumentar Velocidade
     }



     } else {

     if (Speed > 1) {
     Speed -= Acceleration;//Diminuir Velocidade
     }


     }





     if (Input.GetKeyDown(KeyCode.W)) {
     transform.Rotate (-SpeedRotate, 0, 0);//Girar para Cima
     }
     if (Input.GetKeyDown(KeyCode.S)) {
     transform.Rotate (SpeedRotate, 0, 0);//Girar para Baixo
     }

     if (Input.GetKeyDown(KeyCode.A)) {
     transform.Rotate (0, 0, SpeedRotate);//Girar para Esquerda
     }
     if (Input.GetKeyDown(KeyCode.D)) {
     transform.Rotate (0, 0, -SpeedRotate);//Girar para Direita
     }


     if(Input.GetKeyDown(KeyCode.LeftShift)){
     Shift = true;
     }
     if(Input.GetKeyUp(KeyCode.LeftShift)){
     Shift = false;
     }

     if (Shift == true) {
     Break = true;
     } else {
     Break = false;
     }

     rb.AddRelativeForce (Vector3.forward * Speed);//Adicionar Velocidade

     




     }



     private void CalcularArrasto(){ //Calcular Arrasto  (Essa não é a equação original)

     ArrastoExtra = rb.velocity.magnitude * AumentoArrasto; //Calcular o arrastoExtra

     if(Break == true){ //Se "Break" for igual a true
     Arrasto = (ArrastoOriginal + ArrastoExtra) * BreakEffect; //Calcular o arrasto com o efeito do freio("BreakEffect")
     }else{ //Se "Break" for igual a false
     Arrasto = ArrastoOriginal + ArrastoExtra; //Arrasto é igual a "ArrastoOriginal" + "ArrastoExtra"
     }

     rb.drag = Arrasto;//Adicionar Arrasto

     }


     private void LiftPower (){ //Calcular Sustentação

     LiftDirection = Vector3.Cross (rb.velocity,transform.right).normalized;//Achar vetor de Sustentação 

     PowerLift = (Lift * (Speed * Speed)) / 2.4f;//Calcular Força de sustentação (Essa não é a equação original)

     rb.AddForce (LiftDirection * PowerLift);//Adicionar Força de sustentação

     }



    }