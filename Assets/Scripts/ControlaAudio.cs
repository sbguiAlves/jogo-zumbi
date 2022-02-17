using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaAudio : MonoBehaviour
{
    private AudioSource meuAudioSource;

    /* static: o valor da variável é estático, ou seja, ao mudar o valor da variável
    instancia em outro script, ele mudará em todos os outros scripts. */
    public static AudioSource instancia;

    /*Awake: roda antes dos Start das demais classes rodarem
    
    Este comportamento para tocar áudio de qualquer lugar do jogo diante dos scripts
    é conhecido como o padrão de projeto SINGLETON.

    Vantagem: Desta forma, tocamos o áudio a partir do mesmo AudioSource, evitando
    criar um AudioSource para cada objeto sem necessidade.
    */
    void Awake()
    {
        meuAudioSource = GetComponent<AudioSource>();
        instancia = meuAudioSource;
    }
}
