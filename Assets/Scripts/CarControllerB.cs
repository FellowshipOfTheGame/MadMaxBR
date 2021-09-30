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

public enum GearType//tipo de transmissão
{
    Automatic, Manual
}

[Serializable]//aparecer no inspector
public struct Wheel// estrutura para saber qual roda é
{
    public GameObject model;//modelo 3d da rodas
    public WheelCollider collider;//coliders da roda
    public Axel axel; //eixo da roda
    public Side side;//lado da roda
}

public class CarControllerB : MonoBehaviour
{
    public ControlMode controlMode;
    private CarMovementAI carMovementAI;
    public Wheel[] wheels;// rodas, o array[] vc define qts rodas são no inspector
    
    private float torque;//variavel controla torque
    
    public float breaktorque;//freios
   

    private Rigidbody rbCar;
    private Vector2 input;
    private bool isBreak;//freio

    public Transform centerofMass;//variavel do centro da massa do carro
    public float downForce;//força que o vento exerce sobre o carro
    

    public float SteerRadius;//abertura da roda, quanto maior menos vai ser a abertura da curva

    public Text speedTxt;//texto velocidade


    [Header("Engine")]// motor 
    public DriverType driverType;//variavel controla a tração
    [SerializeField]
    private float totalPower;//força de total do carro 

    
    public AnimationCurve enginePower;//curva para identificar o torque
    public GearType gerarType;
    public float[] gears;//marchas do carro
    public int gearInt;//marcha atual
    public float[] maxRPM;//maximo rpm para cambio automatico
    public float minRPM;//minimo rpm para mudar de marcha cambio automatico

    
    private float KPH;//velocidade que o carro está
    [SerializeField]
    private float engineRPM;//rotação atual do motor
    private float whellsRPM;//velocidade rotação das rodas
    private float smoothTime = 0.01f;

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
        CalculateEnginePower();
    }

    #region MEUS MÉTODOS

    private void GetInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Shifter();

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

            Shifter();
            isBreak = carMovementAI.brake;
        }
    }

    private void SetTorque()
    {

        KPH = rbCar.velocity.magnitude * 3.6f;

        speedTxt.text = KPH.ToString("N0") + "KMH";//marcar a velocidade no texto

        if(driverType == DriverType.full) { torque = totalPower / 4; } else { torque = totalPower / 2; }
        
        
        rbCar.AddForce(Vector3.down * downForce * KPH);//adicionar a força do vento de baixo para cima para ganho de estabilidade em velocidades altas

        foreach(Wheel w in wheels)//para cada roda
        {
            switch (driverType)//verifica se é a roda da frente ou tras e aplica tração
            {
                case DriverType.full:
                    w.collider.motorTorque = input.y * torque;//se for traction full aplica tração nas 4 rodas
                    break;

                case DriverType.Front:
                    if(w.axel == Axel.Front)// se a tração for na frente
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
        if (KPH >= 100) { r = 10; }//se a velocidade estiver alta o steerRadius recebe mais assim faz curvas menos bruscas
        else if (KPH >= 80) { r = 8; }
        else if (KPH >= 60) { r = 6; }
        else if (KPH >= 40) { r = 4; }
        else { r = 2.5f; }// se for menor que 40 diminui o steerAngle


        foreach (Wheel w in wheels)
        {
            if(w.axel == Axel.Front)//se for a roda da frente
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
                else if(input.x < 0)//quando tiver colocando para esquerda
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

    private void WhellRpm()//metodo calcular a velocidade das rodas
    {
        float sum = 0;
        int R = 0;

        foreach (Wheel w in wheels)//para cada roda
        {
            sum += w.collider.rpm;
            R++;
        }

        if(R !=0) { whellsRPM = sum / R; } else { whellsRPM = 0; }

        //whellsRPM = (R != 0) ? sum / R : 0;

    }

    private void CalculateEnginePower()//calculo da potencia do motor
    {
        WhellRpm();

        totalPower = enginePower.Evaluate(engineRPM) * gears[gearInt] * input.y;
        float velocity = 0;
        engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + Mathf.Abs(whellsRPM) * 3.6f * gears[gearInt], ref velocity, smoothTime);


    }

    private void Shifter()//método para troca de marchas
    {
        switch (gerarType)
        {
            case GearType.Automatic://configuração cambio automatico

                if(engineRPM > maxRPM[gearInt] && gearInt < gears.Length - 1)
                {
                    gearInt++;
                }
                else if(engineRPM < minRPM && gearInt > 0)
                {
                    gearInt--;
                }

                break;

            case GearType.Manual:

                if (Input.GetKeyDown(KeyCode.E) && gearInt < gears.Length - 1)//apertar E ´para subir a marcha
                {
                    gearInt++;
                }

                if (Input.GetKeyDown(KeyCode.Q) && gearInt > 0 )//apertar q para reduzir a marcha
                {
                    gearInt--;
                }

                break;
        }
    }

    #endregion
}
