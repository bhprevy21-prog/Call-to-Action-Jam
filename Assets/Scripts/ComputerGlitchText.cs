using UnityEngine;
using TMPro;
using System.Collections;

public class ComputerGlitchText : MonoBehaviour
{
    public TMP_Text computerText;

    private string originalText = "Hello Gui Job, you have one new notification.";

    private string characters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
        "abcdefghijklmnopqrstuvwxyz" +
        "0123456789" +
        "!@#$%^&*()_+-=[]{}|;:',.<>?/\\~`";

    private Coroutine glitchCoroutine;

    void OnEnable()
    {
       
        StartGlitch();
    }

    void OnDisable()
    {
        // Stop the glitch when the panel closes
        if (glitchCoroutine != null)
        {
            StopCoroutine(glitchCoroutine);
            glitchCoroutine = null;
        }
    }

    void StartGlitch()
    {
        if (computerText == null)
            return;

        if (glitchCoroutine != null)
            StopCoroutine(glitchCoroutine);

        glitchCoroutine = StartCoroutine(GlitchText());
    }

    IEnumerator GlitchText()
    {
        while (true)
        {
            string glitch = "";

            int length = Random.Range(20, 70);

            for (int i = 0; i < length; i++)
            {
                glitch += characters[Random.Range(0, characters.Length)];
            }

            computerText.text = glitch;

         
            yield return new WaitForSeconds(Random.Range(0.03f, 0.12f));
        }
    }
}