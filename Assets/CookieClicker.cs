using UnityEngine;
using UnityEngine.UI;

public class CookieClicker : MonoBehaviour
{
    [Header("Cookie Settings")]
    public int clicksNeeded = 10000;
    private int currentClicks = 0;

    [Header("Progress Bar")]
    public Slider progressBar;

    private void Start()
    {
        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = clicksNeeded;
            progressBar.value = 0;
        }
    }

    public void ClickCookie()
    {
        currentClicks++;

        if (progressBar != null)
        {
            progressBar.value = currentClicks;
        }

        Debug.Log("Cookie clicked! " + currentClicks + "/" + clicksNeeded);

        if (currentClicks >= clicksNeeded)
        {
            CookieComplete();
        }
    }

    private void CookieComplete()
    {
        Debug.Log("COOKIE COMPLETE!");

        // For now, reset the cookie.
        currentClicks = 0;

        if (progressBar != null)
        {
            progressBar.value = 0;
        }
    }
}