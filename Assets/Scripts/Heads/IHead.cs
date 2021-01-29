using UnityEngine;

public interface IHead
{
    GameObject gameObj { get; }
    
    void OnPickup(SimpleMove _owner);
    void OnDrop(SimpleMove _owner);
}
