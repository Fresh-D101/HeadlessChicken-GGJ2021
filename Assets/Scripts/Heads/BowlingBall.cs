using UnityEngine;

public class BowlingBall : Head
{
    protected override void  OnPickup()
    {
        m_OriginalSpeedRange = m_Owner.MovementSpeedRange;
        m_Owner.MovementSpeedRange = m_NewSpeedRange;
    }

    //////////////////////////////////////////////////////////////////////////

    protected override void OnDrop()
    {
        m_Owner.MovementSpeedRange = m_OriginalSpeedRange;
    }

    //////////////////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {
        if (m_Owner is null) return;

        if (other.CompareTag("Breakable"))
        {
            other.GetComponent<Breakable>()?.Break();
        }
    }

    //////////////////////////////////////////////////////////////////////////

    public GameObject gameObj => gameObject;

    //////////////////////////////////////////////////////////////////////////

    [SerializeField, MinMaxSlider(0, 1000)] private Vector2 m_NewSpeedRange = Vector2.zero;
    [Separator]
    [SerializeField, MinMaxSlider(0, 1000), ReadOnlyField] private Vector2 m_OriginalSpeedRange = Vector2.zero;
}
