using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NitroPU : MonoBehaviour {
    public float MaxNitroAmount; // maximum amount of nitro
    public float UsePerSecond; // use of nitro per second in percentage
    public GameObject NitroHUD; // nitro HUD
    public GameObject NitroEffect1;
    public GameObject NitroEffect2;

    private float curNitroAmount; // amount of car nitro
    private GameObject targetCar; // the car this script is attached
    public void Activate() {
        curNitroAmount = MaxNitroAmount; // set maximum value of nitro amount
        NitroHUD.SetActive(true);
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        NitroHUD.SetActive(false);
        targetCar.GetComponent<CarController>().NitroEnabled = false;
        NitroEffect1.GetComponent<ParticleSystem>().Stop();
        NitroEffect2.GetComponent<ParticleSystem>().Stop();
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Nitro);
    }

    public float GetNitroAmount() {
        return curNitroAmount;
    }

    private void Update() {
        if (curNitroAmount == 0) {
            Deactivate();
        } else {
            if (Input.GetKey(KeyCode.LeftShift)) {
                targetCar.GetComponent<CarController>().NitroEnabled = true;
                if (!NitroEffect1.GetComponent<ParticleSystem>().isPlaying) {
                    NitroEffect1.GetComponent<ParticleSystem>().Play();
                    NitroEffect2.GetComponent<ParticleSystem>().Play();
                }
                curNitroAmount = Mathf.MoveTowards(curNitroAmount, 0f, Time.deltaTime * MaxNitroAmount * UsePerSecond / 100);
            } else {
                targetCar.GetComponent<CarController>().NitroEnabled = false;
                if (!NitroEffect1.GetComponent<ParticleSystem>().isStopped) {
                    NitroEffect1.GetComponent<ParticleSystem>().Stop();
                    NitroEffect2.GetComponent<ParticleSystem>().Stop();
                }
            }
        }
    }
}
