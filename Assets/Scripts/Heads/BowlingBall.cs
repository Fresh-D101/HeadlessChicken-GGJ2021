using System;
using UnityEngine;

public class BowlingBall : MonoBehaviour, IHead
{
    public void OnPickup(SimpleMove _owner)
    {
        originalSpeed = _owner.speed;
        _owner.SetSpeed(4);
    }

    public void OnDrop(SimpleMove _owner)
    {
        _owner.SetSpeed(originalSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Breakable>()?.Break();
    }

    public GameObject gameObj => gameObject;

    private float originalSpeed;
    private SimpleMove owner;
}
