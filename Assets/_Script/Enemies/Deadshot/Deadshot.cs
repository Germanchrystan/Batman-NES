using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadshot : MonoBehaviour
{
    private Rigidbody2D rg;
	private Animator animator;
    public Transform firePoint;
    private bool canTriggerShoot = true;
    public float waitTime = 2f;

    public GameObject deadshotFire;
    private GameObject instantiatedFire;

    private bool facingLeft = true;
    // private int direction = -1;
    public float bulletSpeed = 100f;
    private Rigidbody2D bulletRg;

    private EnemyHealth enemyHealth;

    void Awake() 
	{
    	rg=GetComponent<Rigidbody2D>();
	    animator=GetComponent<Animator>();
        enemyHealth=GetComponent<EnemyHealth>();

	}

    // Update is called once per frame
    void Update()
    {
        if (canTriggerShoot && enemyHealth.currentHealth > 0)
        {
            StartCoroutine(ShootAndWait());
        } 
        
    }

    void LateUpdate()
    {
        animator.SetBool("IsDead", enemyHealth.currentHealth == 0);
    }

    public void Fire() 
    {
        instantiatedFire = (GameObject) Instantiate(deadshotFire, firePoint.position, firePoint.rotation); 
        BulletMovement bulletMovement = instantiatedFire.GetComponent<BulletMovement>();
        if (facingLeft)
        {
            bulletMovement.direction = Vector2.left;
        } else 
        {
            bulletMovement.direction = Vector2.right;
        }
    }

    IEnumerator ShootAndWait()
	{
        canTriggerShoot = false;
		animator.SetTrigger("Shoot");
        Destroy(instantiatedFire);
		yield return new WaitForSeconds(waitTime);
        Destroy(instantiatedFire);
		canTriggerShoot = true;
	}
}
