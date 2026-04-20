using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    [SerializeField] private string teamName = "Team A";
    [SerializeField] private float goalAreaRadius = 2f;
    [SerializeField] private float returnCameraDelay = 2f;
    
    private static Goal[] allGoals;
    private CinemaChineManager cameraManager;

    private void Start()
    {
        allGoals = FindObjectsByType<Goal>(FindObjectsSortMode.None);
        cameraManager = FindFirstObjectByType<CinemaChineManager>();
    }

    public bool IsBallInside(Vector3 ballPosition)
    {
        return Vector3.Distance(transform.position, ballPosition) <= goalAreaRadius;
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

    private void OnTriggerEnter(Collider other)
    {
        
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
            OnBallEntered(ball);

    }

    public void OnBallEntered(Ball ball)
    {

        ball.Stop();

        if (cameraManager != null)
            cameraManager.ReturnToPlayerAfterDelay(returnCameraDelay);

    }

    public string GetTeamName()
    {
        return teamName;
    }
}