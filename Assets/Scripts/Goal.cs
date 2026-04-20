using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private string teamName = "Team A";
    [SerializeField] private float goalAreaRadius = 2f;  // ✅ THÊM: Bán kính khung thành
    
    private static Goal[] allGoals;

    private void Start()
    {
        allGoals = FindObjectsByType<Goal>(FindObjectsSortMode.None);
    }

    /// <summary>
    /// Kiểm tra xem bóng có nằm trong khung thành không
    /// </summary>
    public bool IsBallInside(Vector3 ballPosition)
    {
        return Vector3.Distance(transform.position, ballPosition) <= goalAreaRadius;
    }

    /// <summary>
    /// Lấy khung thành gần nhất
    /// </summary>
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

    /// <summary>
    /// ✅ THÊM: Kiểm tra xem bóng có nằm trong bất kỳ khung thành nào không
    /// </summary>
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

    public void OnBallEntered()
    {
        Debug.Log($"⚽ GOAL! {teamName} ghi được bàn!");
    }

    public string GetTeamName()
    {
        return teamName;
    }
}