using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController _GameController;
    private Rigidbody2D playerRb;

    public GameObject prefabShot;
    public Transform armaPosicao;
    public SpriteRenderer sprNave;
    public SpriteRenderer sprFumaca;

    [Header("Atributos")]
    public float velocidade;
    public float velocidadeTiro;
    public float fireRate;
    public int vida;

    public bool IsVivo { get; set; }
    public bool IsInvulneravel { get; set; }

    private float nextShot;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_GameController.currentState != GameState.gameplay)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        playerRb.velocity = new Vector2(horizontal * velocidade, vertical * velocidade);

        if (Input.GetButton("Fire1") && Time.time > nextShot)
        {
            nextShot = fireRate + Time.time;
            Shot();
        }
    }

    private void Shot()
    {
        GameObject shotTemp = Instantiate(prefabShot);
        //GameObject shotTemp2 = Instantiate(prefabShot);
        //GameObject shotTemp3 = Instantiate(prefabShot);
        shotTemp.transform.position = armaPosicao.position;
        //shotTemp2.transform.position = armaPosicao.position + new Vector3(-0.2f, -0.25f);
        //shotTemp3.transform.position = armaPosicao.position + new Vector3(0.2f, -0.25f);
        shotTemp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocidadeTiro);
        //shotTemp2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocidadeTiro);
        //shotTemp3.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocidadeTiro);
    }

    public void OnHit()
    {
        if (IsVivo && !IsInvulneravel)
        {
            IsInvulneravel = true;
            vida--;
            vida = vida <= 0 ? 0 : vida;
            _GameController.textPlayerVida.text = vida.ToString();

            if (vida > 0)
            {
                StartCoroutine(OnHitEfeito());        
            }
            else
            {
                // Player Morreu // Chamar GameOver
                IsVivo = false;
                IsInvulneravel = true;
                _GameController.currentState = GameState.gameover;
                Instantiate(_GameController.explosao, this.transform.position, _GameController.explosao.transform.rotation);
                vida = 0;
                _GameController.GameOver();
                this.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator OnHitEfeito()
    {
        for (int i = 0; i < 20; i++)
        {
            sprNave.color = Color.clear;
            sprFumaca.color = Color.clear;
            yield return new WaitForSeconds(0.05f);
            sprNave.color = Color.white;
            sprFumaca.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }

        sprNave.color = Color.white;
        sprFumaca.color = Color.white;
        yield return new WaitForEndOfFrame();
        IsInvulneravel = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "enemyShot":
                {
                    OnHit();
                    break;
                }
            case "enemy":
                {
                    OnHit();
                    break;
                }
            default:
                break;
        }
    }
}
