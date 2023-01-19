using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleBullet : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    public Vector2 direction;
    private float distance = 40f;

    private bool firstStageMovement = true;
    private float firstStageDuration = .5f;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float elapsedTime;

    public float angle;
    private Vector2 secondStageVector;
    private Vector2 secondStageDirection;
    private float speed = 100f;

    [SerializeField]
    private AnimationCurve curve;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector2(startPosition.x + (distance * direction.x), startPosition.y);
        // Invoke("changeDirection", firstStageDuration);
    }

    void Update()
    {
        if(transform.position.x == endPosition.x)
        {
            changeDirection();
        }
    }

    private void FixedUpdate()
    {
        if(firstStageMovement)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / firstStageDuration;

            Vector2 movement = Vector2.Lerp(startPosition, endPosition, curve.Evaluate(percentageComplete));
            transform.position = movement;
        }
        else
        {
            secondStageVector = DegreeToVector2(angle);
            secondStageDirection = new Vector2(direction.x * secondStageVector.x, secondStageVector.y).normalized * speed;
            rigidbody.velocity = secondStageDirection;

        }
    }

    void changeDirection()
    {
        firstStageMovement = false;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //==============================================================//
    // Totally did not steal this
    //==============================================================//
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
      
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
}
