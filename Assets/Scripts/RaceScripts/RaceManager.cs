using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour {

    public static RaceManager Instance { get; private set; }

    [Serializable]
    public class Racer
    {
        public readonly GameObject RacerGameObject;
        public readonly VehicleData VehicleData;
        public readonly VehicleRaceData VehicleRaceData;
        public readonly BoxCollider RacerCollider;

        public Racer(GameObject racerGameObject, VehicleData vehicleData, VehicleRaceData vehicleRaceData, BoxCollider racerCollider)
        {
            RacerGameObject = racerGameObject;
            VehicleData = vehicleData;
            VehicleRaceData = vehicleRaceData;
            RacerCollider = racerCollider;
        }
    }

    public float NumberOfLaps;
    public GameObject Player;
    public GameObject RacePath;
    public RunnerAttributeList RunnerAttributesList;
    public List<Racer> RacersList;
    public List<Transform> InitialRacerPositions;
    // HUDs
    public GameObject GameHUD;
    public GameObject RaceResults;
    public GameObject DeathScreen;

    public CarColor CorDoCarro;
    public CarName NomeDoCarro;
    
    private CarUserControl playerCarUserControl;
    private VehicleData playerVehicleData;

    private CarUserControl[] carUserControls;
    private VehicleRaceData[] vehicleRaceDatas;
    private GreasePoolCollision[] greasePoolCollisions;
    private GluePoolCollision[] gluePoolCollisions;

    /// <summary>
    /// Instantiate a random car with a random non repeating color and Runner Name.
    /// </summary>
    private GameObject GenerateRandomCar(Transform spawnPoint, int maxCarPerType, int[] carsGenerated, List<int> namesDrawn, List<string> materialsDrawn) {
        // select a random car prefab that doesn't repeat more than maxCarPerType times
        int drawnCarNumber = UnityEngine.Random.Range(0, RunnerAttributesList.CarList.Length); 

        while (carsGenerated[drawnCarNumber] == maxCarPerType) {
            drawnCarNumber = UnityEngine.Random.Range(0, RunnerAttributesList.CarList.Length);
        }

        carsGenerated[drawnCarNumber]++;

        Car selectedCar = (Car)RunnerAttributesList.CarList[drawnCarNumber];

        // select a random non repeating material of the selected car
        Material[] carMaterialsList = selectedCar.GetCarMaterialsPlayer(false);

        int drawnCarMatNumber = UnityEngine.Random.Range(0, carMaterialsList.Length);

        while (materialsDrawn.Contains(carMaterialsList[drawnCarMatNumber].name)) {
            drawnCarMatNumber = UnityEngine.Random.Range(0, carMaterialsList.Length);
        }

        materialsDrawn.Add(carMaterialsList[drawnCarMatNumber].name);

        // select a random runner name
        int drawnNameNumber = UnityEngine.Random.Range(0, RunnerAttributesList.RunnerNameList.Length);

        while (namesDrawn.Contains(drawnNameNumber)) {
            drawnNameNumber = UnityEngine.Random.Range(0, RunnerAttributesList.RunnerNameList.Length);
        }

        namesDrawn.Add(drawnNameNumber);

        GameObject carDrawn = selectedCar.GetCarPrefabPlayer(false);
        carDrawn.GetComponent<CarMovementAI>().path = RacePath.transform;

        GameObject carPrefab = Instantiate(carDrawn, spawnPoint.position, spawnPoint.rotation);

        // configure instantiated car
        foreach (Transform child in carPrefab.transform.GetChild(0).transform) {
            if (child.gameObject.CompareTag("Chassi")) {
                child.gameObject.GetComponent<Renderer>().material = carMaterialsList[drawnCarMatNumber];
            }
        }

        carPrefab.GetComponent<VehicleData>().RunnerName = RunnerAttributesList.RunnerNameList[drawnNameNumber];
        carPrefab.GetComponent<VehicleRaceData>().TrackerNode = RacePath.transform.GetChild(RacePath.transform.childCount - 1).gameObject.GetComponent<TrackerNode>();
        carPrefab.GetComponent<VehicleRaceData>().TriggerPoint = gameObject.transform.GetChild(gameObject.transform.childCount - 1).gameObject.GetComponentInChildren<TriggerPoint>();

        return carPrefab;
    }
    /// <summary>
    /// Spawn a number of AI cars based on how many starting positions there are.
    /// </summary>
    /// <param name="startingPositions"></param>
    public void SpawnAI(List<Transform> startingPoints) {
        // limit the maximum amount of cars per name
        int MaxCarPerType = ((InitialRacerPositions.Count - 1) / RunnerAttributesList.CarList.Length);
        if ((InitialRacerPositions.Count - 1) % RunnerAttributesList.CarList.Length != 0) {
            MaxCarPerType++;
        }
        // the quantity of cars drawn for each type
        int[] carsDrawn = new int[RunnerAttributesList.CarList.Length];
        for (int i = 0; i < RunnerAttributesList.CarList.Length; i++) {
            carsDrawn[i] = 0;
        }
        // list of materials drawn in game
        List<string> materialsDrawn = new List<string>();
        // the value of a name stored in RunnerAttributesList.RunnerNameList[i] is given by i
        List<int> namesDrawn = new List<int>();
        
        for (int i = 0; i < InitialRacerPositions.Count - 1; i++)
        {
            GameObject car = GenerateRandomCar(startingPoints[i].transform, MaxCarPerType, carsDrawn, namesDrawn,
                materialsDrawn);
            RacersList.Add(new Racer(car, car.GetComponent<VehicleData>(),
                car.GetComponent<VehicleRaceData>(), car.GetComponent<BoxCollider>()));
        }
    }
    /// <summary>
    /// Spawn a car in the intended starting point with carName and carColor for the player to control.
    /// </summary>
    /// <param name="startingPoint">Initial Position of car</param>
    /// <param name="playerCar">Prefab of car</param>
    /// <param name="playerCarMat">Material of car</param>
    public void SpawnPlayer(Transform startingPoint, CarName carName, CarColor carColor, string playerName) {
        // deactivate other cars
        for (int i = 0; i < startingPoint.childCount; i++) {
            if ((int)carName != i) {
                startingPoint.GetChild(i).gameObject.SetActive(false);
            }
        }
        // get reference to car
        GameObject chosenCar = startingPoint.GetChild((int)carName).gameObject;
        // change car color
        Car chosenCarData = (Car)RunnerAttributesList.CarList[(int)carName];
        
        foreach (Transform child in chosenCar.transform.GetChild(0).transform) {
            if (child.gameObject.CompareTag("Chassi")) {
                child.gameObject.GetComponent<Renderer>().material = chosenCarData.GetCarMaterialsPlayer(true)[(int)carColor];
            }
        }
        // change car name
        chosenCar.GetComponent<VehicleData>().RunnerName = playerName;

        Player = chosenCar;

        RacersList.Add(new Racer(Player, Player.GetComponent<VehicleData>(), 
            Player.GetComponent<VehicleRaceData>(), Player.GetComponent<BoxCollider>()));
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        //Load garage
        NomeDoCarro = (CarName)PlayerPrefs.GetInt("selectedId");
        CorDoCarro = (CarColor)PlayerPrefs.GetInt("selectedIdColor");
        // spawn car player
        SpawnPlayer(InitialRacerPositions[InitialRacerPositions.Count - 1], NomeDoCarro, CorDoCarro, "Nina");
        // spawn ai
        SpawnAI(InitialRacerPositions);
        //Racers.Add(Player);

        playerCarUserControl = Player.GetComponent<CarUserControl>();
        playerVehicleData = Player.GetComponent<VehicleData>();
        carUserControls = new CarUserControl[RacersList.Count];
        vehicleRaceDatas = new VehicleRaceData[RacersList.Count];
        greasePoolCollisions = new GreasePoolCollision[RacersList.Count];
        gluePoolCollisions = new GluePoolCollision[RacersList.Count];
        
        for (int i = 0; i < RacersList.Count; i++) {
            carUserControls[i] = RacersList[i].RacerGameObject.GetComponent<CarUserControl>();
            vehicleRaceDatas[i] = RacersList[i].VehicleRaceData;
            greasePoolCollisions[i] = RacersList[i].RacerGameObject.GetComponentInChildren<GreasePoolCollision>();
            gluePoolCollisions[i] = RacersList[i].RacerGameObject.GetComponentInChildren<GluePoolCollision>();
        }
    }

    public void StartRace() {
        for (int i = 0; i < RacersList.Count; i++) {
            carUserControls[i].ControlActive = true; // active control for the racer 'i'
            vehicleRaceDatas[i].ActiveTimer(true); // start timer of race data of vehicle
            greasePoolCollisions[i].ActivateTrigger();
            gluePoolCollisions[i].ActivateTrigger();
        }

    }
    /// <summary>
    /// Called when the player finishes the race. This function sets an AI to control the player's car
    /// and show a table with all the racers and their information: Name, Car Name, Time and Kills.
    /// </summary>
    public void FinishRace() {
        // sets AI on player car
        playerCarUserControl.SetAIControl(true);
        Player.transform.GetChild(4).gameObject.SetActive(true);

        // show Final results
        GameHUD.gameObject.SetActive(false);
        RaceResults.gameObject.SetActive(true);
    }
    private void UpdateRaceResultsTable() {
        GameObject RunnersList = RaceResults.transform.GetChild(0).gameObject;
        for (int i = 0; i < RacersList.Count; i++) {
            int position = i + 1;
            VehicleData VehicleInfo = RacersList[i].VehicleData;
            VehicleRaceData VehicleRaceInfo = RacersList[i].VehicleRaceData;
            if (VehicleRaceInfo.LapTime == null) {
                VehicleRaceInfo.LapTime = gameObject.AddComponent<Timer>();
            }
            if (VehicleRaceInfo.RaceTime == null) {
                VehicleRaceInfo.RaceTime = gameObject.AddComponent<Timer>();
            }
            GameObject RunnerRow = RunnersList.transform.GetChild(position).gameObject;
            // position
            if (position < 10) {
                RunnerRow.transform.GetChild(0).gameObject.GetComponent<Text>().text = "0" + position;
            } else {
                RunnerRow.transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + position;
            }
            // runner name
            RunnerRow.transform.GetChild(1).gameObject.GetComponent<Text>().text = VehicleInfo.RunnerName;
            // car name
            RunnerRow.transform.GetChild(2).gameObject.GetComponent<Text>().text = VehicleInfo.CarName;
            // kills
            RunnerRow.transform.GetChild(3).gameObject.GetComponent<Text>().text = VehicleInfo.KillsCount.ToString();
            // time
            if (!VehicleInfo.IsDead) {
                string MinCount;
                string SecCount;
                string MilliCount;

                RunnerRow.GetComponent<Image>().color = new Color32(0, 0, 0, 100);
                if (VehicleRaceInfo.HasCompletedRace()) {
                    if (RacersList[i].RacerGameObject.CompareTag("Player")) {
                        RunnerRow.GetComponent<Image>().color = new Color32(0, 0, 200, 100);
                    }
                    if (VehicleRaceInfo.GetMinCountTotal() <= 9) {
                        MinCount = "0" + VehicleRaceInfo.GetMinCountTotal();
                    } else {
                        MinCount = VehicleRaceInfo.GetMinCountTotal().ToString();
                    }
                
                    if (VehicleRaceInfo.GetSecCountTotal() <= 9) {
                        SecCount = "0" + VehicleRaceInfo.GetSecCountTotal();
                    } else {
                        SecCount = VehicleRaceInfo.GetSecCountTotal().ToString();
                    }
                
                    if (VehicleRaceInfo.GetMilliCountTotal() < 10) {
                        MilliCount = "00" + VehicleRaceInfo.GetMilliCountTotal().ToString("F0");
                    } else if (VehicleRaceInfo.GetMilliCountTotal() < 100 && VehicleRaceInfo.GetMilliCountTotal() >= 10) {
                        MilliCount = "0" + VehicleRaceInfo.GetMilliCountTotal().ToString("F0");
                    } else {
                        MilliCount = "" + VehicleRaceInfo.GetMilliCountTotal().ToString("F0");
                    }
                } else {
                    if (VehicleRaceInfo.RaceTime.GetMinutes() <= 9) {
                        MinCount = "0" + VehicleRaceInfo.RaceTime.GetMinutes();
                    } else {
                        MinCount = VehicleRaceInfo.RaceTime.GetMinutes().ToString();
                    }

                    if (VehicleRaceInfo.RaceTime.GetSeconds() <= 9) {
                        SecCount = "0" + VehicleRaceInfo.RaceTime.GetSeconds();
                    } else {
                        SecCount = VehicleRaceInfo.RaceTime.GetSeconds().ToString();
                    }

                    if (VehicleRaceInfo.RaceTime.GetMilliseconds() < 10) {
                        MilliCount = "00" + VehicleRaceInfo.RaceTime.GetMilliseconds().ToString("F0");
                    } else if (VehicleRaceInfo.RaceTime.GetMilliseconds() < 100 && VehicleRaceInfo.RaceTime.GetMilliseconds() >= 10) {
                        MilliCount = "0" + VehicleRaceInfo.RaceTime.GetMilliseconds().ToString("F0");
                    } else {
                        MilliCount = "" + VehicleRaceInfo.RaceTime.GetMilliseconds().ToString("F0");
                    }
                }
                RunnerRow.transform.GetChild(4).gameObject.GetComponent<Text>().text = "" + MinCount + ":" + SecCount + "." + MilliCount;
            } else {
                RunnerRow.GetComponent<Image>().color = new Color32(255,0,0,100);
                RunnerRow.transform.GetChild(4).gameObject.GetComponent<Text>().text = "ELIMINADO";
                //RunnerRow.transform.GetChild(4).gameObject.GetComponent<Text>().color = new Color32(255, 0, 0, 100);
            }
            RunnerRow.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Update Racers positions. In order for this method to function correctly, the initial order of the cars in the Racers list must exactly the same 
    /// as the initial order of its respective object cars in the game world.
    /// </summary>
    private void UpdateRacersPositions() {        
        VehicleRaceData curCar;
        VehicleRaceData followCar;

        for (int i = 0; i < RacersList.Count; i++) {
            curCar = vehicleRaceDatas[i];
            for (int j = i + 1; j < RacersList.Count; j++) {
                followCar = vehicleRaceDatas[j];
                if (!curCar.HasCompletedRace()) {
                    // if last tracker node of car i and last tracker node of car j are the same
                    if (followCar.TrackerNode == curCar.TrackerNode) {
                        // if car j passed car i, change their race positions and resort list of racers
                        if (followCar.TrackerNode.GetDistance(RacersList[j].RacerGameObject) > curCar.TrackerNode.GetDistance(RacersList[i].RacerGameObject) 
                            && followCar.GetRacePosition() > curCar.GetRacePosition() 
                            && followCar.GetLapCount() == curCar.GetLapCount()) {
                            float aux = curCar.GetRacePosition();
                            curCar.SetRacePosition(followCar.GetRacePosition());
                            followCar.SetRacePosition(aux);
                            // sort list of racers based on race position and whether its alive
                            RacersList.Sort(delegate (Racer car1, Racer car2)
                            {
                                VehicleData car1VehicleData = car1.VehicleData;
                                VehicleData car2VehicleData = car2.VehicleData;
                                VehicleRaceData car1VehicleRaceData = car1.VehicleRaceData;
                                VehicleRaceData car2VehicleRaceData = car2.VehicleRaceData;
                                
                                if (car1VehicleData.isDead && car2VehicleData.isDead) {
                                    return 0;
                                } if (car2VehicleData.isDead) {
                                    return -1;
                                } if (car1VehicleData.isDead) {
                                    return 1;
                                } if (car1VehicleRaceData.GetRacePosition() > car2VehicleRaceData.GetRacePosition()) {
                                    return 1;
                                }
                                return -1;
                            });
                        }
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < RacersList.Count; i++) {
            RacersList[i].VehicleRaceData.SetRacePosition(RacersList.Count - i);
        }
        RacersList.Sort(delegate (Racer car1, Racer car2)
        {
            if (car1.VehicleRaceData.GetRacePosition() > car2.VehicleRaceData.GetRacePosition()) {
                return 1;
            }

            return -1;
        });
    }

    // Update is called once per frame
    void Update() {
        if (playerVehicleData.isDead) {
            if (GameHUD.activeSelf) {
                GameHUD.SetActive(false);
            }
            if (!DeathScreen.activeSelf && playerVehicleData.DeadTime >= 2f) {
                DeathScreen.SetActive(true);
            }
        } else {
            UpdateRacersPositions();
            for (int i = 0; i < RacersList.Count; i++) {
                // if a car completes the last lap and hasnt completed the race yet
                if (!vehicleRaceDatas[i].HasCompletedRace() && vehicleRaceDatas[i].GetLapCount() == NumberOfLaps) {
                    vehicleRaceDatas[i].CompleteRace();
                    if (RacersList[i].RacerGameObject.CompareTag("Player")) {
                        FinishRace();
                    }
                }
            }
            UpdateRaceResultsTable();
        }
    }
}
