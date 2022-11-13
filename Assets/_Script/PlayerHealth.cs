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
        currentHealth = currentHealth - amount;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            animator.SetTrigger("Death");
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
