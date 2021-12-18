using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroPU : MonoBehaviour {
    [SerializeField] private KeyCode useButton = KeyCode.LeftShift;
    [SerializeField] private float maxNitroAmount; // maximum amount of nitro
    [SerializeField] private float UsePerSecond; // use of nitro per second in percentage
    [SerializeField] private GameObject NitroHUD; // nitro HUD
    [SerializeField] private GameObject NitroEffect1;
    [SerializeField] private GameObject NitroEffect2;

    private float curNitroAmount; // amount of car nitro
    private GameObject targetCar; // the car this script is attached

    public PowerUpData PowerUpInfo;

    public float MaxNitroAmount { get { return maxNitroAmount; } }

    public float CurNitroAmount { get { return curNitroAmount; } }

    public void Activate() {
        curNitroAmount = maxNitroAmount; // set maximum value of nitro amount
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

    private void Update() {
        if (curNitroAmount == 0) {
            Deactivate();
        } else {
            if (Input.GetKey(useButton)) {
                targetCar.GetComponent<CarController>().NitroEnabled = true;
                if (!NitroEffect1.GetComponent<ParticleSystem>().isPlaying) {
                    NitroEffect1.GetComponent<ParticleSystem>().Play();
                    NitroEffect2.GetComponent<ParticleSystem>().Play();
                }
                curNitroAmount = Mathf.MoveTowards(curNitroAmount, 0f, Time.deltaTime * maxNitroAmount * UsePerSecond / 100);
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
