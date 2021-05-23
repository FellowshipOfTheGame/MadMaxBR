using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataDisplayer : MonoBehaviour {
    public GameObject Player; // stores the object that represents the player
    public GameObject NitroUI;
    public GameObject PlayerHealthBar;
    public GameObject PlayerShieldBar;

    public GameObject PowerUpSlot1;
    public GameObject PowerUpSlot2;
    public GameObject PowerUpSlot3;
    public GameObject PowerUpSlot4;

    private GameObject PlayerCar;
    private GameObject PlayerCarData;
    private GameObject PlayerPowerUps;

    // Start is called before the first frame update
    void Start() {
        PlayerCar = Player.transform.GetChild(0).gameObject;
        PlayerPowerUps = PlayerCar.transform.GetChild(8).gameObject;
        //NitroUI = PlayerCarDataUI.transform.GetChild(1).gameObject;
        //PlayerHealthBar = PlayerCarDataUI.transform.GetChild(2).transform.GetChild(0).gameObject;
        //PlayerShieldBar = PlayerCarDataUI.transform.GetChild(3).transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update() {
        // updates health and shield bar
        PlayerShieldBar.GetComponent<Image>().fillAmount = PlayerCar.GetComponent<VehicleData>().GetCurrentShield() / PlayerCar.GetComponent<VehicleData>().MaxCarShield;
        PlayerHealthBar.GetComponent<Image>().fillAmount = PlayerCar.GetComponent<VehicleData>().GetCurrentHealth() / PlayerCar.GetComponent<VehicleData>().MaxCarHealth;
        PowerUpSlot1.GetComponent<Text>().text = "" + PlayerCar.GetComponent<VehicleData>().GetPowerUpSlotValue(1);
        PowerUpSlot2.GetComponent<Text>().text = "" + PlayerCar.GetComponent<VehicleData>().GetPowerUpSlotValue(2);
        PowerUpSlot3.GetComponent<Text>().text = "" + PlayerCar.GetComponent<VehicleData>().GetPowerUpSlotValue(3);
        PowerUpSlot4.GetComponent<Text>().text = "" + PlayerCar.GetComponent<VehicleData>().GetPowerUpSlotValue(4);

        if (PlayerPowerUps.transform.GetChild(0).gameObject.activeSelf) { // if nitro power up is active
            if (!NitroUI.activeSelf) {
                NitroUI.SetActive(true);
            }
            UpdateNitroBar(PlayerPowerUps.transform.GetChild(0).GetComponent<NitroPU>().GetNitroAmount());
        }
    }

    public void UpdateNitroBar(float amount) {
        Debug.Log(amount);
        if (amount < 1) {
            NitroUI.SetActive(false);
        } else {
            GameObject NitroBar = NitroUI.transform.GetChild(0).gameObject; // gets the nitro bar of UI that represents the amount of nitro available
            NitroBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, amount * 2);
        }
    }
}
