using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heatwave : MonoBehaviour
{
    private Animator animator;

    private bool shouldStartAttack = true;
    private bool shooting = false;

    public float shootingTime = 2f;
    public float waitingTime = 2f;

    public Transform firePoint;
    public GameObject heatwaveFire;
    private GameObject instantiatedFire;

    private EnemyHealth enemyHealth;

    void Awake()
    {
        animator=GetComponent<Animator>();
        enemyHealth=GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldStartAttack && enemyHealth.currentHealth > 0) {
            StartCoroutine(AttackCorroutine());
        }
    }

    void LateUpdate() 
    {
        animator.SetBool("Shooting", shooting);
        animator.SetBool("IsDead", enemyHealth.currentHealth == 0);
    }

    void Fire() 
    {
        instantiatedFire = (GameObject) Instantiate(heatwaveFire, firePoint.position, firePoint.rotation);
    }

    IEnumerator AttackCorroutine()
	{
        shouldStartAttack = false;
        shooting = true;
        Fire();
        yield return new WaitForSeconds(shootingTime);
        shooting = false;
        Destroy(instantiatedFire);
        yield return new WaitForSeconds(waitingTime); 
        shouldStartAttack = true;
	}
}
