
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class cutsceneManager : UdonSharpBehaviour
{
    public Animator animator;
    public TextMeshProUGUI dialogueText;
    public GameObject healthText;
    [TextArea(2, 5)]
    public string[] dialogueLines;

    private int currentLineIndex = 0;
    private string currentLine = "";
    private int currentCharIndex = 0;
    public float letterDelay = 0.05f;

    private bool isTyping = false;
    private bool enterCutscene = false;
    private bool hasStartedDialogue = false;

    void Start()
    {
        dialogueText.text = "";
    }

    public override void Interact()
    {
        if (enterCutscene) return; // prevent double triggers

        animator.Play("FadeOut");
        enterCutscene = true;
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (enterCutscene && !hasStartedDialogue && stateInfo.IsName("FadeOut") && stateInfo.normalizedTime >= 1.0f)
        {
            hasStartedDialogue = true;
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        currentLineIndex = 0;
        currentLine = dialogueLines[currentLineIndex];
        currentCharIndex = 0;
        dialogueText.text = "";
        isTyping = true;
        healthText.SetActive(false);

        ShowNextCharacter();
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

            // Wait 1 second, then continue to next line (or fade out)
            SendCustomEventDelayedSeconds(nameof(ContinueToNextLine), 1f);
        }
    }

    public void ContinueToNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            currentLine = dialogueLines[currentLineIndex];
            currentCharIndex = 0;
            dialogueText.text = ""; // Clear the previous line
            isTyping = true;
            ShowNextCharacter();
        }
        else
        {
            // All lines done — fade in
            EndCutscene();
        }
    }


    public void EndCutscene()
    {
        animator.Play("FadeIn");
        healthText.SetActive(true);
        dialogueText.text = "";
        enterCutscene = false;
        hasStartedDialogue = false;
    }
}
