using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScroller : MonoBehaviour
{
    public RectTransform contentPanel; // Assign the LevelContainer
    public float moveAmount = 1850f; // Adjusted to your value
    public float moveSpeed = 6f; // Adjusted to your value

    private Vector2 targetPosition;
    private int currentIndex = 0;
    private int minIndex = 0;
    private int maxIndex = 2; // Limit to 2 right swipes (total 3 slides)

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;

    void Start()
    {
        targetPosition = contentPanel.anchoredPosition;
    }

    void Update()
    {
        // Smoothly move towards the target position
        contentPanel.anchoredPosition = Vector2.Lerp(contentPanel.anchoredPosition, targetPosition, Time.deltaTime * moveSpeed);

        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isSwiping = true;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    DetectSwipe();
                    isSwiping = false;
                    break;
            }
        }
    }

    void DetectSwipe()
    {
        float swipeDistance = endTouchPosition.x - startTouchPosition.x;
        float swipeThreshold = 100f; // Minimum distance to register a swipe

        if (Mathf.Abs(swipeDistance) > swipeThreshold)
        {
            if (swipeDistance > 0) // Swipe Right
                MoveLeft();
            else // Swipe Left
                MoveRight();
        }
    }

    public void MoveLeft()
    {
        if (currentIndex > minIndex)
        {
            currentIndex--;
            targetPosition += new Vector2(moveAmount, 0);
        }
    }

    public void MoveRight()
    {
        if (currentIndex < maxIndex)
        {
            currentIndex++;
            targetPosition -= new Vector2(moveAmount, 0);
        }
    }
}
