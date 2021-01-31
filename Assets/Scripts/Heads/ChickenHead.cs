using System;
using UnityEngine;

public class ChickenHead : Head
{
    protected override void OnPickup()
    {
        GameCamera.Manager.Instance.ChangeCameraPosition(m_VictoryAnchor,
            () =>
            {
                m_Owner.transform.SetPositionAndRotation(m_VictoryPos.position, m_VictoryPos.rotation);
                m_Owner.BlockInputs = true;
                m_WinScreen.alpha = 1;
                Game.CurrentState = Game.State.Victory;
            });
    }

    [SerializeField] private Transform m_VictoryPos;
    [SerializeField] private Transform m_VictoryAnchor;
    [SerializeField] private CanvasGroup m_WinScreen = null;
}
