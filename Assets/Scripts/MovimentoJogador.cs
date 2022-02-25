using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*HERANCA: MovimentoJogador(filho) herda funções de MovimentoPersonagem(pai), assim como os métodos de MonoBehavior*/
public class MovimentoJogador : MovimentoPersonagem
{
    public void RotacaoJogador(LayerMask MascaraChao)
    {
        /*  - A variável do tipo Raio recebe a conversão de ponto para raio que sai da câmera e aponta para a posição do mouse */
        Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(raio.origin, raio.direction * 100, Color.red); // Debug que desenha a origem e a direção do raio na tela

        /*  - Pega o raio gerado e pega a posição de onde está tocando
            - Physics.Raycast para gerar um raio, recebendo como parâmetro o raio, combiná-lo com uma variável de impacto e
            a distância de até onde vai este raio (100 é um valor arbitrário, poderia ir testando)
            - A variável impacto não tem valor nenhum pq ela não tocou em nada, mas após a condição, ele receberá um valor 
        */
        RaycastHit impacto;


        if (Physics.Raycast(raio, out impacto, 100, MascaraChao)) //guardou o ponto em que o raio toca o chão
        {
            /* - A partir do ponto de impacto baseado na posição do jogador, a posição 
            para onde o jogador vai mirar é gerada.
            */
            Vector3 posicaoMiraJogador = impacto.point - transform.position;

            /* -  O raio na posição em Y é cancelado para manter a mira e a posição do jogador na mesma altura
            */
            posicaoMiraJogador.y = transform.position.y;

            Rotacionar(posicaoMiraJogador);
        }
    }
}
