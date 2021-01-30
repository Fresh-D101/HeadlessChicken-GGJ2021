using UnityEngine;

public class BowlingBall : MonoBehaviour, IHead
{
    public void OnPickup(Player.PlayerController _owner)
    {
        m_Owner = _owner;
        m_OriginalSpeed = m_Owner.MaximumVelocity;
        m_Owner.MaximumVelocity = m_NewSpeed;
    }

    //////////////////////////////////////////////////////////////////////////

    public void OnDrop(Player.PlayerController _owner)
    {
        m_Owner.MaximumVelocity = m_OriginalSpeed;
        m_Owner = null;
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

    [SerializeField] private float m_NewSpeed = 0;
    [Separator]
    [SerializeField, ReadOnlyField] private Player.PlayerController m_Owner = null;
    [SerializeField, ReadOnlyField] private float m_OriginalSpeed = 0;
}
