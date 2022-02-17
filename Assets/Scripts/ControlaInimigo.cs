using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaInimigo : MonoBehaviour, IMatavel
{
    public GameObject Jogador;
    public GameObject KitMedicoPrefab;

    public AudioClip SomDeMorte;

    public int TamanhoDaEsfera = 10;

    private Vector3 posicaoAleatoria;
    private Vector3 direcao;
    private float contadorVagar;
    private float tempoEntrePosicoesAleatorias = 4;
    private float porcentagemGerarKitMedico = 0.1f; //10%
    

    private AnimacaoPersonagem animacaoInimigo;
    private MovimentoPersonagem movimentaInimigo;
    private Status statusInimigo;  

    void Start()
    {
        animacaoInimigo = GetComponent<AnimacaoPersonagem>();
        movimentaInimigo = GetComponent<MovimentoPersonagem>();
        statusInimigo = GetComponent<Status>();

        AleatorizarZumbi();
        Jogador = GameObject.FindWithTag(Tags.Jogador);//procurar jogador pela tag
    }

    void FixedUpdate()
    {
        float distancia = Vector3.Distance(transform.position, Jogador.transform.position); //passa duas posições e ele me dá a distancia

        movimentaInimigo.Rotacionar(direcao);
        animacaoInimigo.Movimentar(direcao.magnitude);

        if(distancia > statusInimigo.DistanciaParaVagar)
        {
            Vagar();
        }
        else if (distancia > statusInimigo.DistanciaParaPerseguir) // persegue o jogador
        {
            Perseguir();
            animacaoInimigo.Atacar(false);
        }
        else //Quando estiver perto do jogador, atacar
        {
            direcao = Jogador.transform.position - transform.position;
            animacaoInimigo.Atacar(true);
        }
    }

    void Perseguir()
    {
        direcao = Jogador.transform.position - transform.position;//Posição Destino (jogador) - Posição Origem.
        movimentaInimigo.Movimentar(direcao, statusInimigo.Velocidade);
    }

    void Vagar()
    {
        contadorVagar -= Time.deltaTime;
        if(contadorVagar <= 0)
        {
            posicaoAleatoria = AleatorizarPosicao();
            contadorVagar += tempoEntrePosicoesAleatorias;
        }

        /* Obs.: A game engine não é capaz de calcular com exatidão a diferença de posição atual
        com a destino, dando um bug no intervalo de distância para o movimento. Assim, utiliza-se
        a variável booleana a seguir para dizer que chegou perto suficiente da posição final, deixando
        de bugar a movimentação.

        - Invés da diferença ser 0, o que equivaleria a estar na mesma posição, usamos o valor de 0.05
        para realizar este cálculo.
        */

        bool ficouPertoOSuficiente = Vector3.Distance(transform.position, posicaoAleatoria) <= 0.05;
        if(ficouPertoOSuficiente == false)
        {
            direcao = posicaoAleatoria - transform.position;
            movimentaInimigo.Movimentar(direcao, statusInimigo.Velocidade);
        }
    }

    Vector3 AleatorizarPosicao()
    {
        //vai pegar uma esfera de raio 1 e aleatorizar dentro dessa esfera uma posição qualquer
        Vector3 posicao = Random.insideUnitSphere * TamanhoDaEsfera;
        posicao += transform.position;
        posicao.y = transform.position.y;

        return posicao;
    }

    void AtacaJogador()
    {
        int dano = Random.Range(20, 30);
        Jogador.GetComponent<ControlaJogador>().TomarDano(dano);
    }

    void AleatorizarZumbi()
    {
        int geraTipoZumbi = Random.Range(1, 28);
        transform.GetChild(geraTipoZumbi).gameObject.SetActive(true); //achei um dos filhos em Zumbi e ativar um dos tipos.
    }

    public void TomarDano(int dano)
    {
        statusInimigo.Vida -= dano;
        if (statusInimigo.Vida <= 0)
            Morrer();
    }

    public void Morrer()
    {
        Destroy(gameObject); //o objeto que a bala entrar em contato é destruído
        ControlaAudio.instancia.PlayOneShot(SomDeMorte);
        VerificarGeracaoKitMedico(porcentagemGerarKitMedico);
    }

    void VerificarGeracaoKitMedico(float porcentagemGeracao)
    {
        //Random.value pega todos os números decimais de 0.0 a 1.0
        if(Random.value <= porcentagemGeracao)
        {
            Instantiate(KitMedicoPrefab,transform.position, Quaternion.identity);
        }
    }
}
