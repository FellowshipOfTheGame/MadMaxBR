using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class LaserOuMira
{
    public bool ativarLaser = false;
    public Color corLaser = Color.red;
    public bool AtivarMiraComum = true;
}

[Serializable]
public class Arma919
{
    [HideInInspector]
    public int balasNaArma, balasNoPente;
    public int danoPorTiro = 40;
    [Range(0, 500)]
    public int numeroDeBalas = 240;
    [Range(0, 50)]
    public int balasPorPente = 30;
    [Range(0.01f, 5.0f)]
    public float tempoPorTiro = 0.3f;
    [Range(0.01f, 5.0f)]
    public float tempoDaRecarga = 0.5f;
    [Space(10)]
    public LaserOuMira Miras;
    [Space(10)]
    public GameObject objetoArma;
    public GameObject lugarParticula;
    public GameObject particulaFumaca;
    [Range(0.01f, 2.0f)]
    public float tempoVidaParticula = 0.5f;
    public AudioClip somTiro, somRecarga;
    public WeaponState weaponState = WeaponState.idle;
    public Animator weaponAnimator;
}
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private KeyCode botaoRecarregar = KeyCode.R;
    public int armaInicial = 0;
    [SerializeField] private string TagInimigo = "inimigo";
    [SerializeField] private Text BalasPente, BalasArmaText;
    [SerializeField] private Material MaterialLasers;
    [SerializeField] private Arma919[] armas;
    //
    private int armaAtual;
    private AudioSource emissorSom;
    private bool recarregando, atirando;
    private LineRenderer linhaDoLaser;
    private GameObject luzColisao;

    void Start()
    {
        //laser das armas
        luzColisao = new GameObject();
        luzColisao.AddComponent<Light>();
        luzColisao.GetComponent<Light>().intensity = 8;
        luzColisao.GetComponent<Light>().bounceIntensity = 8;
        luzColisao.GetComponent<Light>().range = 0.2f;
        luzColisao.GetComponent<Light>().color = Color.red;
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = MaterialLasers;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.startWidth = 0.015f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        linhaDoLaser = GetComponent<LineRenderer>();
        //
        for (int x = 0; x < armas.Length; x++)
        {
            armas[x].objetoArma.SetActive(false);
            armas[x].lugarParticula.SetActive(false);
            armas[x].balasNaArma = armas[x].numeroDeBalas - armas[x].balasPorPente;
            armas[x].balasNoPente = armas[x].balasPorPente;
            armas[x].Miras.corLaser.a = 1;
        }
        if (armaInicial > armas.Length - 1)
        {
            armaInicial = armas.Length - 1;
        }
        armas[armaInicial].objetoArma.SetActive(true);
        armas[armaInicial].lugarParticula.SetActive(true);
        armaAtual = armaInicial;
        emissorSom = GetComponent<AudioSource>();
        recarregando = atirando = false;
    }

    void FixedUpdate()
    {
        //UI
        BalasArmaText.text = "Balas: " + armas[armaAtual].balasNaArma;
        BalasPente.text = "Pente: " + armas[armaAtual].balasNoPente;
        if (Input.GetMouseButton(0) && armas[armaAtual].balasNoPente > 0 && !recarregando && !atirando)
        {
            StartCoroutine(TempoTiro(armas[armaAtual].tempoPorTiro));
            emissorSom.clip = armas[armaAtual].somTiro;
            emissorSom.PlayOneShot(emissorSom.clip);
            armas[armaAtual].balasNoPente--;
            armas[armaAtual].weaponAnimator.SetBool("IsShooting", true);
            GameObject balaTemp = Instantiate(armas[armaAtual].particulaFumaca, armas[armaAtual].lugarParticula.transform.position, transform.rotation) as GameObject;
            Destroy(balaTemp, armas[armaAtual].tempoVidaParticula);

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit pontoDeColisao))
            {
                if (pontoDeColisao.transform.gameObject.tag == TagInimigo)
                {
                    pontoDeColisao.transform.gameObject.GetComponent<Inimigo>().vida -= armas[armaAtual].danoPorTiro;
                }
            }
        }
        if (!Input.GetMouseButton(0) && atirando || recarregando || armas[armaAtual].balasNoPente == 0)
        {
            armas[armaAtual].weaponAnimator.SetBool("IsShooting", false);
            armas[armaAtual].weaponAnimator.SetBool("IsLoop", false);
        }
        if (Input.GetMouseButton(0) && atirando)
        {
            armas[armaAtual].weaponAnimator.SetBool("IsLoop", true);
        }
        //recarregar
        if (Input.GetKeyDown(botaoRecarregar) && !recarregando && !atirando && (armas[armaAtual].balasNoPente < armas[armaAtual].balasPorPente) && (armas[armaAtual].balasNaArma > 0))
        {
            emissorSom.clip = armas[armaAtual].somRecarga;
            emissorSom.PlayOneShot(emissorSom.clip);
            int todasAsBalas = armas[armaAtual].balasNoPente + armas[armaAtual].balasNaArma;
            if (todasAsBalas >= armas[armaAtual].balasPorPente)
            {
                armas[armaAtual].balasNoPente = armas[armaAtual].balasPorPente;
                armas[armaAtual].balasNaArma = todasAsBalas - armas[armaAtual].balasPorPente;
            }
            else
            {
                armas[armaAtual].balasNoPente = todasAsBalas;
                armas[armaAtual].balasNaArma = 0;
            }
            recarregando = true;
            StartCoroutine(TempoRecarga(armas[armaAtual].tempoDaRecarga));
        }
        //laser da arma
        if (!recarregando)
        {
            if (armas[armaAtual].Miras.ativarLaser)
            {
                linhaDoLaser.enabled = true;
                linhaDoLaser.material.SetColor("_TintColor", armas[armaAtual].Miras.corLaser);
                luzColisao.SetActive(true);
                Vector3 PontoFinalDoLaser = transform.position + (transform.forward * 500);
                if (Physics.Raycast(transform.position, Quaternion.AngleAxis(0, transform.up) * transform.forward, out RaycastHit hitDoLaser, 500))
                {
                    linhaDoLaser.SetPosition(0, armas[armaAtual].lugarParticula.transform.position);
                    linhaDoLaser.SetPosition(1, hitDoLaser.point);
                    Debug.Log(hitDoLaser.point);
                    float distancia = Vector3.Distance(transform.position, hitDoLaser.point) - 0.03f;
                    luzColisao.transform.position = transform.position + transform.forward * distancia;
                }
                else
                {
                    linhaDoLaser.SetPosition(0, armas[armaAtual].lugarParticula.transform.position);
                    linhaDoLaser.SetPosition(1, PontoFinalDoLaser);
                    luzColisao.transform.position = PontoFinalDoLaser;
                }
            }
        }
        else
        {
            linhaDoLaser.enabled = false;
            luzColisao.SetActive(false);
        }
        //checar limites da municao
        if (armas[armaAtual].balasNoPente > armas[armaAtual].balasPorPente)
        {
            armas[armaAtual].balasNoPente = armas[armaAtual].balasPorPente;
        }
        else if (armas[armaAtual].balasNoPente < 0)
        {
            armas[armaAtual].balasNoPente = 0;
        }
        int numBalasArma = armas[armaAtual].numeroDeBalas - armas[armaAtual].balasPorPente;
        if (armas[armaAtual].balasNaArma > numBalasArma)
        {
            armas[armaAtual].balasNaArma = numBalasArma;
        }
        else if (armas[armaAtual].balasNaArma < 0)
        {
            armas[armaAtual].balasNaArma = 0;
        }
    }

    IEnumerator TempoTiro(float tempoDoTiro)
    {
        atirando = true;
        yield return new WaitForSeconds(tempoDoTiro);
        atirando = false;
    }

    IEnumerator TempoRecarga(float tempoAEsperar)
    {
        yield return new WaitForSeconds(tempoAEsperar);
        recarregando = false;
    }

    public void TrocarArma(int arma)
    {
        armaAtual = arma;
        AtivarArmaAtual();
    }

    void AtivarArmaAtual()
    {
        for (int x = 0; x < armas.Length; x++)
        {
            armas[x].objetoArma.SetActive(false);
            armas[x].lugarParticula.SetActive(false);

        }
        armas[armaAtual].objetoArma.SetActive(true);
        armas[armaAtual].lugarParticula.SetActive(true);
        if (armas[armaAtual].Miras.ativarLaser == true)
        {
            linhaDoLaser.material.color = armas[armaAtual].Miras.corLaser;
            linhaDoLaser.enabled = true;
            luzColisao.SetActive(true);
            luzColisao.GetComponent<Light>().color = armas[armaAtual].Miras.corLaser;
        }
        else
        {
            linhaDoLaser.enabled = false;
            luzColisao.SetActive(false);
        }
    }

    void OnGUI()
    {
        if (armas[armaAtual].Miras.AtivarMiraComum == true)
        {
            GUIStyle stylez = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter
            };
            GUI.skin.label.fontSize = 20;
            GUI.Label(new Rect(Screen.width / 2 - 6, Screen.height / 2 - 12, 12, 22), "+");
        }
    }
}
