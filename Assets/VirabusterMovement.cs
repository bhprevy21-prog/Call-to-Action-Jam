using UnityEngine;

public class VirabusterMovement : MonoBehaviour
{
    private EventManager eventManager;

    private GameObject targetVirus;

    private float moveSpeed;

    public void Setup(
        EventManager manager,
        GameObject target,
        float speed
    )
    {
        eventManager = manager;
        targetVirus = target;
        moveSpeed = speed;
    }

    private void Update()
    {
        if (targetVirus == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move toward the virus.
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetVirus.transform.position,
            moveSpeed * Time.deltaTime
        );

        // Reached the virus.
        if (Vector3.Distance(
            transform.position,
            targetVirus.transform.position
        ) < 10f)
        {
            DestroyVirus();

            Destroy(gameObject);
        }
    }

    private void DestroyVirus()
    {
        if (eventManager != null &&
            targetVirus != null)
        {
            // This is exactly the same function
            // used when the player clicks a virus.
            eventManager.VirusClicked(
                targetVirus
            );

            Debug.Log(
                "VIRABUSTER: Virus destroyed!"
            );
        }
    }
}