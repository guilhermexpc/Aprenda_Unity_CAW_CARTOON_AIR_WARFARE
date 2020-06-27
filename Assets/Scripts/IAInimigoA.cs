using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InimigoAtaque
{
    none,
    linear,
    follow
}

public class IAInimigoA : MonoBehaviour
{
    private Inimigo _Inimigo;
    private PlayerController _PlayerController;

    public InimigoAtaque inimigoAtaque;

    public Transform tfArma;
    public float armaVelocidade;
    public float armaDelay;
    public int idCurva;
    public float velocidadeMovimento;
    public float pontoInicialCurva;   
    public float grausCurva;
    public float incrementar;

    private float rotacaoZ;
    private bool isCurva;
    private float incrementado;
    private bool enableMovimento;


    private void Awake()
    {
        _Inimigo = GetComponent<Inimigo>();
        enableMovimento = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _PlayerController = _Inimigo._PlayerController;
        rotacaoZ = transform.eulerAngles.z;
        StartCoroutine(Ataque());
    }

    // Update is called once per frame
    void Update()
    {
        if (!enableMovimento)
            return;
        MovimentoCurva();       
        
        if (inimigoAtaque == InimigoAtaque.follow)
            tfArma.right = _PlayerController.transform.position - transform.position;
        else
            tfArma.right = Vector3.down;

    }

    public void Atirar()
    {
            
    }
   
    private void MovimentoCurva()
    {
        // Curva X Esquerda pra Direita
        if (idCurva == 1)
        {
            if (transform.position.x >= pontoInicialCurva && !isCurva)
            {
                isCurva = true;
            }

            if (isCurva && incrementado < grausCurva)
            {
                rotacaoZ += incrementar;
                transform.rotation = Quaternion.Euler(0, 0, rotacaoZ);

                if (incrementar < 0)
                {
                    incrementado += (incrementar * -1);
                }
                else
                {
                    incrementado += incrementar;
                }
            }
        }

        // Curva X Direita pra Esquerda
        if (idCurva == 2)
        {
            if (transform.position.x <= pontoInicialCurva && !isCurva)
            {
                isCurva = true;
            }

            if (isCurva && incrementado < grausCurva)
            {
                rotacaoZ += incrementar;
                transform.rotation = Quaternion.Euler(0, 0, rotacaoZ);

                if (incrementar < 0)
                {
                    incrementado += (incrementar * -1);
                }
                else
                {
                    incrementado += incrementar;
                }
            }
        }

        // Curva Y Cima pra Baixo
        if (idCurva == 3)
        {
            if (transform.position.y <= pontoInicialCurva && !isCurva)
            {
                isCurva = true;
            }

            if (isCurva && incrementado < grausCurva)
            {
                rotacaoZ += incrementar;
                transform.rotation = Quaternion.Euler(0, 0, rotacaoZ);

                if (incrementar < 0)
                {
                    incrementado += (incrementar * -1);
                }
                else
                {
                    incrementado += incrementar;
                }
            }
        }

        // Curva Y Baixo pra Cima
        if (idCurva == 4)
        {
            if (transform.position.y >= pontoInicialCurva && !isCurva)
            {
                isCurva = true;
            }

            if (isCurva && incrementado < grausCurva)
            {
                rotacaoZ += incrementar;
                transform.rotation = Quaternion.Euler(0, 0, rotacaoZ);

                if (incrementar < 0)
                {
                    incrementado += (incrementar * -1);
                }
                else
                {
                    incrementado += incrementar;
                }
            }
        }
        transform.Translate(Vector3.down * velocidadeMovimento * Time.deltaTime);
    }

    IEnumerator Ataque()
    {
        if (inimigoAtaque == InimigoAtaque.none)
            yield break;
        yield return new WaitForSeconds(armaDelay);
        GameObject tiroTemp = Instantiate(_Inimigo._GameController.iniTiroPadrao, tfArma.position, tfArma.transform.rotation);
        tiroTemp.GetComponent<Rigidbody2D>().velocity = tfArma.right * armaVelocidade;
        tiroTemp.transform.rotation = tfArma.rotation;

        StartCoroutine(Ataque());
    }
}
