using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataDisplayer : MonoBehaviour {
    public GameObject Player; // stores the object that represents the player
    public RaceManager RaceManager;

    public GameObject LapCounterDisplay;
    public GameObject MaxNumberOfLaps;
    public GameObject RacePositionDisplay;
    public GameObject MaxRacePositionDisplay;

    public GameObject NitroHUD;
    public GameObject SmokeHUD;
    public GameObject PlayerHealthHUD;
    public GameObject PlayerShieldHUD;

    public GameObject PowerUpSlot1;
    public GameObject PowerUpSlot2;
    public GameObject PowerUpSlot3;
    public GameObject PowerUpSlot4;

    private GameObject PlayerPowerUps;
    private VehicleRaceData PlayerRaceData;

    // Start is called before the first frame update
    void Start() {
        PlayerRaceData = Player.GetComponent<VehicleRaceData>();
        PlayerPowerUps = Player.transform.GetComponentInChildren<PowerUp>().gameObject;
        if (PlayerPowerUps == null)
            Debug.Log("NULL");
        //Player = Player.transform.GetChild(0).gameObject;
        //NitroUI = PlayerCarDataUI.transform.GetChild(1).gameObject;
        //PlayerHealthHUD = PlayerCarDataUI.transform.GetChild(2).transform.GetChild(0).gameObject;
        //PlayerShieldHUD = PlayerCarDataUI.transform.GetChild(3).transform.GetChild(0).gameObject;
    }

    // Update is called once per frameGetPosition
    void Update() {
        LapCounterDisplay.GetComponent<Text>().text = "" + PlayerRaceData.GetLapCount();
        RacePositionDisplay.GetComponent<Text>().text = "" + PlayerRaceData.GetRacePosition();
        MaxRacePositionDisplay.GetComponent<Text>().text = "" + RaceManager.Racers.Count;
        MaxNumberOfLaps.GetComponent<Text>().text = "" + RaceManager.NumberOfLaps;
        // updates health and shield bar
        PlayerShieldHUD.GetComponent<Image>().fillAmount = Player.GetComponent<VehicleData>().GetCurrentShield() / Player.GetComponent<VehicleData>().MaxCarShield;
        PlayerHealthHUD.GetComponent<Image>().fillAmount = Player.GetComponent<VehicleData>().GetCurrentHealth() / Player.GetComponent<VehicleData>().MaxCarHealth;
        // updates PowerUpSlot value
        PowerUpSlot1.GetComponent<Text>().text = "" + Player.GetComponent<VehicleData>().GetPowerUpSlotValue(1);
        PowerUpSlot2.GetComponent<Text>().text = "" + Player.GetComponent<VehicleData>().GetPowerUpSlotValue(2);
        PowerUpSlot3.GetComponent<Text>().text = "" + Player.GetComponent<VehicleData>().GetPowerUpSlotValue(3);
        PowerUpSlot4.GetComponent<Text>().text = "" + Player.GetComponent<VehicleData>().GetPowerUpSlotValue(4);
        // updates nitro
        if (NitroHUD.activeSelf) {
            NitroHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = PlayerPowerUps.GetComponentInChildren<NitroPU>().GetNitroAmount() / PlayerPowerUps.GetComponentInChildren<NitroPU>().MaxNitroAmount;
        }

        if (SmokeHUD.activeSelf) {
            SmokeHUD.GetComponent<Image>().fillAmount = PlayerPowerUps.GetComponentInChildren<SmokePU>().GetSmokeAmount() / PlayerPowerUps.GetComponentInChildren<SmokePU>().MaxSmokeAmount;
        }


        /*
        if (PlayerPowerUps.transform.GetChild(0).gameObject.activeSelf) { // if nitro power up is active
            if (!NitroUI.activeSelf) {
                NitroUI.SetActive(true);
            }
            UpdateNitroBar(PlayerPowerUps.transform.GetChild(0).GetComponent<NitroPU>().GetNitroAmount());
        }*/
    }

    /*
    public void UpdateNitroBar(float amount) {
        Debug.Log(amount);
        if (amount < 1) {
            NitroUI.SetActive(false);
        } else {
            GameObject NitroBar = NitroUI.transform.GetChild(0).gameObject; // gets the nitro bar of UI that represents the amount of nitro available
            NitroBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, amount * 2);
        }
    }
    */
}
