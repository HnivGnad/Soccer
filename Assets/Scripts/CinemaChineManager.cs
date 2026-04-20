using UnityEngine;
using Cinemachine;
using System.Collections;

public class CinemaChineManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera ballCamera;
    [SerializeField] private float ballCameraPriority = 15f;
    [SerializeField] private float playerCameraPriority = 10f;
    [SerializeField] private float transitionDuration = 0.5f;

    private Coroutine transitionCoroutine;

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = FindFirstObjectByType<CinemachineVirtualCamera>();

        playerCamera.Priority = (int)playerCameraPriority;
        ballCamera.Priority = (int)playerCameraPriority - 5;
    }

    public void FollowBall(Transform ball)
    {
        if (ballCamera == null)
            return;

        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        ballCamera.Follow = ball;
        ballCamera.LookAt = ball;

        ballCamera.Priority = (int)ballCameraPriority;
        playerCamera.Priority = (int)playerCameraPriority;

    }
    public void ReturnToPlayer()
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(TransitionToPlayer());
    }

    public void ReturnToPlayerAfterDelay(float delay)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(WaitThenReturnToPlayer(delay));
    }

    private IEnumerator TransitionToPlayer()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            ballCamera.Priority = (int)Mathf.Lerp(ballCameraPriority, playerCameraPriority - 5, t);

            yield return null;
        }

        ballCamera.Priority = (int)(playerCameraPriority - 5);
        playerCamera.Priority = (int)playerCameraPriority;

    }

    private IEnumerator WaitThenReturnToPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(TransitionToPlayer());
    }
}