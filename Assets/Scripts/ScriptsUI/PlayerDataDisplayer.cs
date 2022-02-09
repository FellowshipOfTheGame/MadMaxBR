using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataDisplayer : MonoBehaviour {
    public static PlayerDataDisplayer Instance;

    public GameObject VelocityDisplay;
    public GameObject LapCounterDisplay;
    public GameObject RacePositionDisplay;

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
    public GameObject ShieldHUD;

    public GameObject PowerUpSlot1;
    public GameObject PowerUpSlot2;
    public GameObject PowerUpSlot3;
    public GameObject PowerUpSlot4;

    private TextMeshProUGUI ButtonSlot1;
    private TextMeshProUGUI ButtonSlot2;
    private TextMeshProUGUI ButtonSlot3;
    private TextMeshProUGUI ButtonSlot4;

    public TextMeshProUGUI PowerUpTutorialText;

    private GameObject PlayerPowerUps;
    private VehicleRaceData PlayerRaceData;

    private Queue<int> tutorialTextIndQueue;
    private Queue<string> tutorialTextQueue;
    [SerializeField] private float TutorialTextStayTime;
    /// <summary>
    /// Stores the time a tutorial text from an especific powerup stayed on screen. 
    /// </summary>
    private float[] tutorialTextStayedTime;
    private void Awake() {
        Instance = this;
    }

    private void UpdateTutorialTextQueue(PowerUpData PowerUpData) {
        if (!tutorialTextIndQueue.Contains((int)PowerUpData.PowerUpName)) {
            if (tutorialTextStayedTime[PowerUpData.PowerUpId] <= TutorialTextStayTime) {
                tutorialTextQueue.Enqueue(PowerUpData.TutorialText);
                tutorialTextIndQueue.Enqueue((int)PowerUpData.PowerUpName);
            }
        } else {
            if (tutorialTextIndQueue.Peek() == (int)PowerUpData.PowerUpName) {
                tutorialTextStayedTime[PowerUpData.PowerUpId] += Time.deltaTime;
                if (tutorialTextStayedTime[PowerUpData.PowerUpId] > TutorialTextStayTime) {
                    tutorialTextQueue.Dequeue();
                    tutorialTextIndQueue.Dequeue();
                }
            }
        }
    }

    private string GetKeySprite(KeyCode keyCode) {
        string keySpriteVal;

        switch((int)keyCode) {
            case (int)KeyCode.Q:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Q\">";
                break;
            case (int)KeyCode.W:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"W\">";
                break;
            case (int)KeyCode.E:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"E\">";
                break;
            case (int)KeyCode.R:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"R\">";
                break;
            case (int)KeyCode.T:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"T\">";
                break;
            case (int)KeyCode.Y:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Y\">";
                break;
            case (int)KeyCode.U:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"U\">";
                break;
            case (int)KeyCode.I:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"I\">";
                break;
            case (int)KeyCode.O:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"O\">";
                break;
            case (int)KeyCode.P:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"P\">";
                break;
            case (int)KeyCode.A:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"A\">";
                break;
            case (int)KeyCode.S:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"S\">";
                break;
            case (int)KeyCode.D:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"D\">";
                break;
            case (int)KeyCode.F:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"F\">";
                break;
            case (int)KeyCode.G:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"G\">";
                break;
            case (int)KeyCode.H:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"H\">";
                break;
            case (int)KeyCode.J:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"J\">";
                break;
            case (int)KeyCode.K:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"K\">";
                break;
            case (int)KeyCode.L:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"L\">";
                break;
            case (int)KeyCode.Z:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Z\">";
                break;
            case (int)KeyCode.X:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"X\">";
                break;
            case (int)KeyCode.C:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"C\">";
                break;
            case (int)KeyCode.V:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"V\">";
                break;
            case (int)KeyCode.B:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"B\">";
                break;
            case (int)KeyCode.N:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"N\">";
                break;
            case (int)KeyCode.M:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"M\">";
                break;
            case (int)KeyCode.LeftAlt:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alt\">";
                break;
            case (int)KeyCode.Alpha1:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha1\">";
                break;
            case (int)KeyCode.Alpha2:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha2\">";
                break;
            case (int)KeyCode.Alpha3:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha3\">";
                break;
            case (int)KeyCode.Alpha4:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha4\">";
                break;
            case (int)KeyCode.Alpha5:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha5\">";
                break;
            case (int)KeyCode.Alpha6:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha6\">";
                break;
            case (int)KeyCode.Alpha7:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha7\">";
                break;
            case (int)KeyCode.Alpha8:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha8\">";
                break;
            case (int)KeyCode.Alpha9:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha9\">";
                break;
            case (int)KeyCode.Alpha0:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha0\">";
                break;
            case (int)KeyCode.Keypad1:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha1\">";
                break;
            case (int)KeyCode.Keypad2:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha2\">";
                break;
            case (int)KeyCode.Keypad3:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha3\">";
                break;
            case (int)KeyCode.Keypad4:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha4\">";
                break;
            case (int)KeyCode.Keypad5:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha5\">";
                break;
            case (int)KeyCode.Keypad6:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha6\">";
                break;
            case (int)KeyCode.Keypad7:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha7\">";
                break;
            case (int)KeyCode.Keypad8:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha8\">";
                break;
            case (int)KeyCode.Keypad9:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha9\">";
                break;
            case (int)KeyCode.Keypad0:
                keySpriteVal = "<sprite=\"KeyboardBlack\" name=\"Alpha0\">";
                break;
            default:
                keySpriteVal = "";
                break;
        }

        return keySpriteVal;
    }

    private void UpdatePowerUpIcon(Image imgComponent, int powerupIndex, TextMeshProUGUI powerupButtonImg) {
        imgComponent.color = new Color(255, 255, 255, 255);
        switch (powerupIndex) {
            case (int)PowerUpName.MachineGun:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<MachineGunPU>().PowerUpInfo.Icon;
                powerupButtonImg.text = GetKeySprite(RaceManager.Instance.Player.GetComponent<CarUserControl>().PowerUpUseButtons.Slot1UseButton);
                powerupButtonImg.gameObject.SetActive(true);
                break;
            case (int)PowerUpName.Rifle:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<RiflePU>().PowerUpInfo.Icon;
                powerupButtonImg.text = GetKeySprite(RaceManager.Instance.Player.GetComponent<CarUserControl>().PowerUpUseButtons.Slot1UseButton);
                powerupButtonImg.gameObject.SetActive(true);
                break;
            case (int)PowerUpName.Thorns:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<ThornsPU>().PowerUpInfo.Icon;
                break;
            case (int)PowerUpName.Shield:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<ShieldPU>().PowerUpInfo.Icon;
                break;
            case (int)PowerUpName.Fix:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<FixPU>().PowerUpInfo.Icon;
                break;
            case (int)PowerUpName.Smoke:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<SmokePU>().PowerUpInfo.Icon;
                powerupButtonImg.text = GetKeySprite(RaceManager.Instance.Player.GetComponent<CarUserControl>().PowerUpUseButtons.Slot2UseButton);
                powerupButtonImg.gameObject.SetActive(true);
                break;
            case (int)PowerUpName.ExplosiveMine:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<ExplosiveMinePU>().PowerUpInfo.Icon;
                powerupButtonImg.text = GetKeySprite(RaceManager.Instance.Player.GetComponent<CarUserControl>().PowerUpUseButtons.Slot3UseButton);
                powerupButtonImg.gameObject.SetActive(true);
                break;
            case (int)PowerUpName.DeactivatorMine:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<DeactivatorMinePU>().PowerUpInfo.Icon;
                powerupButtonImg.text = GetKeySprite(RaceManager.Instance.Player.GetComponent<CarUserControl>().PowerUpUseButtons.Slot3UseButton);
                powerupButtonImg.gameObject.SetActive(true);
                break;
            case (int)PowerUpName.Pillar:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<PillarPU>().PowerUpInfo.Icon;
                powerupButtonImg.text = GetKeySprite(RaceManager.Instance.Player.GetComponent<CarUserControl>().PowerUpUseButtons.Slot3UseButton);
                powerupButtonImg.gameObject.SetActive(true);
                break;
            case (int)PowerUpName.Nitro:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<NitroPU>().PowerUpInfo.Icon;
                powerupButtonImg.text = GetKeySprite(RaceManager.Instance.Player.GetComponent<CarUserControl>().PowerUpUseButtons.Slot4UseButton);
                powerupButtonImg.gameObject.SetActive(true);
                break;
            case (int)PowerUpName.Glue:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<GluePU>().PowerUpInfo.Icon;
                powerupButtonImg.text = GetKeySprite(RaceManager.Instance.Player.GetComponent<CarUserControl>().PowerUpUseButtons.Slot4UseButton);
                powerupButtonImg.gameObject.SetActive(true);
                break;
            case (int)PowerUpName.Grease:
                imgComponent.sprite = PlayerPowerUps.GetComponentInChildren<GreasePU>().PowerUpInfo.Icon;
                powerupButtonImg.text = GetKeySprite(RaceManager.Instance.Player.GetComponent<CarUserControl>().PowerUpUseButtons.Slot4UseButton);
                powerupButtonImg.gameObject.SetActive(true);
                break;
            default:
                imgComponent.sprite = null;
                imgComponent.color = new Color(255, 255, 255, 0);
                powerupButtonImg.gameObject.SetActive(false);
                break;
        }
    }

    // Start is called before the first frame update
    void Start() {
        PlayerRaceData = RaceManager.Instance.Player.GetComponent<VehicleRaceData>();
        PlayerPowerUps = RaceManager.Instance.Player.transform.GetComponentInChildren<PowerUp>().gameObject;

        ButtonSlot1 = PowerUpSlot1.transform.GetChild(PowerUpSlot1.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
        ButtonSlot2 = PowerUpSlot2.transform.GetChild(PowerUpSlot2.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
        ButtonSlot3 = PowerUpSlot3.transform.GetChild(PowerUpSlot3.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
        ButtonSlot4 = PowerUpSlot4.transform.GetChild(PowerUpSlot4.transform.childCount - 1).GetComponent<TextMeshProUGUI>();

        tutorialTextIndQueue = new Queue<int>();
        tutorialTextQueue = new Queue<string>();
        tutorialTextStayedTime = new float[12];
        for (int i = 0; i < tutorialTextStayedTime.Length; i++) {
            tutorialTextStayedTime[i] = 0;
        }
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
        LapCounterDisplay.GetComponent<Text>().text = "" + PlayerRaceData.GetLapCount() + "/" + RaceManager.Instance.NumberOfLaps;
        RacePositionDisplay.GetComponent<Text>().text = "" + PlayerRaceData.GetRacePosition() + "/" + RaceManager.Instance.Racers.Count;
    }

    private void UpdateUIBars() {
        // updates health and shield bar
        /*
        Transform[] allChildren = PlayerShieldHUD.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren) {
            child.gameObject.GetComponent<Image>().fillAmount = RaceManager.Instance.Player.GetComponent<VehicleData>().GetCurrentShield() / RaceManager.Instance.Player.GetComponent<VehicleData>().MaxCarShield;
        }*/
        PlayerHealthHUD.GetComponent<Image>().fillAmount = RaceManager.Instance.Player.GetComponent<VehicleData>().GetCurrentHealth() / RaceManager.Instance.Player.GetComponent<VehicleData>().MaxCarHealth;
    }

    private void UpdatePowerupSlot() {
        UpdatePowerUpIcon(PowerUpSlot1.GetComponent<Image>(), RaceManager.Instance.Player.GetComponent<VehicleData>().GetPowerUpSlotValue(1), ButtonSlot1);
        UpdatePowerUpIcon(PowerUpSlot2.GetComponent<Image>(), RaceManager.Instance.Player.GetComponent<VehicleData>().GetPowerUpSlotValue(2), ButtonSlot2);
        UpdatePowerUpIcon(PowerUpSlot3.GetComponent<Image>(), RaceManager.Instance.Player.GetComponent<VehicleData>().GetPowerUpSlotValue(3), ButtonSlot3);
        UpdatePowerUpIcon(PowerUpSlot4.GetComponent<Image>(), RaceManager.Instance.Player.GetComponent<VehicleData>().GetPowerUpSlotValue(4), ButtonSlot4);
    }

    private void UpdateTutorialText() {
        for (int i = 1; i < 5; i++) {
            switch (RaceManager.Instance.Player.GetComponent<VehicleData>().GetPowerUpSlotValue(i)) {
                case (int)PowerUpName.MachineGun:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<MachineGunPU>().PowerUpInfo);
                    break;
                case (int)PowerUpName.Rifle:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<RiflePU>().PowerUpInfo);
                    break;
                case (int)PowerUpName.Thorns:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<ThornsPU>().PowerUpInfo);
                    break;
                case (int)PowerUpName.Shield:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<ShieldPU>().PowerUpInfo);
                    break;
                case (int)PowerUpName.Fix:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<FixPU>().PowerUpInfo);
                    break;
                case (int)PowerUpName.Smoke:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<SmokePU>().PowerUpInfo);
                    break;
                case (int)PowerUpName.ExplosiveMine:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<ExplosiveMinePU>().PowerUpInfo);
                    break;
                case (int)PowerUpName.DeactivatorMine:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<DeactivatorMinePU>().PowerUpInfo);
                    break;
                case (int)PowerUpName.Pillar:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<PillarPU>().PowerUpInfo);
                    break;
                case (int)PowerUpName.Nitro:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<NitroPU>().PowerUpInfo);
                    break;
                case (int)PowerUpName.Glue:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<GluePU>().PowerUpInfo);
                    break;
                case (int)PowerUpName.Grease:
                    UpdateTutorialTextQueue(PlayerPowerUps.GetComponentInChildren<GreasePU>().PowerUpInfo);
                    break;
            }
        }

        if (tutorialTextQueue.Count != 0) {
            // show the peek value of queue for a time equals to TutorialTextStayTime in seconds
            PowerUpTutorialText.text = tutorialTextQueue.Peek();
        } else { // show nothing if queue is empty
            PowerUpTutorialText.text = "";
        }
    }

    private void UpdatePowerUpUI() {
        // updates bullet count text with quantity of MachineGun
        if (MachineGunCountText.activeSelf) {
            MachineGunCountText.GetComponent<TextMeshProUGUI>().text = PlayerPowerUps.GetComponentInChildren<MachineGunPU>().BulletAmount.ToString();
        }
        // updates bullet count text with quantity of Rifle
        if (RifleCountText.activeSelf) {
            RifleCountText.GetComponent<TextMeshProUGUI>().text = PlayerPowerUps.GetComponentInChildren<RiflePU>().BulletAmount.ToString();
        }
        // update thorns timer ui
        if (ThornsTimerUI.activeSelf) {
            ThornsTimerUI.GetComponent<Image>().fillAmount = 1 - PlayerPowerUps.GetComponentInChildren<ThornsPU>().GetRunningTime() / PlayerPowerUps.GetComponentInChildren<ThornsPU>().MaxTime;
        }
        // updates shield
        if (ShieldHUD.activeSelf) {
            //SmokeHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = PlayerPowerUps.GetComponentInChildren<SmokePU>().CurSmokeAmount / PlayerPowerUps.GetComponentInChildren<SmokePU>().MaxSmokeAmount;
            ShieldHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = RaceManager.Instance.Player.GetComponent<VehicleData>().GetCurrentShield() / RaceManager.Instance.Player.GetComponent<VehicleData>().MaxCarShield;
        }
        // update explosive mine count ui
        if (ExplosiveMineCount.activeSelf) {
            ExplosiveMineCount.GetComponent<TextMeshProUGUI>().text = PlayerPowerUps.GetComponentInChildren<ExplosiveMinePU>().RemainingMines.ToString();
        }
        // update deactivator mine count ui
        if (DeactivatorMineCount.activeSelf) {
            DeactivatorMineCount.GetComponent<TextMeshProUGUI>().text = PlayerPowerUps.GetComponentInChildren<DeactivatorMinePU>().RemainingMines.ToString();
        }
        // update deactivator pillar count ui
        if (PillarCount.activeSelf) {
            PillarCount.GetComponent<TextMeshProUGUI>().text = PlayerPowerUps.GetComponentInChildren<PillarPU>().RemainingPillars.ToString();
        }
        // updates smoke
        if (SmokeHUD.activeSelf) {
            SmokeHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = PlayerPowerUps.GetComponentInChildren<SmokePU>().CurSmokeAmount / PlayerPowerUps.GetComponentInChildren<SmokePU>().MaxSmokeAmount;
        }
        // updates nitro
        if (NitroHUD.activeSelf) {
            NitroHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = PlayerPowerUps.GetComponentInChildren<NitroPU>().CurNitroAmount / PlayerPowerUps.GetComponentInChildren<NitroPU>().MaxNitroAmount;
        }
        // updates grease
        if (GreaseHUD.activeSelf) {
            GreaseHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = PlayerPowerUps.GetComponentInChildren<GreasePU>().CurGreaseAmount / PlayerPowerUps.GetComponentInChildren<GreasePU>().MaxGreaseAmount;
        }
        // updates glue
        if (GlueHUD.activeSelf) {
            GlueHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = PlayerPowerUps.GetComponentInChildren<GluePU>().CurGlueAmount / PlayerPowerUps.GetComponentInChildren<GluePU>().MaxGlueAmount;
        }
    }
}
