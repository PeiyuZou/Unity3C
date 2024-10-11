//------------------------------------------------------------
// 描述：player input
// 作者：Z.P.Y
// 时间：2024/09/20 11:09
//------------------------------------------------------------

using System;
using UnityEngine;

namespace Test
{
    /// <summary>
    /// player input
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        // input flag
        public bool Enable = true;

        // player input keys
        public string KeyForward = "w";
        public string KeyLeft = "a";
        public string KeyBack = "s";
        public string KeyRight = "d";
        public string KeyRun = "left shift";
        public string KeyJump = "space";

        // axis values
        private float m_InputVertical;
        private float m_InputHorizontal;
        private float m_OutputVertical;
        private float m_OutputHorizontal;

        // output fields
        public float InputSpeed;
        public Vector3 InputDirection;

        public bool IsPressRun;
        public bool IsTriggerJump;

        // lerp fields
        private float m_TargetVertical;
        private float m_TargetHorizontal;
        private float m_VelocityVertical;
        private float m_VelocityHorizontal;

        private Transform m_Transform;

        private void Awake()
        {
            m_Transform = transform;
        }

        private void Update()
        {
            m_TargetVertical = (Input.GetKey(KeyForward) ? 1f : 0f) - (Input.GetKey(KeyBack) ? 1f : 0f);
            m_TargetHorizontal = (Input.GetKey(KeyRight) ? 1f : 0f) - (Input.GetKey(KeyLeft) ? 1f : 0f);

            if (!Enable)
            {
                m_TargetVertical = 0;
                m_TargetHorizontal = 0;
            }

            m_InputVertical = Mathf.SmoothDamp(m_InputVertical, m_TargetVertical, ref m_VelocityVertical, 0.1f);
            m_InputHorizontal = Mathf.SmoothDamp(m_InputHorizontal, m_TargetHorizontal, ref m_VelocityHorizontal, 0.1f);

            var tempVector2 = Square2Circle(new Vector2(m_InputHorizontal, m_InputVertical));
            m_OutputHorizontal = tempVector2.x;
            m_OutputVertical = tempVector2.y;

            InputSpeed = Mathf.Sqrt(m_OutputVertical * m_OutputVertical + m_OutputHorizontal * m_OutputHorizontal);
            InputDirection = m_Transform.forward * m_OutputVertical + m_Transform.right * m_OutputHorizontal;

            IsPressRun = Input.GetKey(KeyRun);

            IsTriggerJump = Input.GetKeyDown(KeyJump);
        }

        private static Vector2 Square2Circle(Vector2 input)
        {
            var output = Vector2.zero;

            output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) * 0.5f);
            output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) * 0.5f);

            return output;
        }
    }
}