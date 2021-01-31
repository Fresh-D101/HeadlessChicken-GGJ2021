using UnityEngine;

public class BowlingBall : Head
{
    protected override void  OnPickup()
    {
        m_OriginalSpeedRange = m_Owner.MovementSpeedRange;
        m_Owner.MovementSpeedRange = m_NewSpeedRange;
        m_OriginalRotLerp = m_Owner.RotationSpeedLerpDecrease;
        m_Owner.RotationSpeedLerpDecrease = 0f;
    }

    //////////////////////////////////////////////////////////////////////////

    protected override void OnDrop()
    {
        m_Owner.MovementSpeedRange = m_OriginalSpeedRange;
        m_Owner.RotationSpeedLerpDecrease = m_OriginalRotLerp;
    }

    //////////////////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {
        if (m_Owner is null) return;

        if (other.CompareTag("Breakable") && m_Owner.CurrentSpeed >= m_MinimumBreakSpeed)
        {
            other.GetComponent<Breakable>()?.Break(other.ClosestPoint(transform.position), m_Owner.CurrentSpeed * 2);
        }
    }

    //////////////////////////////////////////////////////////////////////////

    public GameObject gameObj => gameObject;

    //////////////////////////////////////////////////////////////////////////

    [SerializeField, MinMaxSlider(0, 1000)] private Vector2 m_NewSpeedRange = Vector2.zero;
    [SerializeField] private float m_MinimumBreakSpeed;
    [Separator]
    [SerializeField, MinMaxSlider(0, 1000), ReadOnlyField] private Vector2 m_OriginalSpeedRange = Vector2.zero;
    [SerializeField, ReadOnlyField] private float m_OriginalRotLerp;
}
