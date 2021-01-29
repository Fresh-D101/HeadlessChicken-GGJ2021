using UnityEngine;

public class Hammer : MonoBehaviour,IHead
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Climbable"))
        {
            m_Owner.canClimb = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("Climbable"))
        {
            m_Owner.canClimb = false;
        }
    }
    
    //////////////////////////////////////////////////////////////////////////
    
    public GameObject gameObj => gameObject;
    
    //////////////////////////////////////////////////////////////////////////

    public void OnPickup(SimpleMove _owner)
    {
        m_Owner = _owner;
    }
    
    //////////////////////////////////////////////////////////////////////////

    public void OnDrop(SimpleMove _owner)
    {
        m_Owner = null;
    }
    
    //////////////////////////////////////////////////////////////////////////
    
    private SimpleMove m_Owner;
}
