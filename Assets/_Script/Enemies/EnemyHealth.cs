using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private SpriteRenderer renderer;
    private Animator animator;
    private EnemyHitBox enemyHitBox;

    public Color normalColor;
    public Color hittedColor= new Color(0.8f, 0.3f, 0.05f); // RGBA(0.784, 0.298, 0.047, 1.000)
    
    private bool canGetHit = true;
    public float invisibilityInterval = 0.5f;
    public int currentHealth = 3;

    void Awake()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        enemyHitBox = GetComponent<EnemyHitBox>();
        normalColor = renderer.color;
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
            StartCoroutine(InvisibilityFrame());
            if(currentHealth <= 0)
            {
                currentHealth = 0;
            }
        }
    }

    IEnumerator InvisibilityFrame()
    {
        enemyHitBox.DeactivateHitBoxes();
        canGetHit = false;
        yield return new WaitForSeconds(invisibilityInterval);
        enemyHitBox.ActivateHitBoxes();
        canGetHit = true;
    }
}
