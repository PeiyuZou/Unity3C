using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider CapsuleCollider;
    public float DownwardOffset = 0.05f;

    private float m_Radius;
    private float m_Height;
    private Vector3 m_CapsuleTop;
    private Vector3 m_CapsuleBottom;
    private int m_GroundLayerIndex;

    void Awake()
    {
        m_Radius = CapsuleCollider.radius;
        m_Height = CapsuleCollider.height;
        m_GroundLayerIndex = LayerMask.GetMask("Ground");
    }

    void FixedUpdate()
    {
        m_CapsuleBottom = transform.position + Vector3.up * (m_Radius - DownwardOffset);
        m_CapsuleTop = transform.position + Vector3.up * (m_Height - m_Radius - DownwardOffset);
        var overlaps = Physics.OverlapCapsule(m_CapsuleBottom, m_CapsuleTop, m_Radius, m_GroundLayerIndex);
        SendMessageUpwards(overlaps.Length != 0 ? "OnGround" : "NotOnGround");
    }
}
