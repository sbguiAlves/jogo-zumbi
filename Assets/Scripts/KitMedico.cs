using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitMedico : MonoBehaviour
{
    private int quantidadeDeCura = 20; //melhor deixar private pra definir um valor só de cura pro kit
    private int tempoDeDestruicao = 5;

    public AudioClip SomDeItemColetado;

    private void Start()
    {
        Destroy(gameObject, tempoDeDestruicao);
    }

    private void OnTriggerEnter(Collider objetoDeColisao)
    {
        if (objetoDeColisao.tag == Tags.Jogador)
        {
            objetoDeColisao.GetComponent<ControlaJogador>().CurarVida(quantidadeDeCura);
            ControlaAudio.instancia.PlayOneShot(SomDeItemColetado);
            Destroy(gameObject);
        }
    }
}
