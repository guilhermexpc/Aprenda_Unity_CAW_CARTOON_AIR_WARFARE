using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciarInimigo : MonoBehaviour
{
    public GameObject inimigo;

    private void OnBecameVisible()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InitInimigo"))
        {
            Instantiate(inimigo, this.transform.position, this.transform.rotation);          
            Destroy(this.gameObject);
        }
    }
}
