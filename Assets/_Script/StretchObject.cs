using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchObject : MonoBehaviour
{
    public GameObject StartObject;
    public GameObject EndObject;
    private Vector3 InitialScale;

    void Start()
    {
        InitialScale = transform.localScale;
        UpdateTransformForScale();        
    }

    // Update is called once per frame
    void Update()
    {
        if(StartObject.transform.hasChanged || EndObject.transform.hasChanged)
        {
            UpdateTransformForScale();
        }
    }

    void UpdateTransformForScale()
    {
        float distance = StartObject.transform.position.x - EndObject.transform.position.x;
        transform.localScale = new Vector3(distance/5, 1, 0);

        Vector3 middlePoint = (StartObject.transform.position + EndObject.transform.position) / 2;
        transform.position = middlePoint;

    }
}
