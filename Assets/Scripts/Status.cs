using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int VidaInicial = 100;

    [HideInInspector]
    public int Vida;
    public float Velocidade = 5;

    public float DistanciaParaVagar = 10;
    public float DistanciaParaPerseguir = 2.5f;

    void Awake()
    {
        Vida = VidaInicial;
    }
}
