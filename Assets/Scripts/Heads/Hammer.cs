using UnityEngine;

public class Hammer : Head
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

    [SerializeField, TagSelector] private string m_ClimbableTag = string.Empty;
}
