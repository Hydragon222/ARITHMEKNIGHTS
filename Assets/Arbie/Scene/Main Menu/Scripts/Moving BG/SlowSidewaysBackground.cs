using UnityEngine;

public class SlowSidewaysBackground : MonoBehaviour
{
    public float moveRange = 5f; // Distance to move left and right
    public float moveDuration = 300f; // Time in seconds (5 minutes)

    private float startX;
    private float targetX;
    private float elapsedTime = 0f;
    private bool movingRight = true;

    void Start()
    {
        startX = transform.position.x;
        targetX = startX + moveRange; // Set initial target
    }

    void Update()
    {
        // Calculate progress (0 to 1) based on elapsed time
        elapsedTime += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsedTime / moveDuration);

        // Smooth interpolation (ease-in and ease-out effect)
        float newX = Mathf.Lerp(startX, targetX, Mathf.SmoothStep(0f, 1f, progress));
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // When the background reaches the target, swap direction
        if (progress >= 1f)
        {
            movingRight = !movingRight;
            startX = transform.position.x;
            targetX = movingRight ? startX + moveRange : startX - moveRange;
            elapsedTime = 0f; // Reset timer
        }
    }
}
