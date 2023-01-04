using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    public float speed = 100f;
    public Vector2 direction;
    
    private float _startingTime;
    public float livingTime = 3f;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        _startingTime = Time.time;
        Destroy(gameObject, livingTime);
    }

    private void FixedUpdate() 
    {
        Vector2 movement = direction.normalized * speed;
        rigidbody.velocity = movement;

    }

}
