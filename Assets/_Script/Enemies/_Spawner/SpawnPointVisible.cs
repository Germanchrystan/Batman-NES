using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnPointVisible : MonoBehaviour
{
    [SerializeField] private UnityEvent SpawnPointVisibleEvent;
    [SerializeField] private UnityEvent SpawnPointInvisibleEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("VisibleSpace"))
        {
           SpawnPointVisibleEvent.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("VisibleSpace"))
        {
            SpawnPointInvisibleEvent.Invoke();
        }
    }
}
