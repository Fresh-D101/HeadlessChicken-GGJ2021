using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.MovePosition(Time.fixedDeltaTime * speed * dir + transform.position);

        if (dir != Vector3.zero)
        {
            transform.forward = dir;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            var newHead = other.GetComponent<IHead>();
            if (newHead != null)
            {
                PickUpHead(newHead);
                other.enabled = false;
            }
        }
    }

    private void PickUpHead(IHead _head)
    {
        _head.gameObj.transform.position = headPos.position;
        _head.gameObj.gameObject.transform.SetParent(headPos, true);
        head = _head;
        head.OnPickup(this);
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }
    
    public Transform headPos;
    private IHead head;
    public float speed;
    private Rigidbody rb;
}
