using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    private const string ENEMY_HIT_BOX_TAG = "EnemyHitBox";
    
    public void DeactivateHitBoxes()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            if(child.transform.CompareTag(ENEMY_HIT_BOX_TAG)) {
                child.SetActive(false);
            }
        }
    }

    public void ActivateHitBoxes()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            if(child.transform.CompareTag(ENEMY_HIT_BOX_TAG)) {
                child.SetActive(true);
            }
        }
    }
}