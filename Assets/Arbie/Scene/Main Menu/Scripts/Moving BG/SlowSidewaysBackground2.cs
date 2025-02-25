using UnityEngine;

public class FastSidewaysBackground : MonoBehaviour
{
    public float moveRange = 2f; // Move left and right by 2 units
    public float moveDuration = 10f; // Complete movement in 10 seconds

    private float startX;
    private float targetX;
    private float elapsedTime = 0f;
    private bool movingRight = true;

    void Start()
    {
        startX = transform.position.x;
        targetX = startX + moveRange;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsedTime / moveDuration);

        // Smooth easing
        float newX = Mathf.Lerp(startX, targetX, Mathf.SmoothStep(0f, 1f, progress));
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Change direction when the target is reached
        if (progress >= 1f)
        {
            movingRight = !movingRight;
            startX = transform.position.x;
            targetX = movingRight ? startX + moveRange : startX - moveRange;
            elapsedTime = 0f;
        }
    }
}
