using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{

    private PlayerController _PlayerController;

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
    public float cameraVelocidadeFase;

    

    // Start is called before the first frame update
    void Start()
    {
        _PlayerController = FindObjectOfType(typeof(PlayerController)) as PlayerController ;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerLimitarMovimento();        
    }

    private void PlayerLimitarMovimento()
    {
        float posX = _PlayerController.transform.position.x;
        float posY = _PlayerController.transform.position.y;

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

        _PlayerController.transform.position = new Vector3(Mathf.Clamp(posX, limiteLeft.position.x, limiteRight.position.x),
                                                            Mathf.Clamp(posY, limiteBot.position.y, limiteTop.position.y), 0);
    }

    private void LateUpdate()
    {

        cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(cam.transform.position.x, cameraPosFinal.position.y, -10), cameraVelocidadeFase * Time.deltaTime);

        if (cam.transform.position.x > cameraLimiteLeft.position.x && cam.transform.position.x < cameraLimiteRight.position.x)
        {
            CameraMover();
        }
        else if (cam.transform.position.x <= cameraLimiteLeft.position.x && _PlayerController.transform.position.x > cameraLimiteLeft.position.x)
        {
            Debug.Log("foi");
            CameraMover();
        }
        else if (cam.transform.position.x >= cameraLimiteRight.position.x && _PlayerController.transform.position.x < cameraLimiteRight.position.x)
        {
            Debug.Log("foi2");
            CameraMover();
        }
    }

    private void CameraMover()
    {
        Vector3 posicaoDestinoCamera = new Vector3(_PlayerController.transform.position.x, cam.transform.position.y, -10);
        cam.transform.position = Vector3.Lerp(cam.transform.position, posicaoDestinoCamera, cameraVelocidade * Time.deltaTime);
    }
}
