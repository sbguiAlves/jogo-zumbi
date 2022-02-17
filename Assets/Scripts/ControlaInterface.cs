using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlaInterface : MonoBehaviour
{
    private ControlaJogador scriptControlaJogador;
    public Slider SliderVidaJogador;
    public GameObject PainelDeGameOver;
    public Text TextoTempoDeSobrevivencia;
    public Text TextoPontuacaoMaxima;

    private float tempoPontuacaoSalva;

    /* Boa prática: começar variaveis com o tipo dela (quando não é float, int, etc.)
    */

    void Start()
    {
        scriptControlaJogador = GameObject.FindWithTag(Tags.Jogador).GetComponent<ControlaJogador>();

        SliderVidaJogador.maxValue = scriptControlaJogador.statusJogador.Vida;
        AtualizarSliderVidaJogador(); //atualiza pela quantidade de vida do jogador sem ter que mudar no Inspector
        Time.timeScale = 1;

        tempoPontuacaoSalva = PlayerPrefs.GetFloat("PontuacaoMaxima");
    }

    public void AtualizarSliderVidaJogador()
    {
        SliderVidaJogador.value = scriptControlaJogador.statusJogador.Vida;
    }

    public void GameOver()
    {
        PainelDeGameOver.SetActive(true);
        /*Jogo é pausado para dar o game over quando a vida do jogador zerar*/
        Time.timeScale = 0;

        //Como calcular o tempo sobrevivido desde que o nivel começou: Time.timeSinceLevelLoad
        int minutos = (int)(Time.timeSinceLevelLoad / 60);
        int segundos = (int)(Time.timeSinceLevelLoad % 60);

        TextoTempoDeSobrevivencia.text = 
            "Você sobreviveu por: \n " + minutos + "min e " + segundos + "s";

        AjustarPontuacaoMaxima(minutos, segundos);
    }

    void AjustarPontuacaoMaxima(int min, int seg)
    {
        if(Time.timeSinceLevelLoad > tempoPontuacaoSalva)
        {
            tempoPontuacaoSalva = Time.timeSinceLevelLoad;
            /*Ao utilizar o string.Format, {0} representa o primeiro argumento e {1} representa o segundo argumento*/
            TextoPontuacaoMaxima.text = string.Format("Seu melhor tempo é {0}min e {1}s", min, seg);

            /*Salva as preferências do jogador no sistema, mesmo que o jogo for fechado*/
            PlayerPrefs.SetFloat("PontuacaoMaxima", tempoPontuacaoSalva);
        }

        if(TextoPontuacaoMaxima.text == "")
        {
            min = (int)tempoPontuacaoSalva / 60;
            seg = (int)tempoPontuacaoSalva % 60;
            TextoPontuacaoMaxima.text = string.Format("Seu melhor tempo é {0}min e {1}s", min, seg);
        }
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene("game");
    }
}
