using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    private PlayerController _PlayerController;

    public Transform tfArma;

    // Start is called before the first frame update
    void Start()
    {
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }

    // Update is called once per frame
    void Update()
    {
        tfArma.right = _PlayerController.transform.position - transform.position;
    }
}
