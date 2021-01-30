using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    [AddComponentMenu("Camera/Room Trigger")]
    public class Trigger : MonoBehaviour
    {
        private void Awake()
        {
            if (m_CameraAnchor == null) GetRoomCameraAnchor();
        }

        //////////////////////////////////////////////////////////////////////////

        [ContextMenu("DEV: Find Camera Anchor")]
        private void GetRoomCameraAnchor()
        {
            if ((m_CameraAnchor = this.transform.parent.Find("CameraAnchor")) == null)
            {
                Debug.LogError("No CameraAnchor has been set for this room!", this.gameObject);
            }
        }

        //////////////////////////////////////////////////////////////////////////

        private void OnTriggerEnter(Collider _otherCollider)
        {
            if (m_CameraAnchor == null)
            {
                Debug.LogError($"{nameof(m_CameraAnchor)} is null!", this.gameObject);
                return;
            }

            if (_otherCollider.attachedRigidbody == null) return;

            if (_otherCollider.attachedRigidbody.CompareTag(m_PlayerTag))
            {
                GameCamera.Manager.Instance.ChangeCameraPosition(m_CameraAnchor);
            }
        }

        //////////////////////////////////////////////////////////////////////////

        [SerializeField] private Transform m_CameraAnchor = null;
        [SerializeField, TagSelector] private string m_PlayerTag = string.Empty;
    }
}