using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

[Serializable]
public class Carro
{
    public GameObject car;
    public GameObject carBody;
    public Texture[] materials;
}

public class SelectCar : MonoBehaviour
{
    public Carro[] cars;
    public string gameCene;
    public string menuCene;
    
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text progressText;

    [SerializeField] private int selectedId = 0;
    [SerializeField] private int selectedIdColor = 0;

    [SerializeField] private Button selectButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button blackButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button redButton;
    [SerializeField] private Button yellowButton;

    private MeshRenderer m_Renderer;
    private Camera mainCamera;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("selectedId") != 0)
        {
            selectedId = PlayerPrefs.GetInt("selectedId");
        }

        if (PlayerPrefs.GetInt("selectedIdColor") != 0)
        {
            selectedIdColor = PlayerPrefs.GetInt("selectedIdColor");
        }

    }

    private void Start()
    {
        mainCamera = Camera.main;
        ShowCar();
        SetCarColor();

        selectButton.onClick = new Button.ButtonClickedEvent();
        backButton.onClick = new Button.ButtonClickedEvent();
        leftButton.onClick = new Button.ButtonClickedEvent();
        rightButton.onClick = new Button.ButtonClickedEvent();
        blackButton.onClick = new Button.ButtonClickedEvent();
        blueButton.onClick = new Button.ButtonClickedEvent();
        redButton.onClick = new Button.ButtonClickedEvent();
        yellowButton.onClick = new Button.ButtonClickedEvent();

        selectButton.onClick.AddListener(() => LoadGame());
        backButton.onClick.AddListener(() => Back());
        leftButton.onClick.AddListener(() => LeftCar());
        rightButton.onClick.AddListener(() => RightCar());
        blackButton.onClick.AddListener(() => SetCarColorID(2));
        blueButton.onClick.AddListener(() => SetCarColorID(1));
        redButton.onClick.AddListener(() => SetCarColorID(3));
        yellowButton.onClick.AddListener(() => SetCarColorID(0));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftCar();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightCar();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpColor();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DownColor();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadGame();
        }
    }

    public void LoadGame()
    {
        PlayerPrefs.SetInt("selectedIdColor", selectedIdColor);//grava no salve
        PlayerPrefs.SetInt("selectedId", selectedId);//grava no salve
        StartCoroutine(LoadSceneAsync(gameCene));
    }

    public void Back()
    {
        StartCoroutine(LoadSceneAsync(menuCene));
    }
    
    IEnumerator LoadSceneAsync(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        loadingScreen.SetActive(true);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = $"{Mathf.RoundToInt(progress * 100).ToString()}%";
            yield return null;
        }
    }

    private void UpColor()
    {
        if (selectedIdColor >= (cars[selectedId].materials.Length - 1))
        {
            selectedIdColor = 0;//zera o contador
        }
        else
        {
            selectedIdColor++;
        }
        PlayerPrefs.SetInt("selectedIdColor", selectedIdColor);//grava no salve
        SetCarColor();
    }

    private void DownColor()
    {

        if (selectedIdColor <= 0)
        {
            selectedIdColor = (cars[selectedId].materials.Length - 1);//coloca o valor máximo do vetor
        }
        else
        {
            selectedIdColor--;
        }
        PlayerPrefs.SetInt("selectedIdColor", selectedIdColor);//grava no salve
        SetCarColor();
    }

    private void SetCarColorID(int idColor)
    {
        if (idColor >= 0 && idColor <= (cars[selectedId].materials.Length - 1))
        {
            selectedIdColor = idColor;
            PlayerPrefs.SetInt("selectedIdColor", idColor);//grava no salve
            SetCarColor();
        }
    }

    private void SetCarColor()
    {
        m_Renderer = cars[selectedId].carBody.GetComponent<MeshRenderer>();
        for (int i = 0; i < m_Renderer.sharedMaterials.Length; i++)
        {
            m_Renderer.sharedMaterials[i].mainTexture = cars[selectedId].materials[selectedIdColor];
        }
    }

    private void LeftCar()
    {
        if (selectedId <= 0)
        {
            selectedId = (cars.Length - 1);//coloca o valor máximo do vetor
        }
        else
        {
            selectedId--;
        }
        PlayerPrefs.SetInt("selectedId", selectedId);//grava no salve
        ShowCar();//carrega
    }

    private void RightCar()
    {
        if (selectedId >= (cars.Length - 1))
        {
            selectedId = 0;//zera o contador
        }
        else
        {
            selectedId++;
        }
        PlayerPrefs.SetInt("selectedId", selectedId);//grava no salve
        ShowCar();//carrega
    }

    private void ShowCar()
    {
        mainCamera.transform.position =
            new Vector3(cars[selectedId].car.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);

        SetCarColor();
    }
}
