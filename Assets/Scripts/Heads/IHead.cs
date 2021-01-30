using UnityEngine;

public interface IHead
{
    GameObject gameObj { get; }

    void OnPickup(Player.PlayerController _owner);
    void OnDrop(Player.PlayerController _owner);
}
