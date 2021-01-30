using System.Collections;
using UnityEngine;

public class Hook : Head
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(m_ZiplineTag) && !m_Owner.IsGrounded)
        {
            var zipline = other.GetComponent<Zipline>();
            StartCoroutine(SlideDown(zipline.Start.position, zipline.Stop.position));
            m_Owner.BlockInputs = true;
            m_Owner.PreventFalling = true;
        }       
    }

    private IEnumerator SlideDown(Vector3 _start, Vector3 _stop)
    {
        var length = Vector3.Distance(_start, _stop);
        float traveled = 0;
        while (traveled < length)
        {
            traveled += m_SlideSpeed * Time.deltaTime;
            m_Owner.transform.position = Vector3.Lerp(_start, _stop, traveled / length);
            yield return new WaitForEndOfFrame();
        }

        m_Owner.BlockInputs = false;
        m_Owner.PreventFalling = false;
    }

    [SerializeField] private float m_SlideSpeed;
    [SerializeField, TagSelector] private string m_ZiplineTag;
}
