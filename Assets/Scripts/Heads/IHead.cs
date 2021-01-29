using UnityEngine;

public interface IHead
{
    GameObject gameObj { get; }
    
    void OnPickup(SimpleMove owner);
    void OnDrop(SimpleMove owner);
}
