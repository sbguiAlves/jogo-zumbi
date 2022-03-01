using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlaInterface : MonoBehaviour
{
    public Slider SliderVidaJogador;

    public GameObject PainelDeVitoria;
    public GameObject PainelDeDerrota;
    public GameObject PainelDeJogoPausado;

    public Text TextoTempoDeSobrevivencia;
    public Text TextoPontuacaoMaxima;
    public Text TextoQuantidadeZumbisMortos;
    public Text TextoChefeAparece;
    public Text TextoTimer;

    [HideInInspector]
    public float TempoObjetivo;
    public float TempoObjetivoEmMinutos = 5;

    private ControlaJogador scriptControlaJogador;
    private float tempoPontuacaoSalva;
    private int quantidadeDeZumbisMortos = 0;
    private bool isOver = false;

    public AudioSource MusicaDeFundo;

    public AudioClip SomDeGameOver;
    public AudioClip SomDeVitoria;

    /* Boa prática: começar variaveis com o tipo dela (quando náo é float, int, etc.) */

    void Start()
    {
        TempoObjetivo = TempoObjetivoEmMinutos;
        scriptControlaJogador = GameObject.FindWithTag(Tags.Jogador).GetComponent<ControlaJogador>();

        SliderVidaJogador.maxValue = scriptControlaJogador.statusJogador.VidaJogador;
        AtualizarSliderVidaJogador(); //atualiza pela quantidade de vida do jogador sem ter que mudar no Inspector
        Time.timeScale = 1;

        tempoPontuacaoSalva = PlayerPrefs.GetFloat("PontuacaoMaxima");
        isOver = false;
        TempoParaSobreviver();
    }

    public void AtualizarSliderVidaJogador()
    {
        SliderVidaJogador.value = scriptControlaJogador.statusJogador.VidaJogador;
    }

    public void AtualizarQuantidadeDeZumbisMortos()
    {
        quantidadeDeZumbisMortos++;
        TextoQuantidadeZumbisMortos.text = string.Format("x {0}", quantidadeDeZumbisMortos);
    }

    public void GameOver()
    {
        MusicaDeFundo.Stop();

        Time.timeScale = 0; /*Jogo é pausado */

        if (isOver == true)
        {
            ControlaAudio.instancia.PlayOneShot(SomDeVitoria);

            PainelDeVitoria.SetActive(true);
            PainelDeDerrota.SetActive(false);
        }
        else if (isOver == false)
        {
            ControlaAudio.instancia.PlayOneShot(SomDeGameOver);

            PainelDeDerrota.SetActive(true);
            TextoTimer.gameObject.SetActive(false);

            //Como calcular o tempo sobrevivido desde que o nivel começou: Time.timeSinceLevelLoad
            int minutos = (int)(Time.timeSinceLevelLoad / 60);
            int segundos = (int)(Time.timeSinceLevelLoad % 60);

            TextoTempoDeSobrevivencia.text =
                "Você sobreviveu por\n" + minutos + "min e " + segundos + "s";

            AjustarPontuacaoMaxima(minutos, segundos);
        }
    }


    public void TempoParaSobreviver()
    {
        float sobrevivenciaEmSegundos = TempoObjetivo * 60;
        if (Time.timeSinceLevelLoad >= sobrevivenciaEmSegundos)
        {
            GameOver();
            isOver = true;
        }
        else
        {
            StartCoroutine(RelogioDaFase(TextoTimer));
        }
    }

    IEnumerator RelogioDaFase(Text textoTimer)
    {
        float contador = 0;

        contador += Time.timeSinceLevelLoad;

        int minutos = (int)(Time.timeSinceLevelLoad / 60);
        int segundos = (int)(Time.timeSinceLevelLoad % 60);

        textoTimer.text = minutos.ToString("D2") + ":" + segundos.ToString("D2");

        yield return null;
    }

    public void PausarJogo()
    {
        MusicaDeFundo.Pause();
        PainelDeJogoPausado.SetActive(true);
        Time.timeScale = 0;
    }

    public void DespausarJogo()
    {
        StartCoroutine(VoltarParaFase());
    }

    IEnumerator VoltarParaFase()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        MusicaDeFundo.Play();
        PainelDeJogoPausado.SetActive(false);
        Time.timeScale = 1;
    }

    public void VoltarMenu()
    {
        StartCoroutine(MudarParaMenu("menu"));
    }

    IEnumerator MudarParaMenu(string name)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(name);
    }

    public void ReiniciarFase()
    {
        Scene scene = SceneManager.GetActiveScene();
        StartCoroutine(ReiniciarMusica(scene.name));
    }

    IEnumerator ReiniciarMusica(string name) //tem algo errado aqui
    {
        MusicaDeFundo.Stop();
        yield return new WaitForSecondsRealtime(0.5f);

        isOver=false;
        TempoObjetivo = TempoObjetivoEmMinutos;
        SceneManager.LoadScene(name);
    }

    private void AjustarPontuacaoMaxima(int min, int seg)
    {
        if (Time.timeSinceLevelLoad > tempoPontuacaoSalva)
        {
            tempoPontuacaoSalva = Time.timeSinceLevelLoad;
            /*Ao utilizar o string.Format, {0} representa o primeiro argumento e {1} representa o segundo argumento*/
            TextoPontuacaoMaxima.text = string.Format("Recorde:\n{0}min e {1}s", min, seg);

            /*Salva as preferências do jogador no sistema, mesmo que o jogo for fechado*/
            PlayerPrefs.SetFloat("PontuacaoMaxima", tempoPontuacaoSalva);
        }

        if (TextoPontuacaoMaxima.text == "")
        {
            min = (int)tempoPontuacaoSalva / 60;
            seg = (int)tempoPontuacaoSalva % 60;
            TextoPontuacaoMaxima.text = string.Format("Recorde:\n{0}min e {1}s", min, seg);
        }
    }

    public void AparecerTextoChefeCriado()//mudar para indicar quantos chefes tem em cena
    {
        StartCoroutine(DesaparecerTexto(2, TextoChefeAparece));
    }

    /* Corrotina que faz o texto ir desaparecendo aos poucos
    */
    IEnumerator DesaparecerTexto(float tempoDeSumico, Text textoParaSumir)
    {
        /* - Garante que o texto está totalmente visível */
        textoParaSumir.gameObject.SetActive(true);
        Color corTexto = textoParaSumir.color;
        corTexto.a = 1;
        textoParaSumir.color = corTexto;

        yield return new WaitForSeconds(1.5f); //espera 1 segundo

        float contador = 0;
        while (textoParaSumir.color.a > 0)
        {
            contador += Time.deltaTime / tempoDeSumico; //razão baseado no tempo de sumiço
            corTexto.a = Mathf.Lerp(1, 0, contador); //mathf tem haver com números
            textoParaSumir.color = corTexto;

            if (textoParaSumir.color.a <= 0)
            {
                textoParaSumir.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

}
