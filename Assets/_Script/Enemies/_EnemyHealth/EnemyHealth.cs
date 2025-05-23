using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    private SpriteRenderer renderer;
    // private Animator animator;
    private EnemyHitBox enemyHitBox;

    public Color normalColor;
    public Color hittedColor= new Color(0.8f, 0.3f, 0.05f); // RGBA(0.784, 0.298, 0.047, 1.000)
    
    private bool canGetHit = true;
    public float invisibilityInterval = 0.5f;
    public int currentHealth = 3;
    [SerializeField] private EnemyHealthSO healthSO;
    [SerializeField] private UnityEvent triggerDamageEvent;
    [SerializeField] private UnityEvent triggerDeathEvent;

    void Awake()
    {
        // animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        enemyHitBox = GetComponent<EnemyHitBox>();
        normalColor = renderer.color;
    }
    void Start()
    {
        if(healthSO != null)
        {
            currentHealth = healthSO.healthPoints;
        }
    }
    void OnEnable()
    {
        canGetHit = true;
        currentHealth = healthSO != null ? healthSO.healthPoints : 3;
        enemyHitBox.ActivateHitBoxes();
    }

    void LateUpdate()
    {
        if(!canGetHit && currentHealth > 0)
        {
            // animator.speed = 1;
            renderer.color = hittedColor;
        }
        else if(currentHealth > 0) 
        {
            // animator.speed = 0;
            renderer.color = normalColor;
        }
    }

    public void GetDamage(int damageAmount)
    {
       if (canGetHit)
        {
            currentHealth = currentHealth - damageAmount;
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                enemyHitBox.DeactivateHitBoxes();
                triggerDeathEvent.Invoke();
            }
            else
            {
                triggerDamageEvent.Invoke();
                StartCoroutine(InvisibilityFrame());
            }
        }
    }

    IEnumerator InvisibilityFrame()
    {
        enemyHitBox.DeactivateHitBoxes();
        canGetHit = false;
        yield return new WaitForSeconds(invisibilityInterval);
        if (currentHealth > 0) enemyHitBox.ActivateHitBoxes();
        canGetHit = true;
    }
}
