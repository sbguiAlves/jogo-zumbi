using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int VidaInicialJogador = 100;
    public float VelocidadeJogador = 10;
    
    public int VidaInicialChefe = 100;
    public int VidaInicialZumbi = 5;

    [HideInInspector]
    public int VidaJogador, VidaChefe, VidaZumbi;

    public float VelocidadeMinZumbi = 2;
    public float VelocidadeMaxZumbi = 6;

    public float DistanciaParaVagar = 10;
    public float DistanciaParaPerseguir = 5f;

    void Awake()
    {
        VidaJogador = VidaInicialJogador;
        VidaChefe = VidaInicialChefe;
        VidaZumbi = VidaInicialZumbi;
    }

    public float AleatorizarVelocidade()
    {
        float VelocidadeZumbi = Random.Range(VelocidadeMinZumbi, VelocidadeMaxZumbi);

        return VelocidadeZumbi;
    }
}
