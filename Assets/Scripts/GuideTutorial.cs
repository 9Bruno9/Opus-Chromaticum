using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GuideTutorial : MonoBehaviour

{
    public GameObject chatBox;
    public string[] coseDaDire;
    private float distancetoplayer;
    public Transform player;
    private bool isSpeaking;
    private Dialogue DialogueComp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").transform;
        DialogueComp = chatBox.GetComponent<Dialogue>();
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (isSpeaking && chatBox.activeSelf && (Vector3.Distance(transform.position, player.position) > 10))
        {
            DialogueComp.ResetDialogue(); 
            chatBox.SetActive(false);
            isSpeaking = false;
        }
        if (isSpeaking && (Vector3.Distance(transform.position, player.position) > 10)) isSpeaking = false;
    }
    */

    public void inizio_dialogo()
    {
        if (!chatBox.activeSelf && GameManager.instance.isAMenuOpen) return;
        
        GameManager.instance.isAMenuOpen = true;
        
        isSpeaking = true;
        chatBox.SetActive(true);
        DialogueComp.lines = coseDaDire;
        DialogueComp.StartDialogue();
        
        
    }


}
