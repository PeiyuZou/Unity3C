//------------------------------------------------------------
// 描述：角色控制
// 作者：Z.P.Y
// 时间：2024/09/21 07:46
//------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Test
{
    /// <summary>
    /// 角色控制
    /// </summary>
    public class ActorController : MonoBehaviour
    {
        public PlayerInput PlayerInput;
        public Animator Animator;
        public Transform Trans;
        public float WalkSpeed = 1.4f;
        public float RunSpeed = 3.7f;

        private Rigidbody m_Rigidbody;
        private Vector3 m_MovingVector;

        private static readonly int s_Speed = Animator.StringToHash("Speed");
        private static readonly int s_Jump = Animator.StringToHash("Jump");

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
            if (PlayerInput.InputSpeed > 0.1f)
            {
                var forward = Vector3.Slerp(Trans.forward, PlayerInput.InputDirection, 0.05f);
                Trans.forward = forward;
            }
            m_MovingVector = Trans.forward * (PlayerInput.InputSpeed * (PlayerInput.IsPressRun ? RunSpeed : WalkSpeed));
        }

        private void FixedUpdate()
        {
            //plan #1: using position Additive
            //m_Rigidbody.position += m_MovingVector * Time.fixedDeltaTime;

            //plan #2: using velocity
            m_Rigidbody.velocity = new Vector3(m_MovingVector.x, m_Rigidbody.velocity.y, m_MovingVector.z);
        }
    }
}