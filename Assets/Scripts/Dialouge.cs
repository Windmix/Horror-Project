using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Dialouge : UdonSharpBehaviour
{
    public TextMeshProUGUI dialogueText;
    [TextArea(2, 5)]
    
    public string[] dialogueLines;
    private int currentLineIndex = 0;
    private string currentLine = "";
    private int currentCharIndex = 0;
    public float letterDelay = 0.05f;

    private bool isTyping = false;
    void Start()
    {
        dialogueText.text = "";

    }
   
    //}

    public override void Interact()
    {
        if (isTyping) return;

        currentLine = dialogueLines[currentLineIndex];
        currentCharIndex = 0;
        dialogueText.text = "";
        isTyping = true;
        ShowNextCharacter();

        currentLineIndex = (currentLineIndex + 1) % dialogueLines.Length;
    }
    public void ShowNextCharacter()
    {
        if (currentCharIndex < currentLine.Length)
        {
            dialogueText.text += currentLine[currentCharIndex];
            currentCharIndex++;
            SendCustomEventDelayedSeconds(nameof(ShowNextCharacter), letterDelay);
        }
        else
        {
            isTyping = false;
        }
    }
}
