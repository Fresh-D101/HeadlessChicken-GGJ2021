using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

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
            if (Input.GetButtonDown("Interact")) CheckForHead();

            if (BlockInputs) return;

            // Movement Inputs
            m_HorizontalInput = Input.GetAxisRaw("Horizontal");
            m_VerticalInput = Input.GetAxisRaw("Vertical");
            if (Input.GetButtonDown("Jump")) m_JumpInput = true;
            Rotate();
        }

        //////////////////////////////////////////////////////////////////////////

        #region Movement

        private void FixedUpdate()
        {
            m_IsGrounded = Physics.CheckSphere(m_GroundedCheck.position, m_GroundedCheckRadius, m_GroundedCheckLayers);
            m_IsFalling = (m_Rigidbody.velocity.y < -0.00001f);

            if (m_PreventFalling && m_IsFalling) SetConstraints();

            Move();
            Jump();
        }

        //////////////////////////////////////////////////////////////////////////

        private void Move()
        {
            if (m_VerticalInput != 0)
            {
                m_MovementSpeedLerp += m_LerpRates.x * Time.fixedDeltaTime;
            }
            else
            {
                m_MovementSpeedLerp -= m_LerpRates.y * Time.fixedDeltaTime;
            }

            m_MovementSpeedLerp = Mathf.Clamp01(m_MovementSpeedLerp);

            m_TargetVelocity = this.transform.forward * (m_VerticalInput * Mathf.Lerp(MovementSpeedRange.x, MovementSpeedRange.y, m_MovementSpeedLerp)) * Time.fixedDeltaTime;
            m_TargetVelocity.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = m_TargetVelocity;

            if (new Vector2(m_Rigidbody.velocity.x, m_Rigidbody.velocity.z).magnitude > 0)
            {
                m_Animator.SetBool("IsWalking", true);
            }
            else
            {
                m_Animator.SetBool("IsWalking", false);
            }
        }

        //////////////////////////////////////////////////////////////////////////

        private void Rotate()
        {
            if (m_HorizontalInput != 0) m_MovementSpeedLerp -= m_RotationSpeedLerpDecrease * Time.fixedDeltaTime;
            m_MovementSpeedLerp = Mathf.Clamp01(m_MovementSpeedLerp);
            m_Rigidbody.MoveRotation(Quaternion.Euler(0, this.transform.localEulerAngles.y + m_HorizontalInput * m_Rotation, 0));
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

        private void CheckForHead()
        {
            var nearest = new Collider[1];
            var heads = Physics.OverlapSphereNonAlloc(transform.position, 2, nearest, m_HeadLayer);
            if (heads != 0)
            {
                EquipHead(nearest[0].GetComponent<Head>());
            }
        }

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

        public Vector2 MovementSpeedRange { get => m_MovementSpeedRange; set => m_MovementSpeedRange = value; }
        public bool BlockInputs { get => m_blockInputs; set => m_blockInputs = value; }
        public bool CanClimb { get => m_CanClimb; set => m_CanClimb = value; }
        public bool PreventFalling { get => m_PreventFalling; set { m_PreventFalling = value; ResetConstraints(); } }
        public bool IsGrounded => m_IsGrounded;
        public Rigidbody Rigidbody => m_Rigidbody;
        public bool IsFalling => m_IsFalling;

        //////////////////////////////////////////////////////////////////////////

        [BeginGroup("References")]
        [SerializeField] private Rigidbody m_Rigidbody = null;
        [SerializeField] private Animator m_Animator = null;
        [SerializeField, EndGroup] private Transform m_HeadPosition = null;

        [BeginGroup("Ground Check")]
        [SerializeField] private Transform m_GroundedCheck = null;
        [SerializeField] private float m_GroundedCheckRadius = 0;
        [SerializeField] private LayerMask m_GroundedCheckLayers = default;
        [SerializeField, EndGroup] private LayerMask m_HeadLayer = default;

        [BeginGroup("Movement")]
        [SerializeField, MinMaxSlider(0, 1000)] private Vector2 m_MovementSpeedRange = Vector2.zero;
        [SerializeField] private Vector2 m_LerpRates = Vector2.zero;
        [SerializeField, ReadOnlyField] private float m_MovementSpeedLerp = 0;
        [Separator]
        [SerializeField] private float m_Rotation = 0;
        [SerializeField] private float m_RotationSpeedLerpDecrease = 0;
        [SerializeField] private float m_JumpStrength = 0;
        [SerializeField, EndGroup] private float m_JumpDelay = 0;

        [BeginGroup("Information")]
        [SerializeField, ReadOnlyField] private bool m_blockInputs;
        [Separator]
        [SerializeField, ReadOnlyField] private float m_HorizontalInput = 0;
        [SerializeField, ReadOnlyField] private float m_VerticalInput = 0;
        [SerializeField, ReadOnlyField] private bool m_JumpInput = false;
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