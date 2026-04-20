using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 1.2f;
    [SerializeField] private float kickForce = 15f;
    
    private Rigidbody rb;
    private bool isFlying = false;
    private CinemaChineManager cameraManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraManager = FindFirstObjectByType<CinemaChineManager>();
        
        BallManager.RegisterBall(this);
    }

    public bool IsPlayerNear(Vector3 playerPosition)
    {
        return Vector3.Distance(transform.position, playerPosition) <= detectionRadius;
    }

    public void KickToNearestGoal(Vector3 kickDirection)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        
        rb.linearVelocity = kickDirection.normalized * kickForce;
        isFlying = true;

        if (cameraManager != null)
            cameraManager.FollowBall(transform);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetDetectionRadius()
    {
        return detectionRadius;
    }

    public bool IsFlying()
    {
        return isFlying;
    }

    public void Stop()
    {
        isFlying = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}