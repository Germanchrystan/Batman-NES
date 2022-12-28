using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    public float speed = 100f;
    public Vector2 direction;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate() 
    {
        Vector2 movement = direction.normalized * speed;
        rigidbody.velocity = movement;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
