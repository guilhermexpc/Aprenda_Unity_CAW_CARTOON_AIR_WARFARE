using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public GameObject prefabShot;
    public Transform armaPosicao;
    public float velocidade;
    public float velocidadeTiro;
    public float fireRate;
    private float nextShot;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
        GameObject shotTemp2 = Instantiate(prefabShot);
        GameObject shotTemp3 = Instantiate(prefabShot);
        shotTemp.transform.position = armaPosicao.position;
        shotTemp2.transform.position = armaPosicao.position + new Vector3(-0.2f, -0.25f);
        shotTemp3.transform.position = armaPosicao.position + new Vector3(0.2f, -0.25f);
        shotTemp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocidadeTiro);
        shotTemp2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocidadeTiro);
        shotTemp3.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocidadeTiro);
    }
}
