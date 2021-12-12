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
public class Arma
{
    [HideInInspector]
    public int MunicaoExtraNaArma, municao;
    public int danoPorTiro = 40;
    [Range(0, 500)]
    public int municaoExtraMaxima = 240;
    [Range(0, 50)]
    public int municaoMaxima = 30;
    [Range(0.01f, 5.0f)]
    public float TiroPorSegundo = 0.3f;
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
    [SerializeField] private int armaInicial = 0;
    [SerializeField] private int IdArmaVazia = 0;
    [SerializeField] private Text municaoTexto;
    [SerializeField] private Material MaterialLasers;
    [SerializeField] private Arma[] armas;
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

        for (int x = 0; x < armas.Length; x++)
        {
            armas[x].objetoArma.SetActive(false);
            armas[x].lugarParticula.SetActive(false);
            armas[x].MunicaoExtraNaArma = armas[x].municaoExtraMaxima - armas[x].municaoMaxima;
            armas[x].municao = armas[x].municaoMaxima;
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
        municaoTexto.text = "AMMO: " + armas[armaAtual].municao;
        if (Input.GetMouseButton(0) && armas[armaAtual].municao > 0 && !recarregando && !atirando)
        {
            StartCoroutine(TempoTiro(armas[armaAtual].TiroPorSegundo));
            emissorSom.clip = armas[armaAtual].somTiro;
            emissorSom.PlayOneShot(emissorSom.clip);
            armas[armaAtual].municao--;
            armas[armaAtual].weaponAnimator.SetBool("IsShooting", true);
            GameObject balaTemp = Instantiate(armas[armaAtual].particulaFumaca, armas[armaAtual].lugarParticula.transform.position, transform.rotation) as GameObject;
            Destroy(balaTemp, armas[armaAtual].tempoVidaParticula);

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit pontoDeColisao))
            {
                if (pontoDeColisao.transform.gameObject.GetComponent<VehicleData>() != null)
                {
                    pontoDeColisao.transform.gameObject.GetComponent<VehicleData>().ReceiveDamage(armas[armaAtual].danoPorTiro);
                }
            }
        }
        if (!Input.GetMouseButton(0) && atirando || recarregando || armas[armaAtual].municao == 0)
        {
            armas[armaAtual].weaponAnimator.SetBool("IsShooting", false);
            armas[armaAtual].weaponAnimator.SetBool("IsLoop", false);
        }
        if (Input.GetMouseButton(0) && atirando)
        {
            armas[armaAtual].weaponAnimator.SetBool("IsLoop", true);
        }
        //recarregar
        if (Input.GetKeyDown(botaoRecarregar) && !recarregando && !atirando && (armas[armaAtual].municao < armas[armaAtual].municaoMaxima) && (armas[armaAtual].MunicaoExtraNaArma > 0))
        {
            emissorSom.clip = armas[armaAtual].somRecarga;
            emissorSom.PlayOneShot(emissorSom.clip);
            int todasAsBalas = armas[armaAtual].municao + armas[armaAtual].MunicaoExtraNaArma;
            if (todasAsBalas >= armas[armaAtual].municaoMaxima)
            {
                armas[armaAtual].municao = armas[armaAtual].municaoMaxima;
                armas[armaAtual].MunicaoExtraNaArma = todasAsBalas - armas[armaAtual].municaoMaxima;
            }
            else
            {
                armas[armaAtual].municao = todasAsBalas;
                armas[armaAtual].MunicaoExtraNaArma = 0;
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
        if (armas[armaAtual].municao > armas[armaAtual].municaoMaxima)
        {
            armas[armaAtual].municao = armas[armaAtual].municaoMaxima;
        }
        else if (armas[armaAtual].municao < 0)
        {
            armas[armaAtual].municao = 0;
        }
        int numBalasArma = armas[armaAtual].municaoExtraMaxima - armas[armaAtual].MunicaoExtraNaArma;
        if (armas[armaAtual].MunicaoExtraNaArma > numBalasArma)
        {
            armas[armaAtual].MunicaoExtraNaArma = numBalasArma;
        }
        else if (armas[armaAtual].MunicaoExtraNaArma < 0)
        {
            armas[armaAtual].MunicaoExtraNaArma = 0;
        }
        if (Input.GetMouseButton(0) && armas[armaAtual].MunicaoExtraNaArma <= 0 && armas[armaAtual].municao <= 0)
        {
            PegarArma(IdArmaVazia);
        }
        //DEBUG REmOVER
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PegarArma(1);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            PegarArma(2);
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

    /// <summary>
    /// Seleciona a arma de acordo com o ID e faz a recarrega das munições
    /// </summary>
    /// <param name="armaId"></param>
    public void PegarPoweUpArma(int armaId)
    {
        if (armaAtual == armaId)
        {
            armas[armaAtual].municao = armas[armaAtual].municaoMaxima;
        }
        else if (armaAtual == IdArmaVazia || armas[armaAtual].municao <= 0)
        {
            armaAtual = armaId;
            AtivarArmaAtual();
            armas[armaAtual].municao = armas[armaAtual].municaoMaxima;
        }
    }

    /// <summary>
    /// Remove a arma antiga e mostra a nova no local
    /// </summary>
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
