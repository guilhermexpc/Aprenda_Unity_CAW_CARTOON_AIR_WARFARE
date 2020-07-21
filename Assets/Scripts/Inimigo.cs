using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public GameController _GameController;

    public PlayerController _PlayerController;

    public int score;
    public Transform tfArma;
    private bool isVivo;
    public bool insideCenario;


    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        isVivo = true;
        if (insideCenario)
        {
            this.transform.parent = _GameController.cenario.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnHit()
    {
        if (isVivo)
        {
            isVivo = false;
            _GameController.GameScore(score);
            Instantiate(_GameController.explosao, this.transform.position, _GameController.explosao.transform.rotation);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "playerShot":
                {
                    OnHit();
                    Destroy(collision.gameObject);
                    break;
                }

            case "Player":
                {
                    OnHit();
                    break;
                }
            default:
                break;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
