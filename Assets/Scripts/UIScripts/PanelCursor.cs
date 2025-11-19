using UnityEngine;

public class PanelCursor : MonoBehaviour
{

    public GameEvent CursorOn;
    public GameEvent CursorOff;
    public bool IsThisStartmenu;
    
    private void OnEnable()
    {
        CursorOn.TriggerEvent();

        if (IsThisStartmenu) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnDisable()
    {
        CursorOff.TriggerEvent();
    }
}
