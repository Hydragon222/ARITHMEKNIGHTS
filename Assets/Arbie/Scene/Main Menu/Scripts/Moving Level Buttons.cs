using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScroller : MonoBehaviour
{
    public RectTransform contentPanel; // Assign the LevelContainer
    public float moveAmount = 200f; // Distance to move each time
    public float moveSpeed = 10f; // Smooth movement speed

    private Vector2 targetPosition;

    void Start()
    {
        targetPosition = contentPanel.anchoredPosition;
    }

    void Update()
    {
        // Smoothly move towards the target position
        contentPanel.anchoredPosition = Vector2.Lerp(contentPanel.anchoredPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

    public void MoveLeft()
    {
        targetPosition += new Vector2(moveAmount, 0);
    }

    public void MoveRight()
    {
        targetPosition -= new Vector2(moveAmount, 0);
    }
}

