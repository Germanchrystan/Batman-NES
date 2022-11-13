using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int totalHealth = 8;
    private int currentHealth;
    public RectTransform healthUI;

    private string currentSceneName;
    private Animator animator;

    private bool canGetHit = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        currentSceneName = SceneManager.GetActiveScene().name;
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
                Debug.Log("DEAD");
                currentHealth = 0;
                // animator.SetTrigger("Death");
            }
            Debug.Log("Hitted" + currentHealth);
        }
    }

    public void AddHealth()
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
}
