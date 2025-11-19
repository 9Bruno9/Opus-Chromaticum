using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueStaringBox : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    InputAction InteractionInput;
    private int index;

    [SerializeField] private PlayerMr player;

    // Start is called before the first frame update
    void Start()
    {
        player.blockMovement = true;
        GameManager.instance.isAMenuOpen = true;
        textComponent.text = string.Empty;
        StartDialogue();
        InteractionInput = InputSystem.actions.FindAction("Interact");
    }


    private void OnDisable()
    {
        player.blockMovement = false;
        GameManager.instance.isAMenuOpen = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (InteractionInput.WasPressedThisFrame())
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            GameManager.instance.isAMenuOpen = false;
        }
    }


 

}

