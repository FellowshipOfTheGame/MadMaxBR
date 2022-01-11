using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreasePU : PowerUpBase {
    [SerializeField] private float maxGreaseAmount; // maximum amount of grease
    [SerializeField] private float UsePerSecond; // use of grease per second in percentage
    [SerializeField] private GameObject LiquidSpiller;

    private GameObject GreaseHUD; // grease hud
    private float curGreaseAmount; // amount of Grease
    private GameObject targetCar; // the car this script is attached

    public PowerUpData PowerUpInfo;

    public float MaxGreaseAmount { get { return maxGreaseAmount; } }

    public float CurGreaseAmount { get { return curGreaseAmount; } }
    private void Awake() {
        if (this.transform.parent.gameObject.transform.parent.gameObject.CompareTag("Player")) {
            GreaseHUD = PlayerDataDisplayer.Instance.GreaseHUD;
        } else {
            GreaseHUD = null;
        }
    }
    public override void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        curGreaseAmount = maxGreaseAmount;
        if (GreaseHUD != null) {
            GreaseHUD.SetActive(true);
        }
    }

    public override void Deactivate() {
        this.gameObject.SetActive(false);
        if (GreaseHUD != null) {
            GreaseHUD.SetActive(false);
        }
        LiquidSpiller.GetComponent<ParticleSystem>().Stop();
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Grease);
    }

    public override void UsePowerUp(bool useActive) {
        if (useActive) {
            if (curGreaseAmount != 0) {
                if (!LiquidSpiller.GetComponent<ParticleSystem>().isPlaying) {
                    LiquidSpiller.GetComponent<ParticleSystem>().Play();
                }
                curGreaseAmount = Mathf.MoveTowards(curGreaseAmount, 0f, Time.deltaTime * maxGreaseAmount * UsePerSecond / 100);
            }
        } else {
            if (!LiquidSpiller.GetComponent<ParticleSystem>().isStopped) {
                LiquidSpiller.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    // Update is called once per frame
    public override void Update() {
        if (curGreaseAmount == 0) {
            Deactivate();
        }
    }
}
