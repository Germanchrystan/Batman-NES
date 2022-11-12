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

    // private bool facingLeft = true;
    // private int direction = -1;
    public float bulletSpeed = 100f;
    private Rigidbody2D bulletRg;

    void Awake() 
	{
    	rg=GetComponent<Rigidbody2D>();
	    animator=GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
        if (canTriggerShoot)
        {
            StartCoroutine(ShootAndWait());
        } 
        
    }

    void FixedUpdate()
    {
    }
    public void Fire() 
    {
        instantiatedFire = (GameObject) Instantiate(deadshotFire, firePoint.position, firePoint.rotation); 
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
