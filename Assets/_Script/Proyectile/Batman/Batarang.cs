using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batarang : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    public float speed = 150f;
    public Vector2 direction;
    private Vector2 movement; 
    
    private float _startingTime;
    public float livingTime = 1f;

    public bool returning = false;
    public Transform returnPoint;
 

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        _startingTime = Time.time;
        Invoke("Return", livingTime);
    }

    private void FixedUpdate() 
    {
        if (!returning)
        {
            movement = direction.normalized * speed;
        }
        else 
        {
            direction = new Vector2(returnPoint.position.x - transform.position.x, returnPoint.position.y - transform.position.y);
        
        }
            movement = direction.normalized * speed;
            rigidbody.velocity = movement;

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(returning && collision.transform.gameObject.layer == 7) // If the batarang is returning and collides with an object with the "PlayerHitBox"
        {
            Destroy(this.gameObject);
        }
    }

    private void Return()
    {
        returning = true;
    }
    
}
