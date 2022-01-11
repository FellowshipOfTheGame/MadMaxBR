using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroPU : PowerUpBase {
    [SerializeField] private float maxNitroAmount; // maximum amount of nitro
    [SerializeField] private float UsePerSecond; // use of nitro per second in percentage
    [SerializeField] private GameObject[] NitroEffects;

    private GameObject NitroHUD; // nitro HUD
    private float curNitroAmount; // amount of car nitro
    private GameObject targetCar; // the car this script is attached

    public PowerUpData PowerUpInfo;
    private void Awake() {
        if (this.transform.parent.gameObject.transform.parent.gameObject.CompareTag("Player")) {
            NitroHUD = PlayerDataDisplayer.Instance.NitroHUD;
        } else {
            NitroHUD = null;
        }
    }
    public float MaxNitroAmount { get { return maxNitroAmount; } }

    public float CurNitroAmount { get { return curNitroAmount; } }

    public override void Activate() {
        curNitroAmount = maxNitroAmount; // set maximum value of nitro amount
        if (NitroHUD != null) {
            NitroHUD.SetActive(true);
        }
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached
    }

    public override void Deactivate() {
        this.gameObject.SetActive(false);
        if (NitroHUD != null) {
            NitroHUD.SetActive(false);
        }
        targetCar.GetComponent<CarController>().NitroEnabled = false;

        for (int i = 0; i < NitroEffects.Length; i++) {
            NitroEffects[i].GetComponent<ParticleSystem>().Stop();
        }
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Nitro);
    }

    public override void UsePowerUp(bool useActive) {
        if (useActive) {
            if (curNitroAmount != 0) {
                targetCar.GetComponent<CarController>().NitroEnabled = true;

                if (!NitroEffects[0].GetComponent<ParticleSystem>().isPlaying) {
                    for (int i = 0; i < NitroEffects.Length; i++) {
                        NitroEffects[i].GetComponent<ParticleSystem>().Play();
                    }
                }

                curNitroAmount = Mathf.MoveTowards(curNitroAmount, 0f, Time.deltaTime * maxNitroAmount * UsePerSecond / 100);
            }
        } else {
            targetCar.GetComponent<CarController>().NitroEnabled = false;

            if (!NitroEffects[0].GetComponent<ParticleSystem>().isStopped) {
                for (int i = 0; i < NitroEffects.Length; i++) {
                    NitroEffects[i].GetComponent<ParticleSystem>().Stop();
                }
            }
        }
    }

    public override void Update() {
        if (curNitroAmount == 0) {
            Deactivate();
        }
    }
}
