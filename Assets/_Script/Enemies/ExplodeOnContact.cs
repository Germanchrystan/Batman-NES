using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnContact : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform playerTransform = collision.gameObject.transform;
        Vector2 playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y); 
        
        if(collision.CompareTag("Player"))
        {
            gameObject.SendMessageUpwards("Explode", playerPosition);
        }
    }
}
