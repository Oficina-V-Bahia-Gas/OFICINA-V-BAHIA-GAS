using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    [Range(0f, 1f)]public float defaultTextSpeed = 0.95f;
    public string[] textMessage;

    private int textIndex;

    private string currentLine;

    [HideInInspector] public Tutorial tutorial;

    // Start is called before the first frame update
    void Start()
    {
        StartDialogue();
    }

    public void Interaction()
    {
        if (text.text != currentLine)
        {
            StopAllCoroutines();
            text.text = currentLine;
            return;
        }else if (tutorial != null)
        {
            Debug.LogError("interact"); 
            tutorial.DialogueReturn();
        }
        else
        {
            NextLine();
        }
    }

    public void StartDialogue(string[] texts = null)
    {
        gameObject.SetActive(true);
        textIndex = 0;
        text.text = "";

        if (texts == null) 
        {
            textMessage = texts;
        }
        StartCoroutine(TypeLine(textMessage[textIndex]));
    }

    public void NextLine()
    {
        if(textIndex <= textMessage.Length)
        {
            textIndex++;
            text.text = "";
            StartCoroutine(TypeLine(textMessage[textIndex]));
        }
        else
        {
            Debug.LogError("dialogue close");
            Close();
        }
    }

    public void SimpleLine(string line)
    {
        gameObject.SetActive(true);
        text.text = "";
        StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        currentLine = line;
        foreach(char c in line.ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(1 - defaultTextSpeed);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
