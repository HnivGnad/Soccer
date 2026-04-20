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
        Debug.Log($"🏀 BallManager tìm thấy {allBalls.Count} quả bóng");
    }

    /// <summary>
    /// Lấy quả bóng gần nhất từ vị trí cho trước
    /// </summary>
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

    /// <summary>
    /// Lấy quả bóng xa nhất (không bỏ qua bóng trong khung thành)
    /// </summary>
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

    /// <summary>
    /// ✅ THÊM: Lấy quả bóng xa nhất NGOÀI khung thành
    /// </summary>
    public static Ball GetFarthestBallOutsideGoals(Vector3 position)
    {
        if (instance == null || instance.allBalls.Count == 0)
            return null;

        Ball farthestBall = null;
        float maxDistance = 0f;

        foreach (Ball ball in instance.allBalls)
        {
            // ✅ Bỏ qua bóng nằm trong khung thành
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

    /// <summary>
    /// Thêm Ball mới (dùng nếu tạo Ball runtime)
    /// </summary>
    public static void RegisterBall(Ball ball)
    {
        if (instance != null && !instance.allBalls.Contains(ball))
        {
            instance.allBalls.Add(ball);
            Debug.Log($"➕ Đã thêm quả bóng mới. Tổng: {instance.allBalls.Count}");
        }
    }
}