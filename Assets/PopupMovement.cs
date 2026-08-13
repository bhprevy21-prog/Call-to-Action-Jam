using UnityEngine;

public class PopupMovement : MonoBehaviour
{
    public float moveSpeed = 300f;

    private RectTransform rectTransform;
    private RectTransform spawnArea;
    private Vector2 targetPosition;

    public void Setup(RectTransform area)
    {
        rectTransform = GetComponent<RectTransform>();
        spawnArea = area;

        PickNewTarget();
    }

    private void Update()
    {
        if (rectTransform == null || spawnArea == null)
            return;

        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 5f)
        {
            PickNewTarget();
        }
    }

    private void PickNewTarget()
    {
        float x = Random.Range(
            -spawnArea.rect.width / 2f,
            spawnArea.rect.width / 2f
        );

        float y = Random.Range(
            -spawnArea.rect.height / 2f,
            spawnArea.rect.height / 2f
        );

        targetPosition = new Vector2(x, y);
    }
}
