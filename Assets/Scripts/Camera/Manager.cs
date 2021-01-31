using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    [AddComponentMenu("Camera/Camera Manager")]
    public class Manager : MonoBehaviour
    {
        private void Reset()
        {
            GetMainCamera();
        }

        //////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError($"Another {nameof(Manager)} already exists!");
                Destroy(this);
            }

            GetMainCamera();
        }

        //////////////////////////////////////////////////////////////////////////

        private void GetMainCamera()
        {
            if (m_MainCamera == null) m_MainCamera = UnityEngine.Camera.main;
        }

        //////////////////////////////////////////////////////////////////////////

        public void ChangeCameraPosition(Transform _cameraAnchor, Action _callback = null)
        {
            if (_cameraAnchor == null) return;

            m_MainCameraBlackscreen.blocksRaycasts = true;
            LeanTween.value(this.gameObject, (value) => { m_MainCameraBlackscreen.alpha = value; }, m_MainCameraBlackscreen.alpha, 1, m_FadeInTime).setOnComplete(() => { MoveCameraToAnchor(_cameraAnchor); _callback?.Invoke(); });
            LeanTween.value(this.gameObject, (value) => { m_MainCameraBlackscreen.alpha = value; }, 1, 0, m_FadeOutTime).setOnComplete(() => { m_MainCameraBlackscreen.blocksRaycasts = false; }).setDelay(m_FadeInTime + m_FadeOutDelay);
        }

        //////////////////////////////////////////////////////////////////////////

        private void MoveCameraToAnchor(Transform _cameraAnchor)
        {
            m_MainCamera.transform.SetPositionAndRotation(_cameraAnchor.position, _cameraAnchor.rotation);
        }

        //////////////////////////////////////////////////////////////////////////

        public static Manager Instance = null;

        [SerializeField] private UnityEngine.Camera m_MainCamera = null;
        [SerializeField] private CanvasGroup m_MainCameraBlackscreen = null;
        [SerializeField, TagSelector] private string m_CameraAnchorTag = string.Empty;
        [Separator]
        [SerializeField] private float m_FadeInTime = 0;
        [SerializeField] private float m_FadeOutDelay = 0;
        [SerializeField] private float m_FadeOutTime = 0;

#if UNITY_EDITOR
        [Separator]
        [SerializeField] private int m_CameraIndex = 0;
#endif
    }
}