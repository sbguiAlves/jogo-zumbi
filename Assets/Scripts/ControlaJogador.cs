using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaJogador : MonoBehaviour, IMatavel, ICuravel
{
    private Vector3 direcao;

    /* - A LayerMask "MascaraChao" é utilizada para separar o chão do resto do cenário, colidindo APENAS com o chão */
    public LayerMask MascaraChao;
    public GameObject TextoGameOver;
    public ControlaInterface scriptControlaInterface;
    public AudioClip SomDeDano;

    /*  ----------- PRÁTICAS DE OTIMIZAÇÃO EM UNITY -----------
    - Em vez de declarar GetComponent<> separadamente para cada chamada de função,
    starto com uma variável privada na classe, evitando que eu chame pra todos os
    momentos que precisar.
    */
    private MovimentoJogador meuMovimentoJogador;
    private AnimacaoPersonagem animacaoJogador;
    public Status statusJogador;

    void Start()
    {
        animacaoJogador = GetComponent<AnimacaoPersonagem>();
        meuMovimentoJogador = GetComponent<MovimentoJogador>();
        statusJogador = GetComponent<Status>();
    }

    /* Uma das boas práticas é não utilizar tanto o Update
    com métodos. Neste caso, utilizamos mais de condições dos
    métodos.*/
    void Update()
    {
        //Inputs do Jogador - Guardando valores das teclas de movimento
        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");

        direcao = new Vector3(eixoX, 0, eixoZ); //Utilizar apenas desta variável, o personagem movimentará sem física

        animacaoJogador.Movimentar(direcao.magnitude); //magnitude: tamanho unitário do vetor

    }

    /*Update que roda no tempo fixo a cada 0.02s*/
    void FixedUpdate()
    {
        meuMovimentoJogador.Movimentar(direcao, statusJogador.VelocidadeJogador);


        meuMovimentoJogador.RotacaoJogador(MascaraChao);

    }

    /* Uma boa prática é utilizar métodos para organizar e manter o código
    com pequenos comportamentos, respeitando o encapsulamento. Assim, se
    precisar utilizar de parâmetros de outra classe, usamos diretamente
    os métodos.
    */
    public void TomarDano(int dano)
    {
        statusJogador.VidaJogador -= dano;
        scriptControlaInterface.AtualizarSliderVidaJogador();
        /* PlayOneShot: toca o som apenas uma vez */
        ControlaAudio.instancia.PlayOneShot(SomDeDano);

        if (statusJogador.VidaJogador <= 0)
            Morrer();
    }

    public void Morrer()
    {
        scriptControlaInterface.GameOver();
    }

    public void CurarVida(int quantidadeDeCura)
    {
        statusJogador.VidaJogador += quantidadeDeCura;

        if (statusJogador.VidaJogador > statusJogador.VidaInicialJogador)
        {
            statusJogador.VidaJogador = statusJogador.VidaInicialJogador;
        }

        scriptControlaInterface.AtualizarSliderVidaJogador();
    }
}
