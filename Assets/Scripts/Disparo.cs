using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo : MonoBehaviour
{
    public GameObject projectilePrefab;
    void Update()
    {
       // Disparo
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            
            Instantiate(projectilePrefab, transform.position,
                projectilePrefab.transform.rotation = transform.rotation);
        } 
    }
}
