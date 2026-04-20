using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KickUI : MonoBehaviour
{
    [SerializeField] private Button kickButton;
    [SerializeField] private Button autoKickButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float dimmedAlpha = 0.3f;
    [SerializeField] private float brightAlpha = 1f;

    private Player player;
    private Ball currentNearestBall;
    private bool isNearBall = false;
    private Coroutine fadeCoroutine;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = dimmedAlpha;
        kickButton.interactable = false;

        kickButton.onClick.AddListener(OnKickButtonClicked);
        autoKickButton.onClick.AddListener(OnAutoKickButtonClicked);
    }

    private void Update()
    {
        if (player == null)
            return;

        currentNearestBall = BallManager.GetNearestBall(
            player.transform.position, 
            5f
        );

        bool nearBall = currentNearestBall != null;

        if (nearBall && !isNearBall)
        {
            ShowKickUI();
        }
        else if (!nearBall && isNearBall)
        {
            DimKickUI();
        }
    }

    private void ShowKickUI()
    {
        isNearBall = true;
        kickButton.interactable = true;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeUI(brightAlpha));
        Debug.Log("💚 UI Kick sáng lên - có thể sút!");
    }

    private void DimKickUI()
    {
        isNearBall = false;
        kickButton.interactable = false;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeUI(dimmedAlpha));
        Debug.Log("⚫ UI Kick mờ đi - quá xa để sút");
    }

    private IEnumerator FadeUI(float targetAlpha)
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    private void OnKickButtonClicked()
    {
        if (!isNearBall || currentNearestBall == null)
            return;

        KickBallToGoal(currentNearestBall);
        DimKickUI();
    }

    /// <summary>
    /// ✅ SỬA: Click nút Auto Kick - sút bóng xa nhất NGOÀI khung thành
    /// </summary>
    private void OnAutoKickButtonClicked()
    {
        // ✅ SỬA: Gọi phương thức mới
        Ball farthestBall = BallManager.GetFarthestBallOutsideGoals(player.transform.position);

        if (farthestBall == null)
        {
            Debug.LogWarning("❌ Không có quả bóng nào NGOÀI khung thành!");
            return;
        }

        float distance = Vector3.Distance(player.transform.position, farthestBall.GetPosition());
        Debug.Log($"🤖 Auto Kick! Sút bóng xa nhất ngoài khung (khoảng cách: {distance:F2}m)");
        
        KickBallToGoal(farthestBall);
    }

    private void KickBallToGoal(Ball ball)
    {
        if (ball == null)
            return;

        Goal nearestGoal = Goal.GetNearestGoal(ball.GetPosition());
        
        if (nearestGoal == null)
        {
            Debug.LogError("❌ Không tìm thấy khung thành!");
            return;
        }

        Vector3 ballPosition = ball.GetPosition();
        Vector3 goalPosition = nearestGoal.transform.position;
        Vector3 kickDirection = (goalPosition - ballPosition).normalized;

        ball.KickToNearestGoal(kickDirection);
    }
}