using UnityEngine;

public class Breakable : MonoBehaviour
{
    void Start()
    {
        m_Particles = GetComponentsInChildren<Rigidbody>();
    }
    
    //////////////////////////////////////////////////////////////////////////
    
    public void Break(Vector3 _pos, float _force)
    {
        GetComponent<Collider>().enabled = false;
        transform.DetachChildren();
        Destroy(gameObject);
 
        foreach (var fragment in m_Particles)
        {
            fragment.isKinematic = false;
            fragment.AddExplosionForce(_force, _pos, 10);
        }
    }
    
    //////////////////////////////////////////////////////////////////////////
    
    private Rigidbody[] m_Particles;
}
