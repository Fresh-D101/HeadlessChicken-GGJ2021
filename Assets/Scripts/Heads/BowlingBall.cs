using UnityEngine;

public class BowlingBall : Head
{
    protected override void  OnPickup()
    {
        m_OriginalSpeed = m_Owner.MaximumVelocity;
        m_Owner.MaximumVelocity = m_NewSpeed;
    }

    //////////////////////////////////////////////////////////////////////////

    protected override void OnDrop()
    {
        m_Owner.MaximumVelocity = m_OriginalSpeed;
    }

    //////////////////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {
        if (m_Owner is null) return;
        
        if (other.CompareTag("Breakable"))
        {
            other.GetComponent<Breakable>()?.Break();
        }
    }

    //////////////////////////////////////////////////////////////////////////

    public GameObject gameObj => gameObject;

    //////////////////////////////////////////////////////////////////////////

    [SerializeField] private float m_NewSpeed = 0;
    [Separator]
    [SerializeField, ReadOnlyField] private float m_OriginalSpeed = 0;
}
