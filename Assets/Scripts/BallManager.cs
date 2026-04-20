using UnityEngine;
using System.Collections.Generic;

public class BallManager : MonoBehaviour
{
    private static BallManager instance;
    private List<Ball> allBalls;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        allBalls = new List<Ball>(FindObjectsByType<Ball>(FindObjectsSortMode.None));
    }

    public static Ball GetNearestBall(Vector3 position, float detectionRadius)
    {
        if (instance == null || instance.allBalls.Count == 0)
            return null;

        Ball nearestBall = null;
        float minDistance = detectionRadius;

        foreach (Ball ball in instance.allBalls)
        {
            float distance = Vector3.Distance(position, ball.GetPosition());
            
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestBall = ball;
            }
        }

        return nearestBall;
    }
    public static Ball GetFarthestBall(Vector3 position)
    {
        if (instance == null || instance.allBalls.Count == 0)
            return null;

        Ball farthestBall = null;
        float maxDistance = 0f;

        foreach (Ball ball in instance.allBalls)
        {
            float distance = Vector3.Distance(position, ball.GetPosition());
            
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestBall = ball;
            }
        }

        return farthestBall;
    }

    public static Ball GetFarthestBallOutsideGoals(Vector3 position)
    {
        if (instance == null || instance.allBalls.Count == 0)
            return null;

        Ball farthestBall = null;
        float maxDistance = 0f;

        foreach (Ball ball in instance.allBalls)
        {
            if (Goal.IsBallInAnyGoal(ball.GetPosition()))
            {
                continue;
            }

            float distance = Vector3.Distance(position, ball.GetPosition());
            
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestBall = ball;
            }
        }

        return farthestBall;
    }

    public static void RegisterBall(Ball ball)
    {
        if (instance != null && !instance.allBalls.Contains(ball))
        {
            instance.allBalls.Add(ball);
        }
    }
}