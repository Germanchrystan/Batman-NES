using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batarang : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    public float speed = 100f;
    public Vector2 direction;
    
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
            Vector2 movement = direction.normalized * speed;
            rigidbody.velocity = movement;
        }
        else 
        {
            direction = new Vector2(returnPoint.position.x - transform.position.x, returnPoint.position.y - transform.position.y);
            Vector2 movement = direction.normalized * speed;
            rigidbody.velocity = movement;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(returning)
        {
            Destroy(this.gameObject);
        }
    }
    private void Return()
    {
        returning = true;
    }
    
}
