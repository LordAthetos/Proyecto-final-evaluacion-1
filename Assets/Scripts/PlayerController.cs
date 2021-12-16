using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool gameOver = false;

    private Vector3 initialPos = new Vector3(0, 100, 0);
    public float forwardSpeed = 10f;
    private float horizontalInput;
    private float verticalInput;
    private float turnSpeed = 40f;
    private float gravityModifier = 0;
    private Rigidbody playerRigidbody;

    private AudioSource playerAudioSource;
    private AudioSource cameraAudioSource;

    //Limites del mapa
    private float posXRange = 200f;
    private float negXRange = -200f;
    private float posYRange = 200f;
    private float negYRange = 0f;
    private float posZRange = 200f;
    private float negZRange = -200f;
    private float spawnMargin = 5f;

    private Vector3 randomCooridinates;

    public GameObject recoletable;
    public GameObject obstacle;
    

    private float spawnRate = 5f;

    public int score;

    public AudioClip coinClip;
    public AudioClip explosionClip;

    private float coinNum =10f;

    void Start()
    {
       playerAudioSource = GetComponent<AudioSource>();
       cameraAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
       Physics.gravity *= gravityModifier;
       playerRigidbody = GetComponent<Rigidbody>();
       // Player empieza en la posicion inicial
       transform.position = initialPos;
       
       score = 0;

       // Spawnea 10 recolectables 
       for (float coinInstances = coinNum; coinInstances >= 0; coinInstances -= 1f) 
       {
           randomCooridinates = RandomPosition();
           Instantiate(recoletable, randomCooridinates,recoletable.transform.rotation);
       }

       StartCoroutine("SpawnObstacle");

    }

    void Update()
    {
        
        if (!gameOver)
        {
            // Movimiento hacia delante y controlador de rotacion
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);

            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * horizontalInput);
            transform.Rotate(Vector3.right, turnSpeed * Time.deltaTime * -verticalInput);
        }

        // limites en X
        if (transform.position.x > posXRange)
        {
            transform.position = new Vector3(posXRange, transform.position.y, 
                transform.position.z);
        }
        if (transform.position.x < negXRange)
        {
            transform.position = new Vector3(negXRange, transform.position.y, 
                transform.position.z);
        }

        // limites en Y
        if (transform.position.y > posYRange)
        {
            transform.position = new Vector3(transform.position.x, posYRange, 
                transform.position.z);
        }
        if (transform.position.y < negYRange)
        {
            transform.position = new Vector3(transform.position.x, negYRange, 
                transform.position.z);
        }

        // limites en Z
        if (transform.position.z > posZRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, posZRange);
        }
        if (transform.position.z < negZRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, negZRange);
        }

        // Sonido de disparo
        if (Input.GetKeyDown(KeyCode.RightControl)) 
        {
            playerAudioSource.PlayOneShot(explosionClip, 1);
        }
        

    }

    // Genera una posicion aleatoria dentro del mapa con un margen 
    public Vector3 RandomPosition()
    {
       return new Vector3(Random.Range(negXRange + spawnMargin, posXRange - spawnMargin),
       Random.Range(negYRange + spawnMargin, posYRange - spawnMargin),
       Random.Range(negZRange + spawnMargin, posZRange - spawnMargin));
    }

    // Spawnea un obstaculo cada cierto tiempo
    private IEnumerator SpawnObstacle()
    {
        while(!gameOver)
        {
            yield return new WaitForSeconds(spawnRate);
            randomCooridinates = RandomPosition();
            Instantiate(obstacle, randomCooridinates,recoletable.transform.rotation);
            
        }

    }
    
    // Colisiones
    private void OnTriggerEnter(Collider otherTrigger)
    {
        
        if (!gameOver)       
        {          
            if (otherTrigger.gameObject.CompareTag("Collectible"))
            {
               Destroy(otherTrigger.gameObject);
               score = score + 1;
               playerAudioSource.PlayOneShot(coinClip, 1);

               // El juego se acaba cuando se recogen todos los recolectables
               if (score == coinNum)
               {
                   Debug.Log("Has ganado!");
                   cameraAudioSource.volume = 0.01f;
                   gameOver = true;
               }
               
            }

            else if (otherTrigger.gameObject.CompareTag("Obstacle"))
            {
                // comunicamos que hemos muerto
                Debug.Log("GAME OVER");
                cameraAudioSource.volume = 0.01f;
                gameOver = true;
            }
        }

    }
    
}
