using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluePU : MonoBehaviour {
    public float MaxGlueAmount; // maximum amount of glue
    public float UsePerSecond; // use of glue per second in percentage
    public GameObject GlueHUD; // glue hud

    private float curGlueAmount; // amount of glue
    private GameObject targetCar; // the car this script is attached

    public GameObject LiquidSpiller;

    public float GetGlueAmount() {
        return curGlueAmount;
    }

    // Update is called once per frame
    private void Update() {
        if (curGlueAmount == 0) {
            this.gameObject.SetActive(false);
            GlueHUD.SetActive(false);
            LiquidSpiller.GetComponent<ParticleSystem>().Stop();
            targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Glue);
        } else {
            if (Input.GetKey(KeyCode.LeftShift)) {
                LiquidSpiller.GetComponent<ParticleSystem>().Play();
                curGlueAmount = Mathf.MoveTowards(curGlueAmount, 0f, Time.deltaTime * MaxGlueAmount * UsePerSecond / 100);
            } else {
                LiquidSpiller.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    public void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached to
        curGlueAmount = MaxGlueAmount;
        GlueHUD.SetActive(true);
        Instantiate(LiquidSpiller, this.transform.position, this.transform.rotation, this.transform.parent.transform);

    }
}
