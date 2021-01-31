using System.Collections;
using UnityEngine;

public class Hook : Head
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(m_ZiplineTag) && !m_Owner.IsGrounded && !m_IsSliding)
        {
            var zipline = other.GetComponentInParent<Zipline>();
            StartCoroutine(SlideDown(zipline.Anchor, zipline.Start.position, zipline.Stop.position));
            m_Owner.BlockInputs = true;
        }       
    }
    
    //////////////////////////////////////////////////////////////////////////

    private IEnumerator SlideDown(Joint _anchor, Vector3 _start, Vector3 _stop)
    {
        m_IsSliding = true;
        var rb = _anchor.GetComponent<Rigidbody>();
        _anchor.connectedBody = m_Owner.Rigidbody;
        _anchor.connectedAnchor = new Vector3(0, 1, 0);
        var length = Vector3.Distance(_start, _stop);
        float traveled = 0;
        var spd = 1f;
        while (traveled < length)
        {
            spd = spd >= m_SlideSpeed ? m_SlideSpeed : spd + 03f;
            traveled += spd * Time.deltaTime;
            rb.MovePosition(Vector3.Lerp(_start, _stop, traveled / length));
            yield return new WaitForFixedUpdate();
        }

        m_Owner.BlockInputs = false;
        _anchor.connectedBody = null;
        m_IsSliding = false;
        _anchor.gameObject.transform.position = _start;
    }

    //////////////////////////////////////////////////////////////////////////
    
    private bool m_IsSliding;
    [SerializeField] private float m_SlideSpeed;
    [SerializeField, TagSelector] private string m_ZiplineTag;
}
