using UnityEngine;

public class LightCatcherReference : MonoBehaviour
{
    [SerializeField] private LightCatcher lightCatcher;
    public LightCatcher LightCatcher => lightCatcher;
}
