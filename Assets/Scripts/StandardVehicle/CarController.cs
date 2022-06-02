using System;
using UnityEngine;

internal enum CarDriveType {
    FrontWheelDrive,
    RearWheelDrive,
    FourWheelDrive
}

public class CarController : MonoBehaviour {
    [SerializeField] private CarDriveType m_CarDriveType = CarDriveType.FourWheelDrive;
    [SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
    [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
    [SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[4];
    [SerializeField] private static int NoOfGears = 5;

    public CarSetting CarSettings;

    [System.Serializable]
    public class CarSetting {
        public Vector3 m_CentreOfMassOffset;
        public float m_SteerAngleSuperiorLimit = 25f;
        public float m_SteerAngleInferiorLimit = 5f;
        [Range(0, 1)] public float m_SteerHelper; // 0 is raw physics , 1 the car will grip in the direction it is facing
        [Range(0, 1)] public float m_TractionControl; // 0 is no traction control, 1 is full interference
        public float m_FullTorqueOverAllWheels = 2500f;
        public float m_ReverseTorque = 500f;
        public float m_MaxHandbrakeTorque = 500f;
        public float m_Downforce = 500f;
        [HideInInspector] public float m_DefaultDownforce = 500f;
        public float m_Topspeed = 220f;
        public float m_TopspeedBackwards = 60f;
        public float m_RevRangeBoundary = 1f;
        public float m_SlipLimit = 0.3f;
        public float m_BrakeTorque = 20000f;

        /////////////////////////////////////////////

        public float shiftDownRPM = 2000.0f;
        public float shiftUpRPM = 4000.0f;
        public float idleRPM = 500.0f;

        public float[] gears = { -10f, 9f, 6f, 4.5f, 3f, 2.5f }; // Number of gears of the car
        /////////////////////////////////////////////                                              /////////////////////////////////////////////
    }

    /// <summary>
    /// Value that will multiply current torque when Nitro is enabled.
    /// </summary>
    [SerializeField] private float m_NitroMultFactor;

    public bool NitroEnabled = false;

    /// <summary>
    /// Controls if car is on glued ground.
    /// </summary>
    private bool isGlued = false;
    private Timer glueTimer;
    /// <summary>
    /// Time the car keeps glued after leaving glued ground.
    /// </summary>
    private float timeGlued = 0.3f;
    private bool justEnteredGlue = false;
    
    /// <summary>
    /// Controls if car is on greased ground.
    /// </summary>
    private bool isGreased = false;
    private Timer greaseTimer;
    /// <summary>
    /// Time the car keeps glued after leaving greased ground.
    /// </summary>
    private float timeGreased = 0.1f;

    private WheelFrictionCurve defaultForwardFrictionCurve;
    private WheelFrictionCurve defaultSidewaysFrictionCurve;

    private float m_DefaultTopspeed;
    private Quaternion[] m_WheelMeshLocalRotations;
    private Vector3 m_Prevpos, m_Pos;
    private double m_SteerAngle;
    
    private int m_GearNum;
    /// <summary>
    /// 0 if the car is moving, 1 if neutral, 2 if rear
    /// </summary>
    private int m_GearNumMod;
    private float m_GearFactor;
    private float m_OldRotation;
    
    private float m_CurrentTorque;
    private Rigidbody m_Rigidbody;
    private const float k_ReversingThreshold = 0.01f;

    public CarSounds carSounds;

    [System.Serializable]
    public class CarSounds {
        public AudioSource IdleEngine, LowEngine, HighEngine;

        public AudioSource Nitro;
        public AudioSource SwitchGear;
    }

    public bool Skidding { get; private set; }
    public float BrakeInput { get; private set; }
    public double CurrentSteerAngle{ get { return m_SteerAngle; }}
    public float CurrentSpeed { 
        get { 
            return m_Rigidbody.velocity.magnitude * 2;
        }
    }
    /// <summary>
    /// Return current gear of car:
    /// </summary>
    public int CurrentGear { get { return currentGear; }}
    public float MaxSpeed{ get { return CarSettings.m_Topspeed; }}
    public float Revs { get; private set; }
    public float AccelInput { get; private set; }

    ///////////////////////////////////////////
    // new rpm count system
    private int currentGear = 0;
    private bool neutralGear = false;
    public bool backward = false; // controls if the car can go backwards

    private float motorRPM = 0.0f; // revolutions per time of motor

    public float MotorRPM { get { return motorRPM; } }

    private float wantedRPM = 0.0f;

    private float Pitch;
    private float PitchDelay;

    private float shiftTime = 0.0f;
    private float shiftDelay = 0.0f;
    ///////////////////////////////////////////

    private void CalculateRPM(float accel, bool brake) {
        wantedRPM = (5500.0f * accel) * 0.1f + wantedRPM * 0.9f;

        float rpm = 0.0f;
        int motorizedWheels = 0;
        //int currentWheel = 0; // determine what wheel is being controlled (manage sound)

        foreach (WheelCollider w in m_WheelColliders) {
            //WheelHit hit; variable to manage sound
            WheelCollider col = w;

            if (!neutralGear && brake && currentGear < 2) {
                rpm += accel * CarSettings.idleRPM;
            } else {
                if (!neutralGear) {
                    rpm += col.rpm;
                } else {
                    rpm += (CarSettings.idleRPM * accel);
                }
            }
            motorizedWheels++;
        }

        if (motorizedWheels > 1) {
            rpm /= motorizedWheels;
        }

        // changes the RPM of the motor
        motorRPM = 0.95f * motorRPM + 0.05f * Mathf.Abs(rpm * CarSettings.gears[currentGear]);
        
        if (motorRPM > 5500.0f) {
            motorRPM = 5200.0f;
        }

        // calculate pitch (keep it within reasonable bounds)
        Pitch = Mathf.Clamp(1.2f + ((motorRPM - CarSettings.idleRPM) / (CarSettings.shiftUpRPM - CarSettings.idleRPM)), 1.0f, 10.0f);

        shiftTime = Mathf.MoveTowards(shiftTime, 0.0f, 0.1f);

        if (Pitch == 1) {
            carSounds.IdleEngine.volume = Mathf.Lerp(carSounds.IdleEngine.volume, 1.0f, 0.1f);
            carSounds.LowEngine.volume = Mathf.Lerp(carSounds.LowEngine.volume, 0.5f, 0.1f);
            carSounds.HighEngine.volume = Mathf.Lerp(carSounds.HighEngine.volume, 0.0f, 0.1f);
        } else {
            carSounds.IdleEngine.volume = Mathf.Lerp(carSounds.IdleEngine.volume, 1.8f - Pitch, 0.1f);
            if ((Pitch > PitchDelay || accel > 0) && shiftTime == 0.0f) {
                carSounds.LowEngine.volume = Mathf.Lerp(carSounds.LowEngine.volume, 0.0f, 0.2f);
                carSounds.HighEngine.volume = Mathf.Lerp(carSounds.HighEngine.volume, 1.0f, 0.1f);
            } else {
                carSounds.LowEngine.volume = Mathf.Lerp(carSounds.LowEngine.volume, 0.5f, 0.1f);
                carSounds.HighEngine.volume = Mathf.Lerp(carSounds.HighEngine.volume, 0.0f, 0.2f);
            }

            carSounds.HighEngine.pitch = Pitch;
            carSounds.LowEngine.pitch = Pitch;

            PitchDelay = Pitch;
        }
    }

    private void ChangeGear(float accel, bool brake) {
        if (currentGear == 1 && accel < 0.0f) { // if player decelerates during the first gear
            if (CurrentSpeed < 5.0f) {
                ShiftGearDown();
            }
        } else if (currentGear == 0 && accel > 0.0f) { // if player accelerates during neutral gear
            if (CurrentSpeed < 5.0f) {
                ShiftGearUp();
            }
        } else if (motorRPM > CarSettings.shiftUpRPM && accel > 0.0f && CurrentSpeed > 10.0f && !brake) {
            ShiftGearUp();
        } else if (motorRPM < CarSettings.shiftDownRPM && currentGear > 1) {
            ShiftGearDown();
        }

        if (CurrentSpeed < 1.0f) { // if speed is less than 1, the car can go backwards
            backward = true;
        }

        if (currentGear == 0 && backward == true) {
            //  carSetting.shiftCentre.z = -accel / -5;
            if (CurrentSpeed < -10 * CarSettings.gears[0])
                accel = -accel;
        } else {
            backward = false;
        }
    }

    private void ShiftGearUp() {
        float now = Time.timeSinceLevelLoad;

        if (now < shiftDelay) {
            return;
        }

        if (currentGear < CarSettings.gears.Length - 1) { // verify if currentGear is in the maximum gear
            //if (!carSounds.SwitchGear.isPlaying)
            carSounds.SwitchGear.Play();

            currentGear++;

            shiftDelay = now + 1.0f;
            shiftTime = 1.5f;
        }
    }

    public void ShiftGearDown() {
        float now = Time.timeSinceLevelLoad;

        if (now < shiftDelay) {
            return;
        }

        if (currentGear > 0 || neutralGear) { // verify if currentGear is neutral or not
            //if (!carSounds.SwitchGear.isPlaying)
            carSounds.SwitchGear.Play();

            currentGear--;

            shiftDelay = now + 0.1f;
            shiftTime = 2.0f;
        }
    }

    // Use this for initialization
    private void Start() {
        m_WheelMeshLocalRotations = new Quaternion[4];
        for (int i = 0; i < 4; i++) {
            m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
        }
        m_WheelColliders[0].attachedRigidbody.centerOfMass = CarSettings.m_CentreOfMassOffset;

        CarSettings.m_MaxHandbrakeTorque = float.MaxValue;

        m_Rigidbody = GetComponent<Rigidbody>();
        m_CurrentTorque = CarSettings.m_FullTorqueOverAllWheels - (CarSettings.m_TractionControl * CarSettings.m_FullTorqueOverAllWheels);

        m_GearNumMod = -1;

        StoreDefaultValues();

        glueTimer = gameObject.AddComponent<Timer>();
        SetIsGlued(false);

        greaseTimer = gameObject.AddComponent<Timer>();
        SetIsGreased(false);
    }

    private void StoreDefaultValues() {
        m_DefaultTopspeed = CarSettings.m_Topspeed;

        CarSettings.m_DefaultDownforce = CarSettings.m_Downforce;

        WheelFrictionCurve fF = m_WheelColliders[0].forwardFriction;
        defaultForwardFrictionCurve = CreateFrictionCurve(fF.extremumSlip, fF.extremumValue, fF.asymptoteSlip, fF.asymptoteValue, fF.stiffness);

        WheelFrictionCurve sF = m_WheelColliders[0].sidewaysFriction;
        defaultSidewaysFrictionCurve = CreateFrictionCurve(sF.extremumSlip, sF.extremumValue, sF.asymptoteSlip, sF.asymptoteValue, sF.stiffness);
    }

    private WheelFrictionCurve CreateFrictionCurve(float extremumSlip, float extremumValue, float asymptoteSlip, float asymptoteValue, float stiffness) {
        WheelFrictionCurve newFrictionCurve = new WheelFrictionCurve();

        newFrictionCurve.extremumSlip = extremumSlip;
        newFrictionCurve.extremumValue = extremumValue;
        newFrictionCurve.asymptoteSlip = asymptoteSlip;
        newFrictionCurve.asymptoteValue = asymptoteValue;
        newFrictionCurve.stiffness = stiffness;

        return newFrictionCurve;
    }

    private void GearChanging() {
        float f = Mathf.Abs(CurrentSpeed/MaxSpeed);
        float upgearlimit = (1/(float) NoOfGears) * (m_GearNum + 1);
        float downgearlimit = (1/(float) NoOfGears) * m_GearNum;

        if (m_GearNum > 0 && f < downgearlimit) {
            m_GearNum--;
        }

        if (f > upgearlimit && (m_GearNum < (NoOfGears - 1))) {
            m_GearNum++;
        }

        if (m_GearNumMod >= 0) {
            m_GearNumMod = m_GearNum;
        }

        if (m_GearNumMod <= 0) {
            if (AccelInput == 0) { // if player is pressing or not the arrow down key
                if ((int)CurrentSpeed == 0) {
                    if (m_GearNumMod == 0) {
                        m_GearNumMod--;
                    }
                } else {
                    if (m_GearNumMod == -1) {
                        m_GearNumMod--;
                    }
                }
            } else if (AccelInput > 0) {
                if ((int)CurrentSpeed == 0) {
                    if (m_GearNumMod == -2) {
                        m_GearNumMod++;
                    }  
                } else {
                    if (m_GearNumMod == -1) {
                        m_GearNumMod++;
                    }
                }
            }
        }
    }


    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    private static float CurveFactor(float factor) {
        return 1 - (1 - factor)*(1 - factor);
    }


    // unclamped version of Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value) {
        return (1.0f - value) * from + value * to;
    }


    private void CalculateGearFactor() {
        float f = (1/(float) NoOfGears);
        // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
        // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
        var targetGearFactor = Mathf.InverseLerp(f * m_GearNum, f * (m_GearNum + 1), Mathf.Abs(CurrentSpeed/MaxSpeed));
        m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime * 5f);
    }


    private void CalculateRevs() {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = m_GearNum/(float) NoOfGears;
        var revsRangeMin = ULerp(0f, CarSettings.m_RevRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(CarSettings.m_RevRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
    }


    public void Move(float steering, float accel, float footbrake, bool handbrake) {
        if (isGlued) {
            if (glueTimer.GetSeconds() * 1000 + glueTimer.GetMilliseconds() >= timeGlued * 1000) {
                SetIsGlued(false);
                for (int i = 0; i < 4; i++) {
                    m_WheelColliders[i].forwardFriction = defaultForwardFrictionCurve;

                    m_WheelColliders[i].sidewaysFriction = defaultSidewaysFrictionCurve;

                    CarSettings.m_Downforce = CarSettings.m_DefaultDownforce;

                    justEnteredGlue = false;
                }
            } else {
                // modify wheel colliders values
                for (int i = 0; i < 4; i++) {
                    var forwardFriction = m_WheelColliders[i].forwardFriction;
                    m_WheelColliders[i].forwardFriction = CreateFrictionCurve(forwardFriction.extremumSlip, forwardFriction.extremumValue, forwardFriction.asymptoteSlip, forwardFriction.asymptoteValue, 0.2f);

                    var sidewaysFriction = m_WheelColliders[i].sidewaysFriction;
                    m_WheelColliders[i].sidewaysFriction = CreateFrictionCurve(sidewaysFriction.extremumSlip, sidewaysFriction.extremumValue, sidewaysFriction.asymptoteSlip, sidewaysFriction.asymptoteValue, 0.2f);

                    CarSettings.m_Downforce = CarSettings.m_DefaultDownforce * 2;

                    m_CurrentTorque = 100;

                    if (!justEnteredGlue) {
                        justEnteredGlue = true;

                        //float decreaseRate = 0.75f - 0.45f * VelocityRelativeToMax;
                        float decreaseRate = 0.3f;

                        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x * (1 - decreaseRate), m_Rigidbody.velocity.y * (1 - decreaseRate), m_Rigidbody.velocity.z) * (1 - decreaseRate);
                    }
                }
            }
        }

        if (isGreased) {
            if (greaseTimer.GetSeconds() * 1000 + greaseTimer.GetMilliseconds() >= timeGreased * 1000) {
                SetIsGreased(false);
                for (int i = 0; i < 4; i++) {
                    m_WheelColliders[i].forwardFriction = defaultForwardFrictionCurve;

                    m_WheelColliders[i].sidewaysFriction = defaultSidewaysFrictionCurve;
                }
            } else {
                // modify wheel colliders values
                for (int i = 0; i < 4; i++) {
                    var forwardFriction = m_WheelColliders[i].forwardFriction;
                    m_WheelColliders[i].forwardFriction = CreateFrictionCurve(forwardFriction.extremumSlip, forwardFriction.extremumValue, forwardFriction.asymptoteSlip, forwardFriction.asymptoteValue, 0.01f);

                    var sidewaysFriction = m_WheelColliders[i].sidewaysFriction;
                    m_WheelColliders[i].sidewaysFriction = CreateFrictionCurve(sidewaysFriction.extremumSlip, sidewaysFriction.extremumValue, sidewaysFriction.asymptoteSlip, sidewaysFriction.asymptoteValue, 0.01f);
                }
            }
        }

        for (int i = 0; i < 4; i++) {
            Quaternion quat;
            Vector3 position;
            m_WheelColliders[i].GetWorldPose(out position, out quat);
            m_WheelMeshes[i].transform.position = position;
            m_WheelMeshes[i].transform.rotation = quat;
        }

        CalculateRPM(accel, handbrake);
        ChangeGear(accel, handbrake);

        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        AccelInput = accel = Mathf.Clamp(accel, 0, 1);
        BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
        //handbrake = Mathf.Clamp(handbrake, 0, 1);

        //Set the steer on the front wheels.
        //Assuming that wheels 0 and 1 are the front wheels.
        m_SteerAngle = steering * GetMaxSteerAngle();
        m_WheelColliders[0].steerAngle = (float)m_SteerAngle;
        m_WheelColliders[1].steerAngle = (float)m_SteerAngle;
        SteerHelper();

        if (NitroEnabled) {
            carSounds.Nitro.volume = Mathf.Lerp(carSounds.Nitro.volume, 1.0f, Time.deltaTime * 10.0f);

            if (!carSounds.Nitro.isPlaying) {
                carSounds.Nitro.Play();
            }

            if (CarSettings.m_Topspeed >= m_DefaultTopspeed || CarSettings.m_Topspeed <= m_DefaultTopspeed * 1.5f) {
                CarSettings.m_Topspeed = m_DefaultTopspeed * 1.5f;
            }
            m_CurrentTorque *= m_NitroMultFactor;
            CarSettings.m_TractionControl = 0;
        } else {
            carSounds.Nitro.volume = Mathf.MoveTowards(carSounds.Nitro.volume, 0.0f, Time.deltaTime * 2.0f);

            if (carSounds.Nitro.volume == 0) {
                carSounds.Nitro.Stop();
            }

            CarSettings.m_TractionControl = 1;
            if (CarSettings.m_Topspeed > m_DefaultTopspeed) { // if car just deactivated Nitro
                CarSettings.m_Topspeed = Mathf.MoveTowards(CarSettings.m_Topspeed, m_DefaultTopspeed, Time.deltaTime * (1.5f - 1) * m_DefaultTopspeed * 20 / 100);
                
                if (CurrentSpeed <= m_DefaultTopspeed) {
                    CarSettings.m_Topspeed = m_DefaultTopspeed;
                } else if (CurrentSpeed <= CarSettings.m_Topspeed) {
                    CarSettings.m_Topspeed = CurrentSpeed;
                }
            }
        }

        ApplyDrive(accel, footbrake);

        if (accel == 0 && !NitroEnabled) { // if player is not accelerating/deaccelerating
            if (!backward) {
                DecreaseSpeed();
            }  
        }

        CapSpeed();

        //Set the handbrake.
        //Assuming that wheels 2 and 3 are the rear wheels.
        if (handbrake) {
            var hbTorque = handbrake == true ? 1f : 0f * CarSettings.m_MaxHandbrakeTorque;
            m_WheelColliders[2].brakeTorque = hbTorque;
            m_WheelColliders[3].brakeTorque = hbTorque;
        }
        
        CalculateRevs();
        GearChanging();

        AddDownForce();
        CheckForWheelSpin();

        if (NitroEnabled) {
            m_CurrentTorque /= m_NitroMultFactor;
        }

        TractionControl();
    }
    /// <summary>
    /// Decrease the speed of the car at a rate relative to the maximum speed.
    /// </summary>
    /// <param name="decreaseRate">.</param>
    private void DecreaseSpeed() {
        float maxSpeed = CarSettings.m_Topspeed / 2f;

        // Decrease rate of speed given in percent of the maximum speed per second;
        float decreaseRate; 

        if (m_Rigidbody.velocity.magnitude > maxSpeed * 0.9) {
            decreaseRate = 0.1f;
        } else if (m_Rigidbody.velocity.magnitude > maxSpeed * 0.8 && m_Rigidbody.velocity.magnitude <= maxSpeed * 0.9) {
            decreaseRate = 0.09f;
        } else if (m_Rigidbody.velocity.magnitude > maxSpeed * 0.7 && m_Rigidbody.velocity.magnitude <= maxSpeed * 0.8) {
            decreaseRate = 0.08f;
        } else if (m_Rigidbody.velocity.magnitude > maxSpeed * 0.6 && m_Rigidbody.velocity.magnitude <= maxSpeed * 0.7) {
            decreaseRate = 0.07f;
        } else if (m_Rigidbody.velocity.magnitude > maxSpeed * 0.5 && m_Rigidbody.velocity.magnitude <= maxSpeed * 0.6) {
            decreaseRate = 0.06f;
        } else if (m_Rigidbody.velocity.magnitude > maxSpeed * 0.4 && m_Rigidbody.velocity.magnitude <= maxSpeed * 0.5) {
            decreaseRate = 0.05f;
        } else if (m_Rigidbody.velocity.magnitude > maxSpeed * 0.3 && m_Rigidbody.velocity.magnitude <= maxSpeed * 0.4) {
            decreaseRate = 0.04f;
        } else if (m_Rigidbody.velocity.magnitude > maxSpeed * 0.2 && m_Rigidbody.velocity.magnitude <= maxSpeed * 0.3) {
            decreaseRate = 0.03f;
        } else if (m_Rigidbody.velocity.magnitude <= maxSpeed * 0.2) {
            decreaseRate = 0.01005f;
        } else {
            decreaseRate = 0.00f;
        }

        m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * Mathf.MoveTowards(m_Rigidbody.velocity.magnitude, 0, Time.deltaTime * maxSpeed * decreaseRate);
    }
    /// <summary>
    /// Get the maximum Steer Angle based on the velocity of the car.
    /// </summary>
    private double GetMaxSteerAngle() {
        double SpeedRatio = 100 * CurrentSpeed / MaxSpeed;
        
        if (SpeedRatio < 50) {
            return -0.0041f * SpeedRatio * SpeedRatio - 0.1061f * SpeedRatio + 25.25f;
        } else if (SpeedRatio == 50) {
            return 10;
        } else if (SpeedRatio > 50 && SpeedRatio < 90) {
            return 0.005f * SpeedRatio * SpeedRatio - 0.89f * SpeedRatio + 41.8f;
        } else {
            return 2.2f;
        }
    }

    private void CapSpeed() {
        if (backward) {
            if (CurrentSpeed > CarSettings.m_TopspeedBackwards) {
                m_Rigidbody.velocity = (CarSettings.m_TopspeedBackwards / 2) * m_Rigidbody.velocity.normalized;
            }
        } else {
            if (CurrentSpeed > CarSettings.m_Topspeed) {
                m_Rigidbody.velocity = (CarSettings.m_Topspeed / 2) * m_Rigidbody.velocity.normalized;
            }
        }
    }

    private void ApplyDrive(float accel, float footbrake) {
        float thrustTorque;
        switch (m_CarDriveType) {
            case CarDriveType.FourWheelDrive:
                thrustTorque = accel * (m_CurrentTorque / 4f);
                for (int i = 0; i < 4; i++) {
                    m_WheelColliders[i].motorTorque = thrustTorque;
                }
                break;
            case CarDriveType.FrontWheelDrive:
                thrustTorque = accel * (m_CurrentTorque / 2f);
                m_WheelColliders[0].motorTorque = m_WheelColliders[1].motorTorque = thrustTorque;
                break;
            case CarDriveType.RearWheelDrive:
                thrustTorque = accel * (m_CurrentTorque / 2f);
                m_WheelColliders[2].motorTorque = m_WheelColliders[3].motorTorque = thrustTorque;
                break;
        }

        for (int i = 0; i < 4; i++) {
            if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.velocity) < 50f) {
                m_WheelColliders[i].brakeTorque = CarSettings.m_BrakeTorque * footbrake;
            } else if (footbrake > 0) {
                m_WheelColliders[i].brakeTorque = 0f;
                m_WheelColliders[i].motorTorque = -CarSettings.m_ReverseTorque * footbrake;
            }
        }
    }


    private void SteerHelper() {
        for (int i = 0; i < 4; i++) {
            WheelHit wheelhit;
            m_WheelColliders[i].GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return; // wheels arent on the ground so dont realign the rigidbody velocity
        }

        // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
        if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f) {
            var turnadjust = (transform.eulerAngles.y - m_OldRotation) * CarSettings.m_SteerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            m_Rigidbody.velocity = velRotation * m_Rigidbody.velocity;
        }
        m_OldRotation = transform.eulerAngles.y;
    }


    // this is used to add more grip in relation to speed
    private void AddDownForce() {
        m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up * (CarSettings.m_Downforce * m_WheelColliders[0].attachedRigidbody.velocity.magnitude));
    }


    // checks if the wheels are spinning and is so does three things
    // 1) emits particles
    // 2) plays tiure skidding sounds
    // 3) leaves skidmarks on the ground
    // these effects are controlled through the WheelEffects class
    private void CheckForWheelSpin() {
        if (m_WheelEffects[0]) {
            // loop through all wheels
            for (int i = 0; i < 4; i++) {
                WheelHit wheelHit;
                m_WheelColliders[i].GetGroundHit(out wheelHit);

                // is the tire slipping above the given threshhold
                if (Mathf.Abs(wheelHit.forwardSlip) >= CarSettings.m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= CarSettings.m_SlipLimit) {
                    m_WheelEffects[i].EmitTyreSmoke();
                    // avoiding all four tires screeching at the same time
                    // if they do it can lead to some strange audio artefacts
                    if (!AnySkidSoundPlaying()) {
                        m_WheelEffects[i].PlayAudio();
                    }
                    continue;
                }

                // if it wasnt slipping stop all the audio
                if (m_WheelEffects[i].PlayingAudio) {
                    m_WheelEffects[i].StopAudio();
                }
                // end the trail generation
                m_WheelEffects[i].EndSkidTrail();
            }
        }
    }

    // crude traction control that reduces the power to wheel if the car is wheel spinning too much
    private void TractionControl() {
        WheelHit wheelHit;
        switch (m_CarDriveType) {
            case CarDriveType.FourWheelDrive:
                // loop through all wheels
                for (int i = 0; i < 4; i++) {
                    m_WheelColliders[i].GetGroundHit(out wheelHit);

                    AdjustTorque(wheelHit.forwardSlip);
                }
                break;
            case CarDriveType.RearWheelDrive:
                m_WheelColliders[2].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);

                m_WheelColliders[3].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);
                break;
            case CarDriveType.FrontWheelDrive:
                m_WheelColliders[0].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);

                m_WheelColliders[1].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);
                break;
        }
    }


    private void AdjustTorque(float forwardSlip) {
        if (forwardSlip >= CarSettings.m_SlipLimit && m_CurrentTorque >= 0) {
            m_CurrentTorque -= 10 * CarSettings.m_TractionControl;
        }
        else {
            m_CurrentTorque += 10 * CarSettings.m_TractionControl;
            if (m_CurrentTorque > CarSettings.m_FullTorqueOverAllWheels) {
                m_CurrentTorque = CarSettings.m_FullTorqueOverAllWheels;
            }
        }
    }


    private bool AnySkidSoundPlaying() {
        for (int i = 0; i < 4; i++) {
            if (m_WheelEffects[i].PlayingAudio) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Set car as Glued, resets and resumes the glue timer if value is true.
    /// Otherwise, set car as not glued and pauses timer.
    /// </summary>
    /// <param name="value">Defines if car is set to be glued or not</param>
    public void SetIsGlued(bool value) {
        if (value) {
            isGlued = true;
            glueTimer.ResetTimer();
            glueTimer.ResumeTimer();
        } else {
            isGlued = false;
            glueTimer.PauseTimer();
        }
    }

    /// <summary>
    /// Set car as Greased, resets and resumes the Grease timer if value is true.
    /// Otherwise, set car as not Greased and pauses timer.
    /// </summary>
    /// <param name="value">Defines if car is set to be Greased or not</param>
    public void SetIsGreased(bool value) {
        if (value) {
            isGreased = true;
            greaseTimer.ResetTimer();
            greaseTimer.ResumeTimer();
        } else {
            isGreased = false;
            greaseTimer.PauseTimer();
        }
    }
}