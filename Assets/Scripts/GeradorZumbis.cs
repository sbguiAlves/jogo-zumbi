using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorZumbis : MonoBehaviour
{
    public GameObject Zumbi;
    public float TempoGerarZumbi = 1;
    public LayerMask LayerZumbi;

    private float distanciaDeSpawn = 3;
    private float contadorTempo = 0;
    private float distanciaDoJogadorParaSpawn = 20;
    private GameObject jogador;

    private void Start()
    {
        jogador = GameObject.FindWithTag(Tags.Jogador);
    }

    /*  ----------- PRÁTICAS DE OTIMIZAÇÃO EM UNITY -----------
    - Variáveis públicas: começar com letra maiúscula (lower camel case)
    - Variáveis privadas: começar com letra minúscula (pascal case/ upper camel case)

    - Sempre é uma boa prática definir o acesso das variáveis, mesmo que ela seja usada
    apenas dentro da classe que foi criada. Neste caso, tipar com 'private'.

    */

    void Update()
    {
        if (Vector3.Distance(transform.position, jogador.transform.position) >
            distanciaDoJogadorParaSpawn)
        {
            contadorTempo += Time.deltaTime; //tempo em segundos

            if (contadorTempo >= TempoGerarZumbi)
            {
                StartCoroutine(GerarNovoZumbi());
                contadorTempo = 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaDeSpawn);
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

        Instantiate(Zumbi, posicaoDeCriacao, transform.rotation);
    }

    Vector3 AleatorizarPosicao()
    {
        Vector3 posicao = Random.insideUnitSphere * distanciaDeSpawn;
        posicao += transform.position;
        posicao.y = 0;

        return posicao;
    }
}
