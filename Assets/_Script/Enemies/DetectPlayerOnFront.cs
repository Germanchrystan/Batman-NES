using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerOnFront : MonoBehaviour
{
    public Transform frontPlayerCheck;
    public LayerMask playerLayer;

    private float frontPlayerCheckRadius = 5f;
    
    void Awake()
    {
        frontPlayerCheck=gameObject.transform.Find("FrontPlayerCheck");
    }

    public bool DetectPlayer()
    {
        return Physics2D.OverlapCircle(frontPlayerCheck.position, frontPlayerCheckRadius, playerLayer);
    }
}
