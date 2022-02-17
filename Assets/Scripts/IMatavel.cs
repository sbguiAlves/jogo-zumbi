using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMatavel //todo mundo que for morrer, tem que ter essa interface
{
    void TomarDano(int dano);
    void Morrer();

    /*-----------INTERFACE-----------
        - Outros scripts podem usar os métodos da interface, porém cada método tratará
        do resultado de uma forma diferente. Tipo, as classes ControlaJogador e 
        ControlaInimigo possuem possuem os métodos TomarDano() e Morrer(), mas a maneira
        que estão escritos e como os resultados são retornados são diferentes.

    */
}
