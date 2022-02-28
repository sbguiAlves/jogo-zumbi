using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorChefe : MonoBehaviour
{
    private ControlaInterface scriptControlaInterface;
    private float tempoParaProximaGeracao = 0;

    public float tempoEntreGeracoes = 60;
    public GameObject ChefePrefab;
    public AudioClip AlertaDeChefao;
    public Transform[] PosicoesPossiveisDeSpawn;


    private Transform jogador;


    private void Start()
    {
        tempoParaProximaGeracao = tempoEntreGeracoes;
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
        jogador = GameObject.FindWithTag(Tags.Jogador).transform;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > tempoParaProximaGeracao)
        {
            Vector3 posicaoDeCriacao = CalcularPosicaoMaisDistanteDoJogador();
            ControlaAudio.instancia.PlayOneShot(AlertaDeChefao);
            Instantiate(ChefePrefab, posicaoDeCriacao, Quaternion.identity);
            scriptControlaInterface.AparecerTextoChefeCriado();
            tempoParaProximaGeracao = Time.timeSinceLevelLoad + tempoEntreGeracoes;
        }
    }

    Vector3 CalcularPosicaoMaisDistanteDoJogador()
    {
        Vector3 posicaoDeMaiorDistancia = Vector3.zero;
        float maiorDistancia = 0;
        //para cada um dos elementos, faça isso apenas uma vez no frame atual
        foreach (Transform posicao in PosicoesPossiveisDeSpawn)
        {
            float distanciaEntreJogador = Vector3.Distance(posicao.position, jogador.position);
            if (distanciaEntreJogador > maiorDistancia)
            {
                maiorDistancia = distanciaEntreJogador;
                posicaoDeMaiorDistancia = posicao.position;
            }

        }

        return posicaoDeMaiorDistancia;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        int numeroDeSpawns = PosicoesPossiveisDeSpawn.Length;

        for (int i = 0; i < numeroDeSpawns; i++)
        {
            Gizmos.DrawWireSphere(PosicoesPossiveisDeSpawn[i].position, 1);
        }
    }

}
