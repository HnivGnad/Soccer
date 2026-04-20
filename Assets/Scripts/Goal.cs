using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private string teamName = "Team A";
    [SerializeField] private float goalAreaRadius = 2f;
    [SerializeField] private float returnCameraDelay = 2f;
    [SerializeField] private ParticleSystem goalParticle;
    
    private static Goal[] allGoals;
    private CinemaChineManager cameraManager;

    private void Start()
    {
        allGoals = FindObjectsByType<Goal>(FindObjectsSortMode.None);
        cameraManager = FindFirstObjectByType<CinemaChineManager>();
        
        if (cameraManager == null)
            Debug.LogError($"❌ Goal {teamName}: Không tìm thấy CinemaChineManager!");
        
        if (goalParticle == null)
            Debug.LogWarning($"⚠️ Goal {teamName}: Không có ParticleSystem!");
    }

    public bool IsBallInside(Vector3 ballPosition)
    {
        return Vector3.Distance(transform.position, ballPosition) <= goalAreaRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            OnBallEntered(ball);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            OnBallEntered(ball);
        }
    }

    public static Goal GetNearestGoal(Vector3 position)
    {
        if (allGoals == null || allGoals.Length == 0)
            return null;

        Goal nearest = allGoals[0];
        float minDistance = Vector3.Distance(position, nearest.transform.position);

        foreach (Goal goal in allGoals)
        {
            float distance = Vector3.Distance(position, goal.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = goal;
            }
        }

        return nearest;
    }

    public static bool IsBallInAnyGoal(Vector3 ballPosition)
    {
        if (allGoals == null || allGoals.Length == 0)
            return false;

        foreach (Goal goal in allGoals)
        {
            if (goal.IsBallInside(ballPosition))
            {
                return true;
            }
        }

        return false;
    }

    public void OnBallEntered(Ball ball)
    {
        Debug.Log($"⚽ GOAL! {teamName} ghi được bàn!");

        ball.Stop();

        if (goalParticle != null)
        {
            Debug.Log($"✨ Chạy particle tại {teamName}!");
            goalParticle.Play();
        }

        if (cameraManager != null)
        {
            Debug.Log($"📹 {teamName}: Gọi ReturnToPlayerAfterDelay({returnCameraDelay}s)");
            cameraManager.ReturnToPlayerAfterDelay(returnCameraDelay);
        }
    }

    public string GetTeamName()
    {
        return teamName;
    }
}