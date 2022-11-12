using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heatwave : MonoBehaviour
{
    private Animator animator;

    private bool shouldStartAttack = true;
    private bool shooting = false;

    public float shootingTime = 1f;
    public float waitingTime = 2f;

    public Transform firePoint;
    public GameObject heatwaveFire;
    private GameObject instantiatedFire;

    void Awake()
    {
        animator=GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldStartAttack) {
            StartCoroutine(AttackCorroutine());
        }
    }

    void LateUpdate() 
    {
        animator.SetBool("Shooting", shooting);
    }

    void Fire() 
    {
        instantiatedFire = (GameObject) Instantiate(heatwaveFire, firePoint.position, firePoint.rotation);
    }

    IEnumerator AttackCorroutine()
	{
        shouldStartAttack = false;
        shooting = true;
        yield return new WaitForSeconds(shootingTime);
        shooting = false;
        Destroy(instantiatedFire);
        yield return new WaitForSeconds(waitingTime); 
        shouldStartAttack = true;
	}
}
