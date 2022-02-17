using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    private Rigidbody meuRigidbody;

    void Awake()
    {
        meuRigidbody = GetComponent<Rigidbody>();
    }

    public void Movimentar(Vector3 direcao, float velocidade)
    {
        /*  - A variável "direcao" é limitada por um valor que vai de -1 a 1 em X e Z.
            - Ao deixar de normalizá-la, o inimigo moverá em direções em Vector3 que são bem maiores que as bases.
            - Assim, a normalização pega um VETOR de algum tamanho e transforma em outro que sempre terá tamanho máximo 1.
            - Portanto, utiliza-se o método "direcao.normalized. */

        meuRigidbody.MovePosition
            (meuRigidbody.position + //posição pela física
            direcao.normalized * velocidade * Time.deltaTime); //Posição Destino 
    }

    public void Rotacionar(Vector3 direcao)
    {
        /*  - Quaterninon: calcula a rotação ao utilizar os eixos (X, Y, Z) e um eixo imaginário
            - LookRotation: passa uma posição e calcula o quanto tenho que rotacionar para olhar para esta posição */

        Quaternion novaRotacao = Quaternion.LookRotation(direcao);
        meuRigidbody.MoveRotation(novaRotacao);
    }

}
