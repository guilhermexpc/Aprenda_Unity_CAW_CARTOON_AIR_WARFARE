using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public enum GameState
{
    intro,
    gameplay,
    gameover,
    vitoria
}

public class GameController : MonoBehaviour
{
    public GameState currentState;

    private PlayerController _PlayerController;

    [Header("Variavel Controle")]
    public int gameScore;

    [Header("Limite Movimento")]
    public Transform limiteTop;
    public Transform limiteBot;
    public Transform limiteLeft;
    public Transform limiteRight;

    [Header("Limite Lateral Camera")]
    public Camera cam;
    public float cameraVelocidade;
    public Transform cameraLimiteLeft;
    public Transform cameraLimiteRight;
    public Transform cameraPosFinal;
    public bool enableCameraMovimentoVertical;
    public float cameraVelocidadeFase;

    [Header("Cenario")]
    public Transform cenario;
    public Transform cenarioPosFinal;
    public float cenarioVelocidade;

    [Header("Intro")]
    private bool isDecolar;
    public float tamanhoInicialNave;
    public float tamanhoOriginal;
    public float velocidadeDecolagem;
    private float velocidadeDecolagemAtual;
    public Transform posicaoInicialNave;
    public Transform posicaoDecolagem;
    public Color corFumacaInicial;
    public Color corFumacaFinal;

    [Header("Prefab")]
    public GameObject explosao;

    [Header("Inimigo")]
    public GameObject iniTiroPadrao;

    [Header("HUD")]
    public Text textPlayerVida;
    public Text textPlayerScore;
    public Text textGameVitoria;
    public Text textGameOver;

    private void Awake()
    {
        textGameVitoria.gameObject.SetActive(false);
        textGameOver.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        _PlayerController.sprFumaca.color = Color.Lerp(corFumacaInicial, corFumacaFinal, 0.1f);
        Init();
        StartCoroutine(IntroFase());
    }

    private void Init()
    {
        textPlayerVida.text = _PlayerController.vida.ToString();
        textPlayerScore.text = "0";
  
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isDecolar)
        {
            _PlayerController.transform.position = Vector3.MoveTowards(_PlayerController.transform.position, posicaoDecolagem.position, velocidadeDecolagem * Time.deltaTime);
        }

        if (isDecolar && _PlayerController.transform.position == posicaoDecolagem.position)
        {
            isDecolar = false;
            StartCoroutine(IntroSubir());
        }

        PlayerLimitarMovimento();
    }       

    private void LateUpdate()
    {
        if (currentState == GameState.gameplay)
        {
            cenario.position = Vector3.MoveTowards(cenario.position, new Vector3(cenario.position.x, cenarioPosFinal.position.y, 0), cenarioVelocidade * Time.deltaTime);
            if (cenario.position.y <= cenarioPosFinal.position.y)
            {
                currentState = GameState.vitoria;
                StartCoroutine(GameVitoria());
            }
        }

        if (enableCameraMovimentoVertical)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(cam.transform.position.x, cameraPosFinal.position.y, -10), cameraVelocidadeFase * Time.deltaTime);
            if (cam.transform.position.x > cameraLimiteLeft.position.x && cam.transform.position.x < cameraLimiteRight.position.x)
            {
                CameraMover();
            }
            else if (cam.transform.position.x <= cameraLimiteLeft.position.x && _PlayerController.transform.position.x > cameraLimiteLeft.position.x)
            {
                CameraMover();
            }
            else if (cam.transform.position.x >= cameraLimiteRight.position.x && _PlayerController.transform.position.x < cameraLimiteRight.position.x)
            {
                CameraMover();
            }
        }
    }

    private void PlayerLimitarMovimento()
    {
        float posX = _PlayerController.transform.position.x;
        float posY = _PlayerController.transform.position.y;
        if (!isDecolar)
            _PlayerController.transform.position = new Vector3(Mathf.Clamp(posX, limiteLeft.position.x, limiteRight.position.x),
                                                            Mathf.Clamp(posY, limiteBot.position.y, limiteTop.position.y), 0);

        //if (posX > limiteRight.position.x)
        //{
        //    _PlayerController.transform.position = new Vector3(limiteRight.position.x, posY, 0);
        //}
        //else if (posX < limiteLeft.position.x)
        //{
        //    _PlayerController.transform.position = new Vector3(limiteLeft.position.x, posY, 0);
        //}

        //if (posY > limiteTop.position.y)
        //{
        //    _PlayerController.transform.position = new Vector3(posX, limiteTop.position.y, 0);
        //}
        //else if (posY < limiteBot.position.y)
        //{
        //    _PlayerController.transform.position = new Vector3(posX, limiteBot.position.y, 0);
        //}
    }

    private void CameraMover()
    {
        Vector3 posicaoDestinoCamera = new Vector3(_PlayerController.transform.position.x, cam.transform.position.y, -10);
        cam.transform.position = Vector3.Lerp(cam.transform.position, posicaoDestinoCamera, cameraVelocidade * Time.deltaTime);
    }

    public void InitPlayer()
    {
        _PlayerController.IsInvulneravel = false;
        _PlayerController.IsVivo = true;
    }

    public void GameScore(int score)
    {
        gameScore += score;
        textPlayerScore.text = gameScore.ToString();
    }

    public void GameOver()
    {
        currentState = GameState.gameover;
        StartCoroutine(GameOverEfeito());
    }

    IEnumerator GameVitoria()
    {
        textGameVitoria.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Scene_Titulo");
    }

    IEnumerator GameOverEfeito()
    {
        textGameOver.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Scene_GameOver");
    }

    IEnumerator IntroFase()
    {
        _PlayerController.transform.localScale = new Vector3(tamanhoInicialNave, tamanhoInicialNave, tamanhoInicialNave);
        _PlayerController.transform.position = posicaoInicialNave.position;

        isDecolar = true;

        for (velocidadeDecolagemAtual = 0; velocidadeDecolagemAtual < velocidadeDecolagem; velocidadeDecolagemAtual += 0.01f)
        {
            yield return new WaitForEndOfFrame();
        }
        velocidadeDecolagemAtual = velocidadeDecolagem;
    }

    IEnumerator IntroSubir()
    {
        currentState = GameState.gameplay;
        for (float s = tamanhoInicialNave; s < tamanhoOriginal; s+= 0.01f)
        {
            _PlayerController.transform.localScale = new Vector3(s, s, s);
            _PlayerController.sprFumaca.color = Color.Lerp(corFumacaInicial, corFumacaFinal, s);
            _PlayerController.transform.position = new Vector3(_PlayerController.transform.position.x, _PlayerController.transform.position.y + 0.01f, _PlayerController.transform.position.z);
            yield return new WaitForEndOfFrame();
        }

        _PlayerController.transform.localScale = new Vector3(tamanhoOriginal, tamanhoOriginal, tamanhoOriginal);
        _PlayerController.sprFumaca.color = Color.Lerp(corFumacaInicial, corFumacaFinal, 1);
        InitPlayer();
    }

}
