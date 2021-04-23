using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{
    public Button BotaoMenu;
    public Button BotaoVoltar;
    public Button BotaoSair;
    public Canvas SubMenu;
    public Canvas Controles;
    // Start is called before the first frame update
    void Start()
    {
        BotaoMenu.onClick = new Button.ButtonClickedEvent();
        BotaoVoltar.onClick = new Button.ButtonClickedEvent();
        BotaoSair.onClick = new Button.ButtonClickedEvent();


        BotaoMenu.onClick.AddListener(() => Menu());
        BotaoVoltar.onClick.AddListener(() => Voltar());
        BotaoSair.onClick.AddListener(() => Sair());
    }
    private void Menu()
    {
        SubMenu.gameObject.SetActive(true);
        BotaoMenu.gameObject.SetActive(false);
       /// Controles.gameObject.SetActive(false);////
    }
    private void Voltar()
    {
        SubMenu.gameObject.SetActive(false);
        BotaoMenu.gameObject.SetActive(true);
       /// Controles.gameObject.SetActive(true);///
    }
    private void Sair()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
    // Update is called once per frame

}
