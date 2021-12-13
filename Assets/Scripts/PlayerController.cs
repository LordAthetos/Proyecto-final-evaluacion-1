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
    public GameObject projectilePrefab;

    private float spawnRate = 5f;

    private int score;


    // Start is called before the first frame update
    void Start()
    {

       // Player empieza en la posicion inicial
       transform.position = initialPos;
       
       score = 0;

       // Spawnea 10 recolectables 
       for (float coinInstances = 10f; coinInstances >= 0; coinInstances -= 1f) 
       {
           randomCooridinates = RandomPosition();
           Instantiate(recoletable, randomCooridinates,recoletable.transform.rotation);
       }

       StartCoroutine("SpawnObstacle");

    }

    void Update()
    {

        // Movimiento hacia delante y controlador de rotacion
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);

        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * horizontalInput);
        transform.Rotate(Vector3.right, turnSpeed * Time.deltaTime * -verticalInput);

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

        // Disparo
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            
            Instantiate(projectilePrefab, transform.position,
                projectilePrefab.transform.rotation = transform.rotation);
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

     private void OnCollisionEnter(Collision otherCollider)
    {
        if (!gameOver)
        {
            if (otherCollider.gameObject.CompareTag("Collectible"))
            {
               
            }
            else if (otherCollider.gameObject.CompareTag("Obstacle"))
            {

                gameOver = true;
            }
        }

    }

}
