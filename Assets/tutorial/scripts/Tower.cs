using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform torre;
    public Transform cano;
    [Range(0.0f, 100.0f)]
    public float velocidadeTorreRotacao = 45.0f;
    [Range(0.0f, 100.0f)]
    public float velocidadeCanoRotacao = 45.0f;
    [Range(0.0f, 100.0f)]
    public float limiteRotacaoTorre = 45.0f;
    [Range(0.0f, 100.0f)]
    public float limiteRotacaoCano = 45.0f;
    private float rotacaoTorre;
    private float rotacaoCano;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotacionarTorre();
    }

    void RotacionarTorre()
    {
        rotacaoTorre += Input.GetAxis("Mouse X") * velocidadeTorreRotacao * Time.deltaTime;
        rotacaoTorre = Mathf.Clamp(rotacaoTorre, 0, 180);
        torre.localRotation = Quaternion.AngleAxis(rotacaoTorre, Vector3.up);
    }
    void RotacionarCano()
    {
        // https://youtu.be/gaDFNCRQr38?t=443
        // https://www.youtube.com/watch?v=nJiFitClnKo
    }
}
