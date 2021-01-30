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
        private void Awake()
        {
            m_CanJump = true;
        }

        //////////////////////////////////////////////////////////////////////////

        private void Reset()
        {
            this.TryGetComponent<Rigidbody>(out m_Rigidbody);
        }

        //////////////////////////////////////////////////////////////////////////

        private void Update()
        {
            // Interaction and Head Inputs
            if (Input.GetKeyDown(KeyCode.X)) DropHead();
            if (Input.GetButtonDown("Interact")) m_InteractionInput = true;

            if (BlockInputs) return;

            // Movement Inputs
            m_HorizontalInput = Input.GetAxisRaw("Horizontal");
            m_VerticalInput = Input.GetAxisRaw("Vertical");
            if (Input.GetButtonDown("Jump")) m_JumpInput = true;
            m_Rigidbody.MoveRotation(Quaternion.Euler(0, this.transform.localEulerAngles.y + m_HorizontalInput * m_Rotation, 0));
        }

        //////////////////////////////////////////////////////////////////////////

        private void OnTriggerStay(Collider _otherCollider)
        {
            if (m_InteractionInput && _otherCollider.TryGetComponent<Head>(out var newHead))
            {
                EquipHead(newHead);
                m_InteractionInput = false;
            }
        }

        //////////////////////////////////////////////////////////////////////////

        #region Movement

        private void FixedUpdate()
        {
            m_IsGrounded = Physics.CheckSphere(m_GroundedCheck.position, m_GroundedCheckRadius, m_GroundedCheckLayers);
            m_IsFalling = (m_Rigidbody.velocity.y < 0.00001f);

            if (m_PreventFalling && m_IsFalling) SetConstraints();

            Move();
            Jump();
        }

        //////////////////////////////////////////////////////////////////////////

        private void Move()
        {
            m_TargetVelocity = transform.forward * (m_VerticalInput * MaximumVelocity);
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
            if (!m_CanJump)
            {
                m_JumpInput = false;
                return;
            }

            if (m_JumpInput && (m_IsGrounded || CanClimb))
            {
                ResetConstraints();
                m_Rigidbody.AddForce(Vector3.up * m_JumpStrength, ForceMode.VelocityChange);
                m_CanJump = false;
                LeanTween.delayedCall(m_JumpDelay, () => { m_CanJump = true; });
                m_JumpInput = false;
            }
            else if (m_JumpInput)
            {
                m_JumpInput = false;
                return;
            }
        }

        //////////////////////////////////////////////////////////////////////////

        public void SetConstraints()
        {
            m_Rigidbody.constraints |= RigidbodyConstraints.FreezePositionX;
            m_Rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
            m_Rigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;
        }

        //////////////////////////////////////////////////////////////////////////

        public void ResetConstraints()
        {
            m_Rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
            m_Rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
            m_Rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////

        private void EquipHead(Head _head)
        {
            DropHead();
            m_EquippedHead = _head;
            m_EquippedHead.OnPickup(this, m_HeadPosition);
        }

        //////////////////////////////////////////////////////////////////////////

        private void DropHead()
        {
            if (ReferenceEquals(m_EquippedHead, null)) return;
            m_EquippedHead.OnDrop(this);
            m_EquippedHead = null;
        }

        //////////////////////////////////////////////////////////////////////////

        public bool BlockInputs { get => m_blockInputs; set => m_blockInputs = value; }
        public float MaximumVelocity { get => m_MaximumVelocity; set => m_MaximumVelocity = value; }
        public bool CanClimb { get => m_CanClimb; set => m_CanClimb = value; }
        public bool PreventFalling { get => m_PreventFalling; set { m_PreventFalling = value; ResetConstraints(); } }

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
        [SerializeField] private float m_JumpDelay = 0;
        [Separator]
        [SerializeField, EndGroup] private float m_MaximumVelocity = 0;

        [BeginGroup("Movement")]
        [SerializeField, TagSelector, EndGroup] private string m_ClimableTag = string.Empty;

        [BeginGroup("Information")]
        [SerializeField, ReadOnlyField] private bool m_blockInputs;
        [Separator]
        [SerializeField, ReadOnlyField] private float m_HorizontalInput = 0;
        [SerializeField, ReadOnlyField] private float m_VerticalInput = 0;
        [SerializeField, ReadOnlyField] private bool m_JumpInput = false;
        [SerializeField, ReadOnlyField] private bool m_InteractionInput = false;
        [Separator]
        [SerializeField, ReadOnlyField] private Vector3 m_TargetVelocity = Vector3.zero;
        [SerializeField, ReadOnlyField] private Vector3 m_AppliedVelocity = Vector3.zero;
        [Separator]
        [SerializeField, ReadOnlyField] private bool m_IsGrounded = false;
        [SerializeField, ReadOnlyField] private bool m_IsFalling = false;
        [SerializeField, ReadOnlyField] private bool m_CanJump = false;
        [SerializeField, ReadOnlyField] private bool m_PreventFalling = false;
        [SerializeField, ReadOnlyField, EndGroup] private bool m_CanClimb = false;

        private Head m_EquippedHead = null;
    }
}