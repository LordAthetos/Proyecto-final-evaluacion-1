using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
  // Detectar colisiones 
  private void OnTriggerEnter(Collider otherTrigger)
    {
        Destroy(otherTrigger.gameObject); 
        Destroy(gameObject); 
    }
}
