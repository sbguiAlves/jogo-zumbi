using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControlaChefe : MonoBehaviour, IMatavel
{
    private Transform jogador;
    private NavMeshAgent agente;
    private Status statusChefe;
    private AnimacaoPersonagem animacaoChefe;
    private MovimentoPersonagem movimentoChefe;

    public GameObject KitMedicoPrefab;
    public GameObject ParticulaSangueZumbi;
    public Slider SliderVidaChefe;
    public AudioClip SomDeMorte;

    public Image ImagemSlider;
    public Color CorDaVidaMaxima, CorDaVidaMinima;

    public int DanoMinimoChefe = 30, DanoMaximoChefe = 40;

    private void Start()
    {
        jogador = GameObject.FindWithTag(Tags.Jogador).transform;
        agente = GetComponent<NavMeshAgent>();
        statusChefe = GetComponent<Status>();
        agente.speed = statusChefe.VelocidadeMaxZumbi;

        animacaoChefe = GetComponent<AnimacaoPersonagem>();
        movimentoChefe = GetComponent<MovimentoPersonagem>();

        SliderVidaChefe.maxValue = statusChefe.VidaChefe;
        AtualizarInterface();
    }

    private void Update()
    {
        agente.SetDestination(jogador.position);
        animacaoChefe.Movimentar(agente.velocity.magnitude);

        if (agente.hasPath == true) //só se tem um destino
        {
            //remaining: oq falta pra chegar no destino; stoppingDistance: destino de parada
            bool estouPertoDoJogador = agente.remainingDistance <= agente.stoppingDistance;

            if (estouPertoDoJogador)
            {
                animacaoChefe.Atacar(true);
                Vector3 direcao = jogador.position - transform.position;

                //isKinematic joga a responsabilidade pro NavMesh em vez de ser o RigidBody para comandar o movimento
                movimentoChefe.Rotacionar(direcao);
            }
            else
            {
                animacaoChefe.Atacar(false);
            }
        }
    }

    void AtacaJogador()
    {
        int dano = Random.Range(DanoMinimoChefe, DanoMaximoChefe);
        jogador.GetComponent<ControlaJogador>().TomarDano(dano);
    }

    public void TomarDano(int dano)
    {
        statusChefe.VidaChefe -= dano;
        AtualizarInterface();

        if (statusChefe.VidaChefe <= 0)
        {
            Morrer();
        }
    }

    public void ParticulaSangue(Vector3 posicao, Quaternion rotacao)
    {
        Instantiate(ParticulaSangueZumbi, posicao, rotacao);
    }

    public void Morrer()
    {
        animacaoChefe.Morrer();
        movimentoChefe.Morrer();
        this.enabled = false;
        agente.enabled = false;
        ControlaAudio.instancia.PlayOneShot(SomDeMorte);

        Instantiate(KitMedicoPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, 2);
    }

    void AtualizarInterface()
    {
        SliderVidaChefe.value = statusChefe.VidaChefe;

        float porcentagemDaVida = (float)statusChefe.VidaChefe / statusChefe.VidaInicialChefe;
        /*  - Interpolação linear: de uma cor para a outra
            - De onde eu quero ir, para qual tom de cor deve chegar no limiar
            - E a razão entre as duas
        */
        Color corDaVida = Color.Lerp(CorDaVidaMinima, CorDaVidaMaxima, porcentagemDaVida);
        ImagemSlider.color = corDaVida;
    }
}
