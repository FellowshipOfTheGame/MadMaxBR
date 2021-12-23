using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluePU : PowerUpBase {
    [SerializeField] private KeyCode useButton = KeyCode.LeftShift;
    [SerializeField] private float maxGlueAmount; // maximum amount of glue
    [SerializeField] private float UsePerSecond; // use of glue per second in percentage
    [SerializeField] private GameObject GlueHUD; // glue hud
    [SerializeField] private GameObject LiquidSpiller;

    private float curGlueAmount; // amount of glue
    private GameObject targetCar; // the car this script is attached

    private bool pressingButton;

    public PowerUpData PowerUpInfo;

    public float MaxGlueAmount { get { return maxGlueAmount; } }

    public float CurGlueAmount { get { return curGlueAmount; } }

    public override void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        curGlueAmount = maxGlueAmount;
        if (GlueHUD != null) {
            GlueHUD.SetActive(true);
        }
        pressingButton = false;
    }

    public override void Deactivate() {
        this.gameObject.SetActive(false);
        if (GlueHUD != null) {
            GlueHUD.SetActive(false);
        }
        LiquidSpiller.GetComponent<ParticleSystem>().Stop();
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Glue);
        pressingButton = false;
    }

    public override void UsePowerUp(bool useActive) {
        if (useActive) {
            if (curGlueAmount != 0) {
                if (!LiquidSpiller.GetComponent<ParticleSystem>().isPlaying) {
                    LiquidSpiller.GetComponent<ParticleSystem>().Play();
                }
                curGlueAmount = Mathf.MoveTowards(curGlueAmount, 0f, Time.deltaTime * maxGlueAmount * UsePerSecond / 100);
            }
        } else {
            if (!LiquidSpiller.GetComponent<ParticleSystem>().isStopped) {
                LiquidSpiller.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    // Update is called once per frame
    public override void Update() {
        if (curGlueAmount == 0) {
            Deactivate();
        }
    }
}
