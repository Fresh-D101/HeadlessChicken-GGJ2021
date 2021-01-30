using UnityEngine;

public class Hammer : Head
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag(m_ClimbableTag))
        {
            m_Owner.CanClimb = true;
            m_Owner.PreventFalling = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag(m_ClimbableTag) && !ReferenceEquals(m_Owner, null))
        {
            m_Owner.CanClimb = false;
            m_Owner.PreventFalling = false;
        }
    }

    //////////////////////////////////////////////////////////////////////////

    [SerializeField, TagSelector] private string m_ClimbableTag = string.Empty;
}