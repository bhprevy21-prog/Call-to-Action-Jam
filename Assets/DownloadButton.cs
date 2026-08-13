using UnityEngine;
using UnityEngine.UI;

public class DownloadButton : MonoBehaviour
{
    private DangerDownloading dangerDownloading;
    private bool isReal;

    public void Setup(
        DangerDownloading eventManager,
        bool realButton
    )
    {
        dangerDownloading = eventManager;
        isReal = realButton;
    }

    public void ClickButton()
    {
        if (dangerDownloading == null)
            return;

        if (isReal)
        {
            dangerDownloading.RealButtonClicked();
        }
        else
        {
            dangerDownloading.FakeButtonClicked(gameObject);
        }
    }
}