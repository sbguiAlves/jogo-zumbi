using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int VidaInicial = 100;

    [HideInInspector]
    public int Vida;
    public float VelocidadeJogador = 10;
    public float VelocidadeMinZumbi = 2;
    public float VelocidadeMaxZumbi = 8;

    public float DistanciaParaVagar = 10;
    public float DistanciaParaPerseguir = 2.5f;

    void Awake()
    {
        Vida = VidaInicial;
    }

    public float AleatorizarVelocidade()
    {
        float VelocidadeZumbi = Random.Range(VelocidadeMinZumbi, VelocidadeMaxZumbi);

        return VelocidadeZumbi;
    }
}
