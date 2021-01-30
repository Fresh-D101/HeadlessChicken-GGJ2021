using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [AddComponentMenu("Player/Movement")]
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour
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
            m_JumpInput = Input.GetButton("Jump");

            m_Rigidbody.MoveRotation(Quaternion.Euler(0, this.transform.localEulerAngles.y + m_HorizontalInput * m_Rotation, 0));
        }

        //////////////////////////////////////////////////////////////////////////

        private void FixedUpdate()
        {
            m_IsGrounded = Physics.CheckSphere(m_GroundedCheck.position, m_GroundedCheckRadius, m_GroundedCheckLayers);

            Move();

            if (m_JumpInput && m_IsGrounded)
            {
                m_Rigidbody.AddForce(Vector3.up * m_JumpStrength, ForceMode.VelocityChange);
            }
        }

        //////////////////////////////////////////////////////////////////////////

        private void Move()
        {
            m_TargetVelocity = this.transform.forward * (m_VerticalInput * m_MaximumVelocity);
            m_AppliedVelocity = m_TargetVelocity - m_Rigidbody.velocity;
            Vector2 clampedVelocity = Vector2.ClampMagnitude(new Vector2(m_AppliedVelocity.x, m_AppliedVelocity.z), m_Acceleration);
            m_AppliedVelocity.x = clampedVelocity.x;
            m_AppliedVelocity.y = 0;
            m_AppliedVelocity.z = clampedVelocity.y;
            m_Rigidbody.AddForce(m_AppliedVelocity, ForceMode.VelocityChange);
        }

        //////////////////////////////////////////////////////////////////////////

        [SerializeField] private Rigidbody m_Rigidbody = null;
        [Space]
        [SerializeField] private Transform m_GroundedCheck = null;
        [SerializeField] private float m_GroundedCheckRadius = 0;
        [SerializeField] private LayerMask m_GroundedCheckLayers = default;
        [Space]
        [SerializeField] private float m_Acceleration = 0;
        [SerializeField] private float m_Rotation = 0;
        [SerializeField] private float m_JumpStrength = 0;
        [Space]
        [SerializeField] private float m_MaximumVelocity = 0;
        [Space]
        [SerializeField] private float m_HorizontalInput = 0;
        [SerializeField] private float m_VerticalInput = 0;
        [SerializeField] private Vector3 m_TargetVelocity = Vector3.zero;
        [SerializeField] private Vector3 m_AppliedVelocity = Vector3.zero;
        [SerializeField] private bool m_JumpInput = false;
        [SerializeField] private bool m_IsGrounded = false;
    }
}