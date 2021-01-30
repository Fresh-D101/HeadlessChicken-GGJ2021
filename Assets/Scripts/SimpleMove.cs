using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    //////////////////////////////////////////////////////////////////////////

    private void Update()
    {
        if (canClimb)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Rigidbody.AddForce(Vector3.up * 2000);
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////

    void FixedUpdate()
    {
        //TODO: Move input to Update
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_Rigidbody.MovePosition(Time.fixedDeltaTime * m_Speed * dir + transform.position);

        if (dir != Vector3.zero)
        {
            transform.forward = dir;
        }
    }

    //////////////////////////////////////////////////////////////////////////

    private void OnTriggerStay(Collider _other)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            var newHead = _other.GetComponent<IHead>();
            if (newHead != null)
            {
                PickUpHead(newHead);
                _other.enabled = false;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////

    private void PickUpHead(IHead _head)
    {
        _head.gameObj.transform.SetPositionAndRotation(m_HeadPos.position, m_HeadPos.rotation);
        _head.gameObj.gameObject.transform.SetParent(m_HeadPos);
        m_Head = _head;
        //m_Head.OnPickup(this);
    }

    //////////////////////////////////////////////////////////////////////////

    public void SetSpeed(float _speed)
    {
        m_Speed = _speed;
    }

    //////////////////////////////////////////////////////////////////////////

    public float Speed => m_Speed;

    //////////////////////////////////////////////////////////////////////////

    public bool canClimb;
    [SerializeField] private Transform m_HeadPos;
    [SerializeField] private float m_Speed;

    private IHead m_Head;
    private Rigidbody m_Rigidbody;
}
