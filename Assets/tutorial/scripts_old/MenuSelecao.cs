using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuSelecao : MonoBehaviour
{
    public Button BotaoJogar;
    public Button BotaoOpcoes;
    public Button BotaoSair;
    public Button BotaoEsquerda;
    public Button BotaoDireita;
    public Button BotaoVoltar;
    public Button BotaoAvancar;
    ///public Button
    public Text Label;
    public GameObject[] plane;
    //public GameObject plane0;
    //public GameObject plane1;
    public int modelo;
    // public int tamanho;
    private void Awake()
    {
        modelo = PlayerPrefs.GetInt("modelo");
        //MostraAviao();

    }

    private void Start()
    {
        // =========SETAR BOTOES==========//
        BotaoJogar.onClick = new Button.ButtonClickedEvent();
        BotaoOpcoes.onClick = new Button.ButtonClickedEvent();
        BotaoSair.onClick = new Button.ButtonClickedEvent();
        //
        BotaoVoltar.onClick = new Button.ButtonClickedEvent();
        BotaoEsquerda.onClick = new Button.ButtonClickedEvent();
        BotaoDireita.onClick = new Button.ButtonClickedEvent();
        BotaoAvancar.onClick = new Button.ButtonClickedEvent();


        BotaoJogar.onClick.AddListener(() => Jogar());
        BotaoOpcoes.onClick.AddListener(() => Opcoes());
        BotaoSair.onClick.AddListener(() => Sair());
        //
        BotaoVoltar.onClick.AddListener(() => Voltar());
        BotaoEsquerda.onClick.AddListener(() => Esquerda());
        BotaoDireita.onClick.AddListener(() => Direita());
        BotaoAvancar.onClick.AddListener(() => Avancar());

    }
    private void Jogar()
    {
        BotaoJogar.gameObject.SetActive(false);
        BotaoOpcoes.gameObject.SetActive(false);
        BotaoSair.gameObject.SetActive(false);
        Label.text="ESCOLHA SEU AVIAO";
        //
        BotaoVoltar.gameObject.SetActive(true);
        BotaoEsquerda.gameObject.SetActive(true);
        BotaoDireita.gameObject.SetActive(true);
        BotaoAvancar.gameObject.SetActive(true);
        plane[modelo].gameObject.SetActive(true);
        MostraAviao();
    }
    private void Opcoes()
    {
        BotaoJogar.gameObject.SetActive(false);
        BotaoOpcoes.gameObject.SetActive(false);
        BotaoSair.gameObject.SetActive(false);
        Label.text="OPCAO";
        //
        BotaoVoltar.gameObject.SetActive(true);
        BotaoEsquerda.gameObject.SetActive(false);
        BotaoDireita.gameObject.SetActive(false);
        BotaoAvancar.gameObject.SetActive(false);
        plane[modelo].gameObject.SetActive(false);
    }
    private void Voltar()
    {
        BotaoJogar.gameObject.SetActive(true);
        BotaoOpcoes.gameObject.SetActive(true);
        BotaoSair.gameObject.SetActive(true);
        Label.text="";
        //
        BotaoVoltar.gameObject.SetActive(false);
        BotaoEsquerda.gameObject.SetActive(false);
        BotaoDireita.gameObject.SetActive(false);
        BotaoAvancar.gameObject.SetActive(false);
        plane[modelo].gameObject.SetActive(false);
        transform.position = new Vector3(0, 0, -20);

    }
    private void Avancar()
    {
        SceneManager.LoadScene("Seletor", LoadSceneMode.Single);
    }
    private void Sair()
    {
        Application.Quit();
    }
    private void Esquerda()
    {
        if (modelo <= 0)
        {
            modelo = (plane.Length - 1);//coloca o valor maximo do vetor
        }
        else
        {
            modelo--;
        }

        PlayerPrefs.SetInt("modelo", modelo);//grava no salve
        MostraAviao();//carrega
    }
    private void Direita()
    {
        if (modelo >= (plane.Length - 1))
        {
            modelo = 0;//zera o contador
        }
        else
        {
            modelo++;
        }
        PlayerPrefs.SetInt("modelo", modelo);//grava no salve
        MostraAviao();//carrega
    }
   /* void Update()
    {

        /*       if (CrossPlatformInputManager.GetButtonDown("primeiro"))///vefirica se a tecla foi apertado e tiver liberado pulo
              {
                  PlayerPrefs.SetInt("modelo", 0);
                  Debug.Log("Primeiro");
              }
              else
              if (CrossPlatformInputManager.GetButtonDown("segundo"))///vefirica se a tecla foi apertado e tiver liberado pulo
              {
                  PlayerPrefs.SetInt("modelo", 1);
                  Debug.Log("Segundo");
              }
              else
              if (CrossPlatformInputManager.GetButtonDown("batalha"))///vefirica se a tecla foi apertado e tiver liberado pulo
              {
                  Debug.Log("Proxima Fase");
                  // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
                  SceneManager.LoadScene("cena2", LoadSceneMode.Single);
              } 
    }
     private void FixedUpdate()
    {

    } */
    private void MostraAviao()
    {
        //for (int i = 0; i < plane.Length; i++)
        // {
        //    Destroy(plane[i]);
        // }
        // while (plane.Length > 0)
        // {
        //     Destroy(plane[0]);
        // }
        // Instantiate(plane[modelo], new Vector3(0, 0, 0), Quaternion.identity);

        // Destroy(plane[modelo]);

        /*  if (modelo == 0)
         {

             plane.transform.position = new Vector3(-2, -2, 0);
             plane1.transform.position = new Vector3(-20, -2, 0);
         }
         else if (modelo == 1)
         {
             plane0.transform.position = new Vector3(-20, -2, 0);
             plane1.transform.position = new Vector3(-2, -2, 0);
         } */
        transform.position = new Vector3(plane[modelo].transform.position.x, plane[modelo].transform.position.y, -20);
    }
}
