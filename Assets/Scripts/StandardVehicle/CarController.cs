using System;
using UnityEngine;

internal enum CarDriveType
{
    FrontWheelDrive,
    RearWheelDrive,
    FourWheelDrive
}

internal enum SpeedType
{
    MPH,
    KPH
}

public class CarController : MonoBehaviour {
    [SerializeField] private CarDriveType m_CarDriveType = CarDriveType.FourWheelDrive;
    [SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
    [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
    [SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[4];
    [SerializeField] private Vector3 m_CentreOfMassOffset;
    [SerializeField] private float m_MaximumSteerAngle;
    [Range(0, 1)] [SerializeField] private float m_SteerHelper; // 0 is raw physics , 1 the car will grip in the direction it is facing
    [Range(0, 1)] [SerializeField] private float m_TractionControl; // 0 is no traction control, 1 is full interference
    [SerializeField] private float m_FullTorqueOverAllWheels;
    [SerializeField] private float m_ReverseTorque;
    [SerializeField] private float m_MaxHandbrakeTorque;
    [SerializeField] private float m_Downforce = 100f;
    private float m_DefaultDownforce = 100f;
    [SerializeField] private SpeedType m_SpeedType;
    [SerializeField] private float m_Topspeed = 200;
    [SerializeField] private static int NoOfGears = 5;
    [SerializeField] private float m_RevRangeBoundary = 1f;
    [SerializeField] private float m_SlipLimit;
    [SerializeField] private float m_BrakeTorque;
    /// <summary>
    /// Value that will multiply current torque when nitro is enabled.
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
    private float m_SteerAngle;
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

    public bool Skidding { get; private set; }
    public float BrakeInput { get; private set; }
    public float CurrentSteerAngle{ get { return m_SteerAngle; }}
    public float CurrentSpeed { 
        get { 
            if (m_SpeedType == SpeedType.MPH) { 
                return m_Rigidbody.velocity.magnitude * 2.23693629f; 
            } else { 
                return m_Rigidbody.velocity.magnitude * 3.6f; 
            } 
        }
    }
    /// <summary>
    /// Return current gear of car:
    /// 0 to 4 means the car is moving, 
    /// -1 if the car is stopped (neutral), 
    /// -2 if the car is rearing
    /// </summary>
    public int CurrentGear { get { return m_GearNumMod; }}
    public float MaxSpeed{ get { return m_Topspeed; }}
    public float Revs { get; private set; }
    public float AccelInput { get; private set; }

    // Use this for initialization
    private void Start() {
        m_WheelMeshLocalRotations = new Quaternion[4];
        for (int i = 0; i < 4; i++) {
            m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
        }
        m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;

        m_MaxHandbrakeTorque = float.MaxValue;

        m_Rigidbody = GetComponent<Rigidbody>();
        m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);

        m_GearNumMod = -1;

        StoreDefaultValues();

        glueTimer = gameObject.AddComponent<Timer>();
        SetIsGlued(false);

        greaseTimer = gameObject.AddComponent<Timer>();
        SetIsGreased(false);
    }

    private void StoreDefaultValues() {
        m_DefaultTopspeed = m_Topspeed;

        m_DefaultDownforce = m_Downforce;

        WheelFrictionCurve fF = m_WheelColliders[0].GetComponent<WheelCollider>().forwardFriction;
        defaultForwardFrictionCurve = CreateFrictionCurve(fF.extremumSlip, fF.extremumValue, fF.asymptoteSlip, fF.asymptoteValue, fF.stiffness);

        WheelFrictionCurve sF = m_WheelColliders[0].GetComponent<WheelCollider>().sidewaysFriction;
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
        var targetGearFactor = Mathf.InverseLerp(f*m_GearNum, f*(m_GearNum + 1), Mathf.Abs(CurrentSpeed/MaxSpeed));
        m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime*5f);
    }


    private void CalculateRevs() {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = m_GearNum/(float) NoOfGears;
        var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
    }


    public void Move(float steering, float accel, float footbrake, bool handbrake) {
        if (isGlued) {
            if (glueTimer.GetSeconds() * 1000 + glueTimer.GetMilliseconds() >= timeGlued * 1000) {
                SetIsGlued(false);
                for (int i = 0; i < 4; i++) {
                    m_WheelColliders[i].GetComponent<WheelCollider>().forwardFriction = defaultForwardFrictionCurve;

                    m_WheelColliders[i].GetComponent<WheelCollider>().sidewaysFriction = defaultSidewaysFrictionCurve;

                    m_Downforce = m_DefaultDownforce;

                    justEnteredGlue = false;
                }
            } else {
                Debug.Log("glued");
                // modify wheel colliders values
                for (int i = 0; i < 4; i++) {
                    var forwardFriction = m_WheelColliders[i].GetComponent<WheelCollider>().forwardFriction;
                    m_WheelColliders[i].GetComponent<WheelCollider>().forwardFriction = CreateFrictionCurve(forwardFriction.extremumSlip, forwardFriction.extremumValue, forwardFriction.asymptoteSlip, forwardFriction.asymptoteValue, 0.2f);

                    var sidewaysFriction = m_WheelColliders[i].GetComponent<WheelCollider>().sidewaysFriction;
                    m_WheelColliders[i].GetComponent<WheelCollider>().sidewaysFriction = CreateFrictionCurve(sidewaysFriction.extremumSlip, sidewaysFriction.extremumValue, sidewaysFriction.asymptoteSlip, sidewaysFriction.asymptoteValue, 0.2f);

                    m_Downforce = m_DefaultDownforce * 2;

                    m_CurrentTorque = 100;

                    if (!justEnteredGlue) {
                        justEnteredGlue = true;

                        float VelocityRelativeToMax;

                        if (m_SpeedType == SpeedType.MPH) {
                            VelocityRelativeToMax = m_Rigidbody.velocity.magnitude * 2.23693629f / m_Topspeed;
                        } else {
                            VelocityRelativeToMax = m_Rigidbody.velocity.magnitude * 3.6f / m_Topspeed;
                        }

                        float decreaseRate = 0.75f - 0.45f * VelocityRelativeToMax;

                        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x * (1 - decreaseRate), m_Rigidbody.velocity.y * (1 - decreaseRate), m_Rigidbody.velocity.z) * (1 - decreaseRate);
                    }
                }
            }
        }

        if (isGreased) {
            if (greaseTimer.GetSeconds() * 1000 + greaseTimer.GetMilliseconds() >= timeGreased * 1000) {
                SetIsGreased(false);
                for (int i = 0; i < 4; i++) {
                    m_WheelColliders[i].GetComponent<WheelCollider>().forwardFriction = defaultForwardFrictionCurve;

                    m_WheelColliders[i].GetComponent<WheelCollider>().sidewaysFriction = defaultSidewaysFrictionCurve;
                }
            } else {
                Debug.Log("greased");
                // modify wheel colliders values
                for (int i = 0; i < 4; i++) {
                    var forwardFriction = m_WheelColliders[i].GetComponent<WheelCollider>().forwardFriction;
                    m_WheelColliders[i].GetComponent<WheelCollider>().forwardFriction = CreateFrictionCurve(forwardFriction.extremumSlip, forwardFriction.extremumValue, forwardFriction.asymptoteSlip, forwardFriction.asymptoteValue, 0.01f);

                    var sidewaysFriction = m_WheelColliders[i].GetComponent<WheelCollider>().sidewaysFriction;
                    m_WheelColliders[i].GetComponent<WheelCollider>().sidewaysFriction = CreateFrictionCurve(sidewaysFriction.extremumSlip, sidewaysFriction.extremumValue, sidewaysFriction.asymptoteSlip, sidewaysFriction.asymptoteValue, 0.01f);
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

        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        AccelInput = accel = Mathf.Clamp(accel, 0, 1);
        BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
        //handbrake = Mathf.Clamp(handbrake, 0, 1);

        //Set the steer on the front wheels.
        //Assuming that wheels 0 and 1 are the front wheels.
        m_SteerAngle = steering * m_MaximumSteerAngle;
        m_WheelColliders[0].steerAngle = m_SteerAngle;
        m_WheelColliders[1].steerAngle = m_SteerAngle;
        SteerHelper();

        if (NitroEnabled) {
            if (m_Topspeed >= m_DefaultTopspeed || m_Topspeed <= m_DefaultTopspeed * 1.5f) {
                m_Topspeed = m_DefaultTopspeed * 1.5f;
            }
            m_CurrentTorque *= m_NitroMultFactor;
            m_TractionControl = 0;
        } else {
            m_TractionControl = 1;
            if (m_Topspeed > m_DefaultTopspeed) { // if car just deactivated nitro
                m_Topspeed = Mathf.MoveTowards(m_Topspeed, m_DefaultTopspeed, Time.deltaTime * (1.5f - 1) * m_DefaultTopspeed * 20 / 100);
                
                if (CurrentSpeed <= m_DefaultTopspeed) {
                    m_Topspeed = m_DefaultTopspeed;
                } else if (CurrentSpeed <= m_Topspeed) {
                    m_Topspeed = CurrentSpeed;
                }
            }
        }

        ApplyDrive(accel, footbrake);

        CapSpeed();

        //Set the handbrake.
        //Assuming that wheels 2 and 3 are the rear wheels.
        if (handbrake) {
            var hbTorque = handbrake == true ? 1f : 0f * m_MaxHandbrakeTorque;
            Debug.Log(hbTorque);
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


    private void CapSpeed() {
        float speed = m_Rigidbody.velocity.magnitude;
        switch (m_SpeedType) {
            case SpeedType.MPH:
                speed *= 2.23693629f;
                if (speed > m_Topspeed)
                    m_Rigidbody.velocity = (m_Topspeed/2.23693629f) * m_Rigidbody.velocity.normalized;
                break;
            case SpeedType.KPH:
                speed *= 3.6f;
                if (speed > m_Topspeed)
                    m_Rigidbody.velocity = (m_Topspeed/3.6f) * m_Rigidbody.velocity.normalized;
                break;
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
                m_WheelColliders[i].brakeTorque = m_BrakeTorque * footbrake;
            } else if (footbrake > 0) {
                m_WheelColliders[i].brakeTorque = 0f;
                m_WheelColliders[i].motorTorque = -m_ReverseTorque * footbrake;
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
            var turnadjust = (transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            m_Rigidbody.velocity = velRotation * m_Rigidbody.velocity;
        }
        m_OldRotation = transform.eulerAngles.y;
    }


    // this is used to add more grip in relation to speed
    private void AddDownForce() {
        m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up*m_Downforce * m_WheelColliders[0].attachedRigidbody.velocity.magnitude);
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
                if (Mathf.Abs(wheelHit.forwardSlip) >= m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= m_SlipLimit) {
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
        if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0) {
            m_CurrentTorque -= 10 * m_TractionControl;
        }
        else {
            m_CurrentTorque += 10 * m_TractionControl;
            if (m_CurrentTorque > m_FullTorqueOverAllWheels) {
                m_CurrentTorque = m_FullTorqueOverAllWheels;
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