using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolbox.Editor.Hierarchy;
using Toolbox.Editor.Drawers;

namespace Player
{
    [AddComponentMenu("Player/Player Controller")]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private void Reset()
        {
            this.TryGetComponent<Rigidbody>(out m_Rigidbody);
        }

        //////////////////////////////////////////////////////////////////////////

        private void Update()
        {
            m_HorizontalInput = Input.GetAxisRaw("Horizontal");
            m_VerticalInput = Input.GetAxisRaw("Vertical");
            if (Input.GetButtonDown("Jump")) m_JumpInput = true;

            m_Rigidbody.MoveRotation(Quaternion.Euler(0, this.transform.localEulerAngles.y + m_HorizontalInput * m_Rotation, 0));
        }

        //////////////////////////////////////////////////////////////////////////

        private void OnTriggerEnter(Collider _otherCollider)
        {
            if (_otherCollider.CompareTag(m_ClimableTag))
            {
                CanClimb = true;
            }
        }

        //////////////////////////////////////////////////////////////////////////

        private void OnTriggerStay(Collider _otherCollider)
        {
            if (Input.GetButtonDown("Interact") && _otherCollider.TryGetComponent<IHead>(out IHead newHead))
            {
                EquipHead(newHead);
                _otherCollider.enabled = false;
            }
        }

        //////////////////////////////////////////////////////////////////////////

        private void OnTriggerExit(Collider _otherCollider)
        {
            if (_otherCollider.CompareTag(m_ClimableTag))
            {
                CanClimb = false;
            }
        }

        //////////////////////////////////////////////////////////////////////////

        #region Movement

        private void FixedUpdate()
        {
            m_IsGrounded = Physics.CheckSphere(m_GroundedCheck.position, m_GroundedCheckRadius, m_GroundedCheckLayers);

            Move();
            Jump();
        }

        //////////////////////////////////////////////////////////////////////////

        private void Move()
        {
            m_TargetVelocity = this.transform.forward * m_VerticalInput * MaximumVelocity;
            m_AppliedVelocity = m_TargetVelocity - m_Rigidbody.velocity;
            Vector2 clampedVelocity = Vector2.ClampMagnitude(new Vector2(m_AppliedVelocity.x, m_AppliedVelocity.z), m_Acceleration);
            m_AppliedVelocity.x = clampedVelocity.x;
            m_AppliedVelocity.y = 0;
            m_AppliedVelocity.z = clampedVelocity.y;
            m_Rigidbody.AddForce(m_AppliedVelocity, ForceMode.VelocityChange);
        }

        //////////////////////////////////////////////////////////////////////////

        private void Jump()
        {
            if (m_JumpInput && (m_IsGrounded || CanClimb))
            {
                m_Rigidbody.AddForce(Vector3.up * m_JumpStrength, ForceMode.VelocityChange);
                m_JumpInput = false;
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////

        private void EquipHead(IHead _head)
        {
            m_EquippedHead = _head;
            _head.gameObj.transform.SetPositionAndRotation(m_HeadPosition.position, m_HeadPosition.rotation);
            _head.gameObj.gameObject.transform.SetParent(m_HeadPosition);
            m_EquippedHead.OnPickup(this);
        }

        //////////////////////////////////////////////////////////////////////////

        public float MaximumVelocity { get => m_MaximumVelocity; set => m_MaximumVelocity = value; }
        public bool CanClimb { get => m_CanClimb; set => m_CanClimb = value; }

        //////////////////////////////////////////////////////////////////////////

        [BeginGroup("References")]
        [SerializeField] private Rigidbody m_Rigidbody = null;
        [SerializeField, EndGroup] private Transform m_HeadPosition = null;

        [BeginGroup("Ground Check")]
        [SerializeField] private Transform m_GroundedCheck = null;
        [SerializeField] private float m_GroundedCheckRadius = 0;
        [SerializeField, EndGroup] private LayerMask m_GroundedCheckLayers = default;

        [BeginGroup("Movement")]
        [SerializeField] private float m_Acceleration = 0;
        [SerializeField] private float m_Rotation = 0;
        [SerializeField] private float m_JumpStrength = 0;
        [Separator]
        [SerializeField, EndGroup] private float m_MaximumVelocity = 0;

        [BeginGroup("Movement")]
        [SerializeField, TagSelector, EndGroup] private string m_ClimableTag = string.Empty;

        [BeginGroup("Information")]
        [SerializeField, ReadOnlyField] private float m_HorizontalInput = 0;
        [SerializeField, ReadOnlyField] private float m_VerticalInput = 0;
        [SerializeField, ReadOnlyField] private Vector3 m_TargetVelocity = Vector3.zero;
        [SerializeField, ReadOnlyField] private Vector3 m_AppliedVelocity = Vector3.zero;
        [SerializeField, ReadOnlyField] private bool m_JumpInput = false;
        [SerializeField, ReadOnlyField] private bool m_IsGrounded = false;
        [SerializeField, ReadOnlyField, EndGroup] private bool m_CanClimb = false;

        private IHead m_EquippedHead = null;
    }
}