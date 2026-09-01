using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrologueSystem : MonoBehaviour
{
    [Header("UI")]
    public GameObject prologuePanel;
    public Image backgroundImage;
    public Image textBackgroundImage;
    public Text dialogueText;

    [Header("Text")]
    public TextAsset textFile;
    public float textSpeed = 0.05f;

    public UnityEngine.Events.UnityEvent onPrologueEnd;

    private List<string> textLines = new List<string>();
    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool isFinished = false;

    private void Awake()
    {
        if (prologuePanel != null)
            prologuePanel.SetActive(false);
    }

    public void PlayPrologue()
    {
        if (prologuePanel == null || textFile == null)
        {
            Debug.LogWarning("PrologueSystem: Lack UI");
            onPrologueEnd?.Invoke();
            return;
        }

        LoadTextFromFile();
        currentLineIndex = 0;
        isFinished = false;
        prologuePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    private void LoadTextFromFile()
    {
        textLines.Clear();
        string[] lines = textFile.text.Split('\n');
        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            if (!string.IsNullOrEmpty(trimmed))
                textLines.Add(trimmed);
        }
    }

    private IEnumerator TypeLine()
    {
        if (currentLineIndex >= textLines.Count)
        {
            EndPrologue();
            yield break;
        }

        isTyping = true;
        dialogueText.text = "";
        string line = textLines[currentLineIndex];

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.I));
        currentLineIndex++;
        StartCoroutine(TypeLine());
    }

    private void EndPrologue()
    {
        isFinished = true;
        prologuePanel.SetActive(false);
        onPrologueEnd?.Invoke();
    }
}