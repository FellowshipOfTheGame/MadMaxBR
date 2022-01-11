using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluePU : PowerUpBase {
    [SerializeField] private float maxGlueAmount; // maximum amount of glue
    [SerializeField] private float UsePerSecond; // use of glue per second in percentage
    [SerializeField] private GameObject LiquidSpiller;

    private GameObject GlueHUD; // glue hud
    private float curGlueAmount; // amount of glue
    private GameObject targetCar; // the car this script is attached

    public PowerUpData PowerUpInfo;

    public float MaxGlueAmount { get { return maxGlueAmount; } }

    public float CurGlueAmount { get { return curGlueAmount; } }
    private void Awake() {
        if (this.transform.parent.gameObject.transform.parent.gameObject.CompareTag("Player")) {
            GlueHUD = PlayerDataDisplayer.Instance.GlueHUD;
        } else {
            GlueHUD = null;
        }
    }
    public override void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        curGlueAmount = maxGlueAmount;
        if (GlueHUD != null) {
            GlueHUD.SetActive(true);
        }
    }

    public override void Deactivate() {
        this.gameObject.SetActive(false);
        if (GlueHUD != null) {
            GlueHUD.SetActive(false);
        }
        LiquidSpiller.GetComponent<ParticleSystem>().Stop();
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Glue);
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
