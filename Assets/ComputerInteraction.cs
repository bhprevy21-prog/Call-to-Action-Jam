using UnityEngine;
using UnityEngine.UI;

public class ComputerInteraction : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject computerPanel;

    public PlayerMovement playerMovement;

    // The UI button that calls Interact()
    public Button interactButton;

    public bool hasStarted = false;

    private bool playerInRange = false;

    void Start()
    {
        startPanel.SetActive(false);
        computerPanel.SetActive(false);
    }

    public void Interact()
    {
        if (!playerInRange)
            return;

        // Stop player movement
        playerMovement.canMove = false;

        // Hide the interact button ONLY when interaction happens
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
        }

        // Hide both panels first
        startPanel.SetActive(false);
        computerPanel.SetActive(false);

        // Open the correct panel
        if (hasStarted)
        {
            computerPanel.SetActive(true);
        }
        else
        {
            startPanel.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}