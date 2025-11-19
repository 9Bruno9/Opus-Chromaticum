using UnityEngine;

namespace DebugScripts
{
    public class EventInvoker : MonoBehaviour
    {
        [SerializeField] private GameEvent eventToInvoke;

        [ContextMenu("Invoke!")]
        public void InvokeEvent()
        {
            eventToInvoke.TriggerEvent();
        }
    }
}
