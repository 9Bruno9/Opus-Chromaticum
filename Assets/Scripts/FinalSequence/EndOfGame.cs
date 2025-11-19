using System.Collections;
using TMPro;
using UnityEngine;

public class EndOfGame : MonoBehaviour
{
    [SerializeField] private GameObject finalPanel;
    [SerializeField] private GameObject mainMenuButton;


    public void EndGame()
    {
        GameManager.instance.isAMenuOpen = true;
        mainMenuButton.SetActive(false);
        StartCoroutine(DelayedPopUp());
    }

    private IEnumerator DelayedPopUp()
    {
        var buttonText = mainMenuButton.GetComponentInChildren<TMP_Text>();
        var currentColor = buttonText.color;
        var currentAlpha = 0f;
        yield return new WaitForSecondsRealtime(5f);
        mainMenuButton.SetActive(true);
        currentColor = new Color(currentColor.r,currentColor.g,currentColor.b,0);
        buttonText.color = currentColor;
        while (currentAlpha < 1f)
        {
            currentAlpha = Mathf.Min(currentAlpha + 0.01f, 1f);
            currentColor.a = currentAlpha;
            buttonText.color = currentColor;
            yield return new WaitForFixedUpdate();
        }
    }
}
