using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public RectTransform box;
    private Vector2 boxPosition;
    [Range(0f, 1f)]public float defaultTextSpeed = 0.95f;
    public string[] textMessage;

    private int textIndex;

    private string currentLine;
    private bool onTween = false;

    [HideInInspector] public Tutorial tutorial;

    // Start is called before the first frame update
    void Start()
    {
        boxPosition = box.anchoredPosition;
        // StartDialogue();
    }

    public void Interaction()
    {
        if (!onTween)
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
        if (!gameObject.activeInHierarchy)
        {
            Tween(true, textMessage[textIndex]);
        }
        else
        {
            StartCoroutine(TypeLine(textMessage[textIndex]));
        }
    }

    public void NextLine()
    {
        if(textIndex <= textMessage.Length)
        {
            textIndex++;
            text.text = "";
            if (!gameObject.activeInHierarchy)
            {
                Tween(true, textMessage[textIndex]);
            }
            else
            {
                StartCoroutine(TypeLine(textMessage[textIndex]));
            }
        }
        else
        {
            Debug.LogError("dialogue close");
            Close();
        }
    }

    public void SimpleLine(string line)
    {
        if (!gameObject.activeInHierarchy)
        {
            text.text = "";
            Tween(true, line);
        }
        else
        {
            text.text = "";
            StartCoroutine(TypeLine(line));
        }
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
        if (gameObject.activeInHierarchy)
        {
            Tween(false);
        }
    }

    void Tween(bool on = true, string line = "")
    {
        onTween = true;

        if (on)
        {
            gameObject.SetActive(true);
            box.anchoredPosition = new Vector2(0, -500f);

            if (line != "")
                StartCoroutine(TypeLine(line));

            box.DOAnchorPos(new Vector2(0, 50f), 0.5f, false)
                .SetEase(Ease.OutQuint)
                .OnComplete(() =>
                {
                    onTween = false;
                });
        }
        else
        {
            box.DOAnchorPos(new Vector2(0, -500f), 0.5f, false)
                .SetEase(Ease.InQuint)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    onTween = false;
                });
        }
    }
}
