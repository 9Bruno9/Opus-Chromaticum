using System.Collections;
using UnityEngine;

public class ChangeWaterColor : MonoBehaviour
{
    [SerializeField] private float transitionTime = 2f;
    
    private Renderer[] _renderers;
    private Color[] _initialColors;

    private float _targetPoint;

    private void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        
        //Update on save-file load
        if (GameManager.instance.BossDestroyed[1])
        {
            ChangeColorToBlue();
        }
    }
    
    /// <summary>
    /// Call this function to change the color of all the water in the scene
    /// </summary>
    /// <param name="color">The target color</param>
    public void ChangeColor(Color color)
    {
        _initialColors = new Color[_renderers.Length];
        for (var i = 0; i < _renderers.Length; i++)
        {
            _initialColors[i] = _renderers[i].material.GetColor("_BaseColor");
        }
        StartCoroutine(ChangeColorRoutine(color));
    }

    private IEnumerator ChangeColorRoutine(Color color)
    {
        while (true)
        {
            _targetPoint += Time.deltaTime / transitionTime;
            for (var i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].material.SetColor("_BaseColor", Color.Lerp(_initialColors[i], color, _targetPoint)); 
            }

            if (_targetPoint >= 1)
            {
                yield break; //Works just like a return;
            }
            yield return null;
        }
    }

    
    public void ChangeColorToBlue()
    {
        //ChangeColor(new Color(0f,0.6f,0.8f));
        ChangeColor(new Color(0.023f,0.77f,1f,0.3f));
    }
}