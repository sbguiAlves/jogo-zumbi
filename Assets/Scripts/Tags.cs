using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour
{
    public const string Jogador = "Jogador";
    /*
        -static: a variável não pode trocar de valor durante a execução do jogo
        Sempre que for preciso usar a Tag em outro código, faça:
            GameObject.FindWithTag(Tags.'NOME DA VARIÁVEL PÚBLICA');
        
        - Não precisa declarar o GetComponent no Start da classe
    */
}
