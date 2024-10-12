using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Test
{
    public class ActorController : MonoBehaviour
    {
        public PlayerInput PlayerInput;
        public Animator Animator;
        public Transform Trans;
        public float WalkSpeed = 1.4f;
        public float RunSpeed = 3.7f;
        public float JumpVelocity = 3f;

        private Rigidbody m_Rigidbody;

        /// <summary>
        /// 平面向量
        /// </summary>
        private Vector3 m_PlanarVector;
        private bool m_IsLockPlanar;

        /// <summary>
        /// 跳跃冲量
        /// </summary>
        private Vector3 m_ThrustVector;

        private static readonly int s_Speed = Animator.StringToHash("Speed");
        private static readonly int s_Jump = Animator.StringToHash("Jump");
        private static readonly int s_IsOnGround = Animator.StringToHash("IsOnGround");

        private void Awake()
        {
            m_Rigidbody = transform.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            var animSpeed = Mathf.Lerp(Animator.GetFloat(s_Speed), (PlayerInput.IsPressRun ? 2f : 1f), 0.05f);
            Animator.SetFloat(s_Speed, PlayerInput.InputSpeed * animSpeed);
            if (PlayerInput.IsTriggerJump)
            {
                Animator.SetTrigger(s_Jump);
            }
            if (PlayerInput.InputSpeed > 0.1f && !m_IsLockPlanar)
            {
                var forward = Vector3.Slerp(Trans.forward, PlayerInput.InputDirection, 0.05f);
                Trans.forward = forward;
            }
            if (!m_IsLockPlanar)
            {
                m_PlanarVector = Trans.forward * (PlayerInput.InputSpeed * (PlayerInput.IsPressRun ? RunSpeed : WalkSpeed));
            }
        }

        private void FixedUpdate()
        {
            //plan #1: using position Additive
            //m_Rigidbody.position += m_MovingVector * Time.fixedDeltaTime;

            //plan #2: using velocity
            m_Rigidbody.velocity = new Vector3(m_PlanarVector.x, m_Rigidbody.velocity.y, m_PlanarVector.z) + m_ThrustVector;
            m_ThrustVector = Vector3.zero;
        }

        public void OnJumpEnter()
        {
            PlayerInput.Enable = false;
            m_IsLockPlanar = true;
            m_ThrustVector = new Vector3(0, JumpVelocity, 0);
            Debug.LogError("OnJumpEnter!!");
        }

        public void OnJumpExit()
        {
            Debug.LogError("OnJumpExit!!");
        }

        public void OnGround()
        {
            Animator.SetBool(s_IsOnGround, true);
        }

        public void NotOnGround()
        {
            Animator.SetBool(s_IsOnGround, false);
        }

        public void OnGroundEnter()
        {
            PlayerInput.Enable = true;
            m_IsLockPlanar = false;
        }
    }
}