using Player;
using UnityEngine;
public abstract class Head : MonoBehaviour
{
    protected virtual void OnPickup() {}
    protected virtual void OnDrop() {}

    //////////////////////////////////////////////////////////////////////////

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    //////////////////////////////////////////////////////////////////////////

    public void OnPickup(PlayerController _owner, Transform _headPos)
    {
        m_Owner = _owner;
        transform.SetPositionAndRotation(_headPos.position, _headPos.rotation);
        transform.SetParent(_headPos);
        gameObject.layer = LayerMask.NameToLayer("AttachedHead");
        m_Rigidbody.isKinematic = true;
        OnPickup();
    }

    //////////////////////////////////////////////////////////////////////////

    public void OnDrop(PlayerController _owner)
    {
        OnDrop();
        gameObject.layer = LayerMask.NameToLayer("Head");
        transform.parent = null;
        transform.position += transform.up;
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.velocity = (transform.forward + transform.up) * m_DropForce;
        m_Owner = null;
    }

    //////////////////////////////////////////////////////////////////////////

    protected PlayerController m_Owner;
    
    [SerializeField] private float m_DropForce;
    private Rigidbody m_Rigidbody;
}
