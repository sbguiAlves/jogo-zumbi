﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float Velocidade = 20;
    public int DanoDaBala = 1;

    private Rigidbody rigidbodyBala;
    public AudioClip SomDeMorte;

    void Start()
    {
        rigidbodyBala = GetComponent<Rigidbody>();
    }

    void FixedUpdate() //utilizado devido ao componente RigidBody
    {
        rigidbodyBala.MovePosition
            (rigidbodyBala.position + 
            transform.forward * Velocidade * Time.deltaTime); 

            /*  - Utiliza-se o transform.forward que aponta para frente em relação a posição e rotação local do objeto e não
                ao eixo padrão da Unity, caso o Vector3.forward fosse utilizado.            */ 
    }


        /*  - Quando um Trigger bate em um objeto, o método a seguir é chamado, retornando ao qual objeto colidiu.            */ 
    void OnTriggerEnter(Collider objetoDeColisao)
    {
        if(objetoDeColisao.tag == "Inimigo") //Utilizar tags nos objetos permite que a destruição aconteça apenas com tag "Inimigo"
        {
           objetoDeColisao.GetComponent<ControlaInimigo>().TomarDano(DanoDaBala);
        }

        Destroy(gameObject);//destruir apenas a bala e não os objetos que entrou em contato.
        //dps dar um jeito das balas destruirem dps de um tempo
        
    }
}
