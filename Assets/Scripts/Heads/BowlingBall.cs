using UnityEngine;

public class BowlingBall : MonoBehaviour, IHead
{
    public void OnPickup(SimpleMove _owner)
    {
        m_OriginalSpeed = _owner.Speed;
        _owner.SetSpeed(4);
    }

    //////////////////////////////////////////////////////////////////////////
    
    public void OnDrop(SimpleMove _owner)
    {
        _owner.SetSpeed(m_OriginalSpeed);
    }

    //////////////////////////////////////////////////////////////////////////
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Breakable"))
        {
            other.GetComponent<Breakable>()?.Break();
        }
    }

    //////////////////////////////////////////////////////////////////////////
    
    public GameObject gameObj => gameObject;

    //////////////////////////////////////////////////////////////////////////
    
    private float m_OriginalSpeed;
    private SimpleMove m_Owner;
}
