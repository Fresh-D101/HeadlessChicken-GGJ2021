using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    void Start()
    {
        particles = GetComponentsInChildren<Rigidbody>();
    }
    
    public void Break()
    {
        GetComponent<Collider>().enabled = false;
        transform.DetachChildren();
        Destroy(gameObject);
 
        foreach (var fragment in particles)
        {
            fragment.isKinematic = false;
        }
    }
    
    private Rigidbody[] particles;
}
