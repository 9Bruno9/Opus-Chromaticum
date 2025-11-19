using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    


    private bool iswriting;


    public int index;

    public GameEvent dialogueON;
    public GameEvent dialogueOFF;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame

    private void OnEnable()
    {
        dialogueON.TriggerEvent();
    }

    private void OnDisable()
    {
        dialogueOFF.TriggerEvent();
        GameManager.instance.isAMenuOpen = false;
    }



    public void StartDialogue()
    {
        if (iswriting==false)
        { 
            iswriting = true;
            textComponent.text = string.Empty;
            index = 0;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (textComponent.text == lines[index])
            {
                nextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                //iswriting = false;
            }
        }
    }

    IEnumerator TypeLine()
    {
        // type each character 1 by 1
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void ResetDialogue()
    {
        textComponent.text = string.Empty;
        lines = new string[0];
        index = 0;
        StopAllCoroutines();
        GameManager.instance.isAMenuOpen = false;
    }

    void nextLine()
    {
        if (index < lines.Length -1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            iswriting = false;
            gameObject.SetActive(false);

        }
    }
}
