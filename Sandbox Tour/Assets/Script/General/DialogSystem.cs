using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI")]
    public Text textLabel;
    public Image faceImage;

    [Header("Text")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;

    [Header("Faces")]
    public Sprite[] faces;

    [Header("Condition")]
    public string dialogConditionID;
    public ConditionEventSO conditionEvent;

    bool textFinished;
    bool cancelTyping;

    List<string> textList = new List<string>();

    void Awake()
    {
        GetTextFromFile(textFile);
    }

    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(SetTextUI());
    }

    private void FinishDialog()
    {
        if (!string.IsNullOrEmpty(dialogConditionID))
        {
            ConditionManager.Instance?.SetCondition(dialogConditionID);
            conditionEvent?.RaiseEvent(dialogConditionID);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && index == textList.Count)
        {
            FinishDialog();
            gameObject.SetActive(false);
            index = 0;
            return;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (textFinished && !cancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished && !cancelTyping)
            {
                cancelTyping = true;
            }
        }
    }

    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineData = file.text.Split('\n');
        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";

        if (int.TryParse(textList[index], out int faceIndex))
        {
            if (faceIndex >= 0 && faceIndex < faces.Length)
            {
                faceImage.sprite = faces[faceIndex];
            }
            else
            {
                faceImage.sprite = faces.Length > 0 ? faces[0] : null;
            }
            index++;
        }

        int letter = 0;
        while (!cancelTyping && letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
    }
}