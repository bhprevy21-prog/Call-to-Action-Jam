using UnityEngine;

public class ComputerInteraction : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject computerPanel;

    public PlayerMovement playerMovement;

    public bool hasStarted = false;
    public bool power = true;

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

        // Computer has no power
        if (!power)
            return;

        // Stop player movement
        playerMovement.canMove = false;

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

    // Called by the button inside StartPanel
    public void ReadEmail()
    {
        // Close the email
        startPanel.SetActive(false);

        // Shut off the computer
        power = false;

        // Remember that the email was read
        hasStarted = true;

        // Give player movement back
        playerMovement.canMove = true;
    }

    // DEBUG BUTTON
    public void TogglePower()
    {
        power = !power;

        Debug.Log("Computer power: " + power);
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