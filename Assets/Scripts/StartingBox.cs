using UnityEngine;

public class StartingBox : MonoBehaviour
{
    public GameEvent DialogON;
    public GameEvent DialogOFF;

    [SerializeField] private PlayerMr player;
    
    private void OnEnable()
    {
        player.blockMovement = true;
        DialogON.TriggerEvent();
        if (GameManager.instance.nuovapartita == false)
        {
            GameManager.instance.isAMenuOpen = false;
            gameObject.SetActive(false);
            return;
        }
        GameManager.instance.nuovapartita = false;
    }

    private void OnDisable()
    {
        player.blockMovement = false;
        DialogOFF.TriggerEvent();
    }
}
