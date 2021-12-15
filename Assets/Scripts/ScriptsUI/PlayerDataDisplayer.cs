using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataDisplayer : MonoBehaviour {
    public GameObject Player; // stores the object that represents the player
    public RaceManager RaceManager;

    public GameObject VelocityDisplay;
    public GameObject LapCounterDisplay;
    public GameObject MaxNumberOfLaps;
    public GameObject RacePositionDisplay;
    public GameObject MaxRacePositionDisplay;

    public GameObject MachineGunCountText;
    public GameObject RifleCountText;
    public GameObject ThornsTimerUI;
    public GameObject NitroHUD;
    public GameObject SmokeHUD;
    public GameObject ExplosiveMineCount;
    public GameObject DeactivatorMineCount;
    public GameObject PillarCount;
    public GameObject GlueHUD;
    public GameObject GreaseHUD;
    public GameObject PlayerHealthHUD;
    public GameObject PlayerShieldHUD;

    public GameObject PowerUpSlot1;
    public GameObject PowerUpSlot2;
    public GameObject PowerUpSlot3;
    public GameObject PowerUpSlot4;

    public GameObject PowerUpTutorialText;

    private GameObject PlayerPowerUps;
    private VehicleRaceData PlayerRaceData;

    private Queue<string> queueTutorialText;
    [SerializeField] private float TutorialTextStayTime;
    private float queueTimeWaited;

    // Start is called before the first frame update
    void Start() {
        PlayerRaceData = Player.GetComponent<VehicleRaceData>();
        PlayerPowerUps = Player.transform.GetComponentInChildren<PowerUp>().gameObject;
        //Player = Player.transform.GetChild(0).gameObject;
        //NitroUI = PlayerCarDataUI.transform.GetChild(1).gameObject;
        //PlayerHealthHUD = PlayerCarDataUI.transform.GetChild(2).transform.GetChild(0).gameObject;
        //PlayerShieldHUD = PlayerCarDataUI.transform.GetChild(3).transform.GetChild(0).gameObject;
    }

    // Update is called once per frameGetPosition
    void Update() {
        // update race data
        UpdateRaceData();
        // update health and shield bars
        UpdateUIBars();
        // updates PowerUpSlot value
        UpdatePowerupSlot();
        // updates tutorial text
        UpdateTutorialText();
        // update powerUp information
        UpdatePowerUpUI();
    }

    private void UpdateRaceData() {
        LapCounterDisplay.GetComponent<Text>().text = "" + PlayerRaceData.GetLapCount();
        RacePositionDisplay.GetComponent<Text>().text = "" + PlayerRaceData.GetRacePosition();
        MaxRacePositionDisplay.GetComponent<Text>().text = "" + RaceManager.Racers.Count;
        MaxNumberOfLaps.GetComponent<Text>().text = "" + RaceManager.NumberOfLaps;
    }

    private void UpdateUIBars() {
        // updates health and shield bar
        PlayerShieldHUD.GetComponent<Image>().fillAmount = Player.GetComponent<VehicleData>().GetCurrentShield() / Player.GetComponent<VehicleData>().MaxCarShield;
        PlayerHealthHUD.GetComponent<Image>().fillAmount = Player.GetComponent<VehicleData>().GetCurrentHealth() / Player.GetComponent<VehicleData>().MaxCarHealth;
    }

    private void UpdatePowerupSlot() {
        PowerUpSlot1.GetComponent<Text>().text = "" + Player.GetComponent<VehicleData>().GetPowerUpSlotValue(1);
        PowerUpSlot2.GetComponent<Text>().text = "" + Player.GetComponent<VehicleData>().GetPowerUpSlotValue(2);
        PowerUpSlot3.GetComponent<Text>().text = "" + Player.GetComponent<VehicleData>().GetPowerUpSlotValue(3);
        PowerUpSlot4.GetComponent<Text>().text = "" + Player.GetComponent<VehicleData>().GetPowerUpSlotValue(4);
    }

    private void UpdateTutorialText() {
        for (int i = 1; i < 5; i++) {
            switch (Player.GetComponent<VehicleData>().GetPowerUpSlotValue(i)) {
                case (int)PowerUpName.MachineGun:

                    break;
                case (int)PowerUpName.Rifle:

                    break;
                case (int)PowerUpName.Thorns:

                    break;
                case (int)PowerUpName.Shield:

                    break;
                case (int)PowerUpName.Fix:

                    break;
                case (int)PowerUpName.Smoke:

                    break;
                case (int)PowerUpName.ExplosiveMine:

                    break;
                case (int)PowerUpName.DeactivatorMine:

                    break;
                case (int)PowerUpName.Pillar:

                    break;
                case (int)PowerUpName.Nitro:

                    break;
                case (int)PowerUpName.Glue:

                    break;
                case (int)PowerUpName.Grease:

                    break;
            }
        }
    }

    private void UpdatePowerUpUI() {
        // updates bullet count text with quantity of MachineGun
        if (MachineGunCountText.activeSelf) {
            MachineGunCountText.GetComponent<Text>().text = PlayerPowerUps.GetComponentInChildren<MachineGunPU>().GetBulletAmount().ToString();
        }
        // updates bullet count text with quantity of Rifle
        if (RifleCountText.activeSelf) {
            RifleCountText.GetComponent<Text>().text = PlayerPowerUps.GetComponentInChildren<RiflePU>().GetBulletAmount().ToString();
        }
        // update thorns timer ui
        if (ThornsTimerUI.activeSelf) {
            ThornsTimerUI.GetComponent<Image>().fillAmount = 1 - PlayerPowerUps.GetComponentInChildren<ThornsPU>().GetRunningTime() / PlayerPowerUps.GetComponentInChildren<ThornsPU>().MaxTime;
        }
        // update explosive mine count ui
        if (ExplosiveMineCount.activeSelf) {
            ExplosiveMineCount.GetComponent<Text>().text = PlayerPowerUps.GetComponentInChildren<ExplosiveMinePU>().GetRemainingMines().ToString();
        }
        // update deactivator mine count ui
        if (DeactivatorMineCount.activeSelf) {
            DeactivatorMineCount.GetComponent<Text>().text = PlayerPowerUps.GetComponentInChildren<DeactivatorMinePU>().GetRemainingMines().ToString();
        }
        // update deactivator pillar count ui
        if (PillarCount.activeSelf) {
            PillarCount.GetComponent<Text>().text = PlayerPowerUps.GetComponentInChildren<PillarPU>().GetRemainingPillars().ToString();
        }
        // updates smoke
        if (SmokeHUD.activeSelf) {
            SmokeHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = PlayerPowerUps.GetComponentInChildren<SmokePU>().GetSmokeAmount() / PlayerPowerUps.GetComponentInChildren<SmokePU>().MaxSmokeAmount;
        }
        // updates nitro
        if (NitroHUD.activeSelf) {
            NitroHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = PlayerPowerUps.GetComponentInChildren<NitroPU>().GetNitroAmount() / PlayerPowerUps.GetComponentInChildren<NitroPU>().MaxNitroAmount;
        }
        // updates grease
        if (GreaseHUD.activeSelf) {
            GreaseHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = PlayerPowerUps.GetComponentInChildren<GreasePU>().GetGreaseAmount() / PlayerPowerUps.GetComponentInChildren<GreasePU>().MaxGreaseAmount;
        }
        // updates glue
        if (GlueHUD.activeSelf) {
            GlueHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = PlayerPowerUps.GetComponentInChildren<GluePU>().GetGlueAmount() / PlayerPowerUps.GetComponentInChildren<GluePU>().MaxGlueAmount;
        }
    }
}
