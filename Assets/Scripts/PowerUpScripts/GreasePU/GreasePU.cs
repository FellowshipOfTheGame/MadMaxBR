using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreasePU : MonoBehaviour {
    [SerializeField] private KeyCode useButton = KeyCode.LeftShift;
    [SerializeField] private float maxGreaseAmount; // maximum amount of grease
    [SerializeField] private float UsePerSecond; // use of grease per second in percentage
    [SerializeField] private GameObject GreaseHUD; // grease hud
    [SerializeField] private GameObject LiquidSpiller;

    private float curGreaseAmount; // amount of Grease
    private GameObject targetCar; // the car this script is attached

    public PowerUpData PowerUpInfo;

    public float MaxGreaseAmount { get { return maxGreaseAmount; } }

    public float CurGreaseAmount { get { return curGreaseAmount; } }

    public void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        curGreaseAmount = maxGreaseAmount;
        if (GreaseHUD != null) {
            GreaseHUD.SetActive(true);
        }
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
        if (GreaseHUD != null) {
            GreaseHUD.SetActive(false);
        }
        LiquidSpiller.GetComponent<ParticleSystem>().Stop();
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Grease);
    }

    // Update is called once per frame
    private void Update() {
        if (curGreaseAmount == 0) {
            Deactivate();
        } else {
            if (Input.GetKey(useButton)) {
                if (!LiquidSpiller.GetComponent<ParticleSystem>().isPlaying) {
                    LiquidSpiller.GetComponent<ParticleSystem>().Play();
                }
                curGreaseAmount = Mathf.MoveTowards(curGreaseAmount, 0f, Time.deltaTime * maxGreaseAmount * UsePerSecond / 100);
            } else {
                if (!LiquidSpiller.GetComponent<ParticleSystem>().isStopped) {
                    LiquidSpiller.GetComponent<ParticleSystem>().Stop();
                }
            }
        }
    }
}
