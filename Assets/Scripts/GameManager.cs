using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameButtons;
    public GameObject OptionsScreen;
    public GameObject Player;
    public float AnimationDuration = 3.0f;
    public GameObject ButtonText;
    public TextMeshProUGUI scoreText;// Refer�ncia para o TMP do contador de pontos
    public Button StartButton;
    public GameObject ScoreTextObj;
    public GameObject GameOverTxt;// Refer�ncia para o game over
    public GameObject TitleTxt;// Refer�ncia para o t�tulo
    private bool gameIsPaused = true; // Estado inicial do jogo
    public float score = 0f;// pontua��o inicial
    private float gameSpeed = 1f; // Velocidade inicial do jogo
    public GameObject StartBackGround;

    void Start()
    {
        gameButtons.SetActive(false);

        OptionsScreen.SetActive(false);

        Player.SetActive(false);

        // Ativa o bot�o start
        ButtonText.SetActive(true);

        // Esconde o Score na tela de titulo
        ScoreTextObj.SetActive(false);

        // Pausa o jogo no in�cio
        Time.timeScale = 0f;

        // Exibe o t�tulo
        TitleTxt.SetActive(true);

        // Esconde o game over
        GameOverTxt.SetActive(false);

        // Exibe o texto de pontua��o inicial (0)
        scoreText.text = "Score: 0";
    }

    void Update()
    {
        if (gameIsPaused && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        else if (!gameIsPaused)
        {
            // Atualiza o contador de pontos
            score += Time.deltaTime * gameSpeed;
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();

            // Aumenta a velocidade do jogo a cada 10 pontos
            if (Mathf.FloorToInt(score) % 10 == 0 && Mathf.FloorToInt(score) > 0)
            {
                IncreaseGameSpeed();
            }
        }
    }

    public void StartGame()
    {
        gameButtons.SetActive(true);
        Player.SetActive(true);
        ButtonText.SetActive(false);
        ScoreTextObj.SetActive(true);
        gameIsPaused = false;
        Time.timeScale = gameSpeed;
        TitleTxt.SetActive(false);// Esconde o t�tulo
        StartBackGround.SetActive(false);
    }

    private void IncreaseGameSpeed()
    {
        gameSpeed =gameSpeed + 0.00015f; // Aumenta a velocidade do jogo mais gradualmente
        Time.timeScale = gameSpeed; // Atualiza a escala de tempo para a nova velocidade
    }

    public void GameOver()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
        GameOverTxt.SetActive(true);// Exibe a mensagem de game over

        // Reinicia a cena ao pressionar "Espa�o"
        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        // Espera at� que o jogador pressione "Espa�o"
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        // Carrega a cena novamente, reiniciando o jogo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartGame();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartGame();
    }

    void OnEnable()
    {
        PlayerController.OnPlayerDeath += AnimationWait; // Subscri��o para evento de morte do jogador
    }

    void OnDisable()
    {
        PlayerController.OnPlayerDeath -= AnimationWait; // Cancelar subscri��o para evento de morte do jogador
    }
    private IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(1);
        GameOver();
    }
    private void AnimationWait()
    {
        StartCoroutine(WaitAnimation());
    }
}
