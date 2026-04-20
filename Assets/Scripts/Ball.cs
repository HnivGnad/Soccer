using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private float kickForce = 20f;
    
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // ✅ THÊM: Đăng ký Ball này với Manager
        BallManager.RegisterBall(this);
    }

    /// <summary>
    /// Kiểm tra xem người chơi có gần bóng không
    /// </summary>
    public bool IsPlayerNear(Vector3 playerPosition)
    {
        return Vector3.Distance(transform.position, playerPosition) <= detectionRadius;
    }

    /// <summary>
    /// Sút bóng về phía khung thành gần nhất
    /// </summary>
    public void KickToNearestGoal(Vector3 kickDirection)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        // Áp dụng lực sút
        rb.linearVelocity = kickDirection.normalized * kickForce;   

        Debug.Log($"🦵 Kicked ball! Direction: {kickDirection.normalized}, Force: {kickForce}");
    }

    /// <summary>
    /// Lấy vị trí bóng hiện tại
    /// </summary>
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetDetectionRadius()
    {
        return detectionRadius;
    }
}