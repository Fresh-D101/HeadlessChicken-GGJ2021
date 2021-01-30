using UnityEngine;

public class Hammer : MonoBehaviour,IHead
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag(m_ClimbableTag))
        {
            m_Owner.CanClimb = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag(m_ClimbableTag))
        {
            m_Owner.CanClimb = false;
        }
    }

    //////////////////////////////////////////////////////////////////////////

    public GameObject gameObj => gameObject;

    //////////////////////////////////////////////////////////////////////////

    public void OnPickup(Player.PlayerController _owner)
    {
        m_Owner = _owner;
    }

    //////////////////////////////////////////////////////////////////////////

    public void OnDrop(Player.PlayerController _owner)
    {
        m_Owner = null;
    }

    //////////////////////////////////////////////////////////////////////////

    [SerializeField, TagSelector] private string m_ClimbableTag = string.Empty;
    [Separator]
    [SerializeField, ReadOnlyField] private Player.PlayerController m_Owner;
}
