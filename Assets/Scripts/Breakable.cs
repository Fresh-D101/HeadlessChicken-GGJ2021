using UnityEngine;

public class Breakable : MonoBehaviour
{
    void Start()
    {
        m_Particles = GetComponentsInChildren<Rigidbody>();
    }
    
    //////////////////////////////////////////////////////////////////////////
    
    public void Break()
    {
        GetComponent<Collider>().enabled = false;
        transform.DetachChildren();
        Destroy(gameObject);
 
        foreach (var fragment in m_Particles)
        {
            fragment.isKinematic = false;
        }
    }
    
    //////////////////////////////////////////////////////////////////////////
    
    private Rigidbody[] m_Particles;
}
