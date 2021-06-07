using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaItem : MonoBehaviour
{
    public int quantidadeBala;
    public int quantidadePente;
    public int tipoArma;

    public int GetBala()
    {
        return quantidadeBala;
    }
    public int GetPente()
    {
        return quantidadePente;
    }
    public int GetArma()
    {
        return tipoArma;
    }
}