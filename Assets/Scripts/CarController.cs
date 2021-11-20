using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ControlMode
{
    Player, AI
}

public enum Axel//saber qual eixo esta
{
    Front, Rear// define qual roda é da frente qual é de tras
}

public enum Side//para saber qual lado esta a roda
{
    Left, Right
}

public enum DriverType//dizer qual vai ser a tração do carro, se é 4x4 etc
{
    Front, Rear, full
}

[Serializable]//aparecer no inspector
public struct Wheel// estrutura para saber qual roda é
{
    public GameObject model;//modelo 3d da rodas
    public WheelCollider collider;//coliders da roda
    public Axel axel; //eixo da roda
    public Side side;//lado da roda
}

public class CarController : MonoBehaviour
{
    public ControlMode controlMode;
    private CarMovementAI carMovementAI;
    public Wheel[] wheels;// rodas, o array[] vc define qts rodas são no inspector
    public DriverType driverType;//variavel controla a tração
    private float torque;//variavel controla torque
    public float Maxtorque;//força de aceleração
    public float breaktorque;//freios
    public float maxSteelAngle = 30f;//angulo que as rodas da frente giram
    public float TurnSensitivity = 1f;// sensibilidade do volante

    private Rigidbody rbCar;
    private Vector2 input;
    private bool isBreak;//freio

    public Transform centerofMass;//variavel do centro da massa do carro
    public float downForce;//força que o vento exerce sobre o carro
    public float maxSpeed;//velocidade máxima do carro
    public float speed;//velocidade que o carro está

    public float SteerRadius;//abertura da roda, quanto maior menos vai ser a abertura da curva

    public Text speedTxt;//texto velocidade

    // Start is called before the first frame update
    void Start()
    {
        rbCar = GetComponent<Rigidbody>();//ajuda ajustar o centro de gravidade
        rbCar.centerOfMass = centerofMass.localPosition;//definindo centro de massa 
        carMovementAI = GetComponent<CarMovementAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controlMode == ControlMode.Player)
        {
            GetInput(); //chamar o metodo Input
        }
        else if (controlMode == ControlMode.AI)
        {
            InputAI();
        }
    }

    private void FixedUpdate()
    {
        SetTorque();
        SetBreak();
        Turn();
        AnimateWheels();
    }

    #region MEUS MÉTODOS

    private void GetInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))//ao apertar espaço freia
        {
            isBreak = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))//ao apertar a tecla espaço
        {
            isBreak = false;
        }
    }

    private void InputAI()
    {
        if (carMovementAI)
        {
            input.x = carMovementAI.steer;
            input.y = carMovementAI.throttle;
            isBreak = carMovementAI.brake;
        }
    }


    private void SetTorque()
    {

        speed = rbCar.velocity.magnitude * 3.6f;

        if (speedTxt != null) {
            speedTxt.text = speed.ToString("N0");//marcar a velocidade no texto
        }
        

        if (driverType == DriverType.full) { torque = Maxtorque / 4; } else { torque = Maxtorque / 2; }

        if (speed >= maxSpeed) { torque = 0; }//se a velocidade estiver igual ou maior que a velocidade maxima,cancela o torque
        rbCar.AddForce(Vector3.down * downForce * speed);//adicionar a força do vento de baixo para cima para ganho de estabilidade em velocidades altas

        foreach (Wheel w in wheels)//para cada roda
        {
            switch (driverType)//verifica se é a roda da frente ou tras e aplica tração
            {
                case DriverType.full:
                    w.collider.motorTorque = input.y * torque;//se for traction full aplica tração nas 4 rodas
                    break;

                case DriverType.Front:
                    if (w.axel == Axel.Front)// se a tração for na frente
                    {
                        w.collider.motorTorque = input.y * torque;//se a tração for na frente aplique tração nas rodas da frente
                    }
                    break;

                case DriverType.Rear:
                    if (w.axel == Axel.Rear)// se a tração for atrás
                    {
                        w.collider.motorTorque = input.y * torque;//se a tração for atrás aplique tração nas rodas da traseiras
                    }
                    break;

            }
        }
    }

    private void SetBreak()//método para freiar
    {
        foreach (Wheel w in wheels)
        {
            if (isBreak)
            {
                w.collider.motorTorque = 0;
                w.collider.brakeTorque = breaktorque;
            }
            else
            {
                w.collider.brakeTorque = 0;
            }
        }
    }

    private void Turn()//metodo para fazer a curva
    {
        float r = SteerRadius;

        //conforme a velocidade o steerRadius deve aumentar para que o controle de curva faça curvas mais suaves
        if (speed >= 100) { r = 10; }//se a velocidade estiver alta o steerRadius recebe mais assim faz curvas menos bruscas
        else if (speed >= 80) { r = 8; }
        else if (speed >= 60) { r = 6; }
        else if (speed >= 40) { r = 4; }
        else { r = 2.5f; }// se for menor que 40 diminui o steerAngle


        foreach (Wheel w in wheels)
        {
            if (w.axel == Axel.Front)//se for a roda da frente
            {
                if (input.x > 0) //esta indo para direita?
                {
                    switch (w.side)//testa se é a roda da direita ou esquerda
                    {
                        case Side.Left:
                            w.collider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (r + (1.5f / 2))) * input.x;
                            break;

                        case Side.Right:
                            w.collider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (r - (1.5f / 2))) * input.x;
                            break;
                    }
                }
                else if (input.x < 0)//quando tiver colocando para esquerda
                {
                    switch (w.side)
                    {
                        case Side.Left:
                            w.collider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (r - (1.5f / 2))) * input.x;
                            break;

                        case Side.Right:
                            w.collider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (r + (1.5f / 2))) * input.x;
                            break;
                    }
                }
                else
                {
                    w.collider.steerAngle = 0;
                }



                //float steerAngle = input.x * TurnSensitivity * maxSteelAngle;
                // w.collider.steerAngle = steerAngle;
            }
        }
    }

    void AnimateWheels()//método para animar as rodas
    {
        foreach (Wheel w in wheels)
        {
            Quaternion rot;//variavel rotação da roda
            Vector3 pos;//posição da roda
            w.collider.GetWorldPose(out pos, out rot);
            w.model.transform.position = pos;//recebe a posição do modelo da roda
            w.model.transform.rotation = rot;//recebe a rotação do modelo da roda
        }
    }

    #endregion
}
