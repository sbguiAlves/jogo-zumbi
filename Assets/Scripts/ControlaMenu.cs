﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlaMenu : MonoBehaviour
{
    public GameObject BotaoSair;
    public GameObject BotoesMenuPrincipal;
    public GameObject MenuSelecaoDeFase;

    private void Start()
    {
        /* - Platform dependent compilation (Compilação dependente da plataforma)
                - UNITY_STANDALONE: só vai acontecer se o jogo estiver para PC 
                - UNITY_EDITOR: só vai acontecer se estiver no Editor
        */
        #if UNITY_STANDALONE || UNITY_EDITOR
            BotaoSair.SetActive(true);
        #endif

    }

    public void SelecionarFase()
    {
        BotoesMenuPrincipal.SetActive(false);
        MenuSelecaoDeFase.SetActive(true);
    }

    public void VoltarMenu()
    {
        BotoesMenuPrincipal.SetActive(true);
        MenuSelecaoDeFase.SetActive(false);
    }

    public void JogarFase1()
    {
        StartCoroutine(MudarCena("fase1"));
    }

    public void JogarFase2() //fazer inapertável por enquanto
    {
        StartCoroutine(MudarCena("fase2"));
    }

    public void JogarFase3()//fazer inapertável por enquanto
    {
        StartCoroutine(MudarCena("fase3"));
    }

    IEnumerator MudarCena(string name)
    {
        /*Pra dar um tempo pro som tocar e dps entrar na cena*/
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadScene(name);
    }

    public void SairJogo()
    {
        StartCoroutine(Sair());
    }

    IEnumerator Sair()
    {
        yield return new WaitForSecondsRealtime(0.3f); //vai esperar por segundos reais
        Application.Quit();

        /* - Enquanto o jogo estive rodando e, se estiver no editor,
        o editor do jogo é fechado, tirando o play.
        */

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
