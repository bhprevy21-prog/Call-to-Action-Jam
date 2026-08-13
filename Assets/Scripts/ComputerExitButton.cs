using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ComputerExitButton : MonoBehaviour
{
    public float fadeDuration = 3f;

    private Image buttonImage;
    private Button button;

    void OnEnable()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();

        // Start completely invisible
        Color color = buttonImage.color;
        color.a = 0f;
        buttonImage.color = color;

        // Prevent clicking while fading in
        button.interactable = false;

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;

        Color color = buttonImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Clamp01(timer / fadeDuration);

            color.a = alpha;
            buttonImage.color = color;

            yield return null;
        }

        // Make sure it's completely visible
        color.a = 1f;
        buttonImage.color = color;

        // Allow clicking
        button.interactable = true;
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}