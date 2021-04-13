using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuFase : MonoBehaviour
{
    public int numeroDaFase;
    public string nomeDaCena;

    private bool faseBloqueada = true;
    public Sprite[] cadeado;
    private int faseAtual = 1;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("fase1"))
        {
            PlayerPrefs.SetInt("fase1", 1);
            PlayerPrefs.SetInt("faseEStrela1", 0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Atualizar();
        this.gameObject.transform.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        this.gameObject.transform.GetComponent<Button>().onClick.AddListener(() => Jogar(nomeDaCena));
    }
    private void Jogar(string nome)
    {
        SceneManager.LoadScene(nome, LoadSceneMode.Single);
    }

    // Update is called once per frame
    private void Atualizar()
    {
        if (PlayerPrefs.HasKey("fase" + numeroDaFase))//fase foi liberada
        {
            if (PlayerPrefs.GetInt("faseEStrela" + numeroDaFase) == 0)// fase não foi passada
            {
                this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = cadeado[0];//mostra desenho
            }
            else if (PlayerPrefs.GetInt("faseEStrela" + numeroDaFase) == 1)//uma estrela
            {
                this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = cadeado[1];//mostra desenho
            }
            else if (PlayerPrefs.GetInt("faseEStrela" + numeroDaFase) == 2)//duas estrela
            {
                this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = cadeado[2];//mostra desenho
            }
            else if (PlayerPrefs.GetInt("faseEStrela" + numeroDaFase) == 3)// tres estrela
            {
                this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = cadeado[3];//mostra desenho
            }
        }
        else if (!PlayerPrefs.HasKey("fase" + numeroDaFase))// não foi liberada
        {
            this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = cadeado[4];//mostra desenho
            this.gameObject.transform.GetComponent<Button>().interactable = false;// deixa o botão sem poder clicar
        }
    }
}
