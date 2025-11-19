using UnityEngine;

public class SaveFunctions : MonoBehaviour
{
    public void SaveF()
    {
        GameManager.instance.ButtonSave();
    }

    public void LoadF()
    {
        GameManager.instance.LoadButton(true);
    }
}
