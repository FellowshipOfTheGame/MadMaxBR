using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegarItens : MonoBehaviour
{
    public Atirar torreta;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "item" && other.gameObject.GetComponent<CaixaItem>())
        {
            torreta.TrocarArma(other.gameObject.GetComponent<CaixaItem>().GetArma());
            torreta.armas[torreta.armaInicial].balasNaArma = other.gameObject.GetComponent<CaixaItem>().GetBala();
            torreta.armas[torreta.armaInicial].balasNoPente = other.gameObject.GetComponent<CaixaItem>().GetPente();
            Destroy(other.gameObject);
        }
    }
}
