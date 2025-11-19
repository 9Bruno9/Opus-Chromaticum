using System.Collections;
using UnityEngine;

public class DiscoveryFadeIn : MonoBehaviour
{
    [SerializeField] private RectTransform itemToFadeIn;
    [SerializeField] private float fadeDuration = 1f;

    private void OnDisable()
    {
        GameManager.instance.isAMenuOpen = false;
    }

    public void StartFadeIn()
    {
        GameManager.instance.isAMenuOpen = true;
        gameObject.SetActive(true);
        itemToFadeIn.localScale = Vector3.zero;
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        var newScaleF = 0f;
        var newScale = new Vector3(0, 0, 0);
        var increment = 1 / (fadeDuration / Time.fixedDeltaTime);
        while (newScaleF < 1f)
        {
            itemToFadeIn.localScale = newScale;
            newScaleF = Mathf.Min(1, newScaleF + increment);
            newScale.x = newScaleF;
            newScale.y = newScaleF;
            newScale.z = newScaleF;
            itemToFadeIn.localScale = newScale;
            yield return new WaitForFixedUpdate();
        }
    }
}
