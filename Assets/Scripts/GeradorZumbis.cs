using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorZumbis : MonoBehaviour
{
    public GameObject Zumbi;
    public LayerMask LayerZumbi;
    public float TempoGerarZumbi = 1;
    public Transform[] PosicoesSpawnParaZumbis;

    private GameObject jogador;
    private int quantidadeMaximaZumbisVivos = 2, quantidadeDeZumbisVivos;
    private float distanciaDeSpawn = 1f; /*Tamanho da área circular de ataque do zumbi*/
    private float contadorTempo = 0;
    private float distanciaDoJogadorParaSpawn = 5;
    private float contadorAumentarDificuldade, tempoProximoAumentoDeDificuldade = 20; //em segundos

    private void Start()
    {
        jogador = GameObject.FindWithTag(Tags.Jogador);
        contadorAumentarDificuldade = tempoProximoAumentoDeDificuldade;
        for (int i = 0; i < quantidadeMaximaZumbisVivos; i++)
        {
            StartCoroutine(GerarNovoZumbi());
        }
    }

    /*  ----------- PRÁTICAS DE OTIMIZAÇÃO EM UNITY -----------
    - Variáveis públicas: começar com letra maiúscula (lower camel case)
    - Variáveis privadas: começar com letra minúscula (pascal case/ upper camel case)

    - Sempre é uma boa prática definir o acesso das variáveis, mesmo que ela seja usada
    apenas dentro da classe que foi criada. Neste caso, tipar com 'private'.

    */

    void Update()
    {
        bool possoGerarZumbisPelaDistancia = Vector3.Distance(transform.position, jogador.transform.position) >
            distanciaDoJogadorParaSpawn;

        if (possoGerarZumbisPelaDistancia == true && quantidadeDeZumbisVivos <
            quantidadeMaximaZumbisVivos)
        {
            contadorTempo += Time.deltaTime; //tempo em segundos

            if (contadorTempo >= TempoGerarZumbi)
            {
                StartCoroutine(GerarNovoZumbi());
                contadorTempo = 0;
            }
        }

        //contador alternativo
        if (Time.timeSinceLevelLoad > contadorAumentarDificuldade)
        {       // A LÓGICA TÁ AQUI
            quantidadeMaximaZumbisVivos += PosicoesSpawnParaZumbis.Length; //tem que aumentar pelo numero de spawns

            Debug.LogFormat("Máximo de zumbis: {0}\nQuantidade Vida: {1}", quantidadeMaximaZumbisVivos, quantidadeDeZumbisVivos);

            contadorAumentarDificuldade = Time.timeSinceLevelLoad +
                tempoProximoAumentoDeDificuldade;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        int numeroDeSpawns = PosicoesSpawnParaZumbis.Length;

        for (int i = 0; i < numeroDeSpawns; i++)
        {
            Gizmos.DrawWireSphere(PosicoesSpawnParaZumbis[i].position, distanciaDeSpawn);
        }
    }

    /*  - Usar IEnumerator, StartCoroutine() e 'yield return null' é uma ótima estratégia
        quando for usar while e evitar que trave a Unity e precisar
        reiniciar por completo.

        - Ao usar desta estratégia, a linha de execução do código sai do
        método e espera o próximo frame para rodar o método de novo, caso
        a condição do laço de repetição não for atendido como se espera
        para sair do laço.
    */
    IEnumerator GerarNovoZumbi()
    {
        foreach (Transform gerador in PosicoesSpawnParaZumbis)
        {
            Vector3 posicaoDeCriacao = AleatorizarPosicao();

            /* - Quando o zumbi nascer, a condição vai testar se há
            outro zumbi no mesmo local a partir da colisão. Se houver,
            logo existe uma condição de colisão naquele lugar.

                - Coleção de colisores, onde houveram colisões. Assim,
            'colisores' retorna todos os zumbis que colidiram dentro
            da esfera de raio 1.
            */
            Collider[] colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi);

            while (colisores.Length > 0)
            {
                /* Quer dizer que houve colisão e os zumbis não podem ser criados
                naquele local. Assim entra em loop até encontrar uma posição
                apropriada para gerar o zumbi*/
                posicaoDeCriacao = AleatorizarPosicao();
                colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi);
                yield return null;
            }

            /*  - Cada gerador vai cuidar dos seus zumbis e verificar se deve
                ou não criar mais.
                - Então atrelamos o zumbi ao gerador, criando uma variável no 
                script de ControlaInimigo que guarda o gerador daquele zumbi
                - A variável será pública, porque preenchemos essa variável
                no momento que criamos o zumbi no script GeradorZumbis */

            //Debug.LogFormat("Posição do gerador: {0}\n Posicao de Criação: {1}",gerador.position, posicaoDeCriacao);

            ControlaInimigo zumbi = Instantiate(Zumbi, gerador.position + posicaoDeCriacao, Quaternion.identity).GetComponent<ControlaInimigo>();

            /*  - No momento em que o zumbi é criado, guarda-se seu script
            de ControlaInimigo */
            zumbi.meuGerador = this;
            quantidadeDeZumbisVivos++;
        }
    }

    Vector3 AleatorizarPosicao()
    {
        Vector3 posicao = Random.insideUnitSphere * distanciaDeSpawn;
        posicao += transform.position;
        posicao.y = 0;

        return posicao;
    }

    public void DiminuirQuantidadeZumbisVivos()
    {
        quantidadeDeZumbisVivos--;
    }
}
