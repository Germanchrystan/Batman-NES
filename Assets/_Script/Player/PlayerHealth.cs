using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int totalHealth = 8;
    public int currentHealth;
    public RectTransform healthUI;

    private string currentSceneName;
    private Animator animator;

    //------------------------------------------//
	// Hit animation
	//------------------------------------------//
    private SpriteRenderer renderer;
    public Color hittedColor; // RGBA(0.784, 0.298, 0.047, 1.000)
    public Color normalColor;
    private bool canChangeColor = true;

    private bool canGetHit = true;

    private void Awake()
    {
        animator = gameObject.GetComponentInParent<Animator>();
        currentSceneName = SceneManager.GetActiveScene().name;
        
        renderer = GetComponent<SpriteRenderer>();
        normalColor = renderer.color;
    }

    void LateUpdate()
    {
        if(!canGetHit)
        {
            renderer.color = hittedColor;
            // if(canChangeColor)
            // {
            //     Invoke("SwitchDamageColor", 0.2f);
            // }
        }
        else 
        {
            renderer.color = normalColor;
        }
    }

    private void OnEnable()
    {
        currentHealth = totalHealth;
    }

    public void GetDamage(int amount)
    {
        if (canGetHit)
        {
            currentHealth = currentHealth - amount;
            StartCoroutine(InvisibilityFrame());
            if(currentHealth <= 0)
            {
                currentHealth = 0;
            }
        }
    }

    public void GetHealth()
    {
        currentHealth = currentHealth + 1;

        if (currentHealth > totalHealth)
        {
            currentHealth = totalHealth;
        }
    }

    IEnumerator InvisibilityFrame()
    {
        canGetHit = false;
        yield return new WaitForSeconds(1f);
        canGetHit = true;
    }

    public void DeathEvent()
    {
        StartCoroutine(DeathCorroutine());
    }

    IEnumerator DeathCorroutine()
    {
        Destroy(this.gameObject);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(currentSceneName);
    }
    
    // TODO make this work
    void SwitchDamageColor()
    {
        canChangeColor = false;
        if (renderer.color == hittedColor)
        {
            renderer.color = normalColor;
        }
        else
        {
            renderer.color = hittedColor;
        }
        canChangeColor = true;
    }
}
