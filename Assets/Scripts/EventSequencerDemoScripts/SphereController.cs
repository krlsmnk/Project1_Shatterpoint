using UnityEngine;

public class SphereController : MonoBehaviour
{
    public float moveDuration = 0.5f; // Duration of the movement
    public float moveDistance = 5f;   // Distance of the movement

    private Vector3 targetPosition;
    private bool isMoving = false;

    private void OnEnable()
    {
        EventManager.Instance.AddListener("move.up", OnMoveUp);
        EventManager.Instance.AddListener("move.down", OnMoveDown);
        EventManager.Instance.AddListener("move.left", OnMoveLeft);
        EventManager.Instance.AddListener("move.right", OnMoveRight);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener("move.up", OnMoveUp);
        EventManager.Instance.RemoveListener("move.down", OnMoveDown);
        EventManager.Instance.RemoveListener("move.left", OnMoveLeft);
        EventManager.Instance.RemoveListener("move.right", OnMoveRight);
    }

    private void OnMoveUp(AudioClip clip, float duration)
    {
        moveDuration = duration;
        StartMove(Vector3.up);
    }

    private void OnMoveDown(AudioClip clip, float duration)
    {
        moveDuration = duration;
        StartMove(Vector3.down);
    }

    private void OnMoveLeft(AudioClip clip, float duration)
    {
        moveDuration = duration;
        StartMove(Vector3.left);
    }

    private void OnMoveRight(AudioClip clip, float duration)
    {
        moveDuration = duration;
        StartMove(Vector3.right);
    }

    private void StartMove(Vector3 direction)
    {
        if (!isMoving)
        {
            targetPosition = transform.position + direction * moveDistance;
            StartCoroutine(SmoothMove(targetPosition));
        }
    }

    private System.Collections.IEnumerator SmoothMove(Vector3 target)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, target, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }
}
