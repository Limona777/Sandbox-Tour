using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    [Header("Load")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;

    [Header("Conditions")]
    public string[] requiredConditions;
    public bool requireAllConditions = true;
    public ConditionConfigSO config;

    public ConditionEventSO conditionEvent;

    [Header("UI")]
    public GameObject hintPanel;
    public Text hintText;

    private bool isUnlocked = false;

    private void OnEnable()
    {
        if (conditionEvent != null)
        {
            conditionEvent.OnConditionRaised += OnConditionUpdated;
        }
        RefreshState();
    }

    private void OnDisable()
    {
        if (conditionEvent != null)
        {
            conditionEvent.OnConditionRaised -= OnConditionUpdated;
        }
    }

    private void OnConditionUpdated(string conditionID)
    {
        RefreshState();
    }

    private void RefreshState()
    {
        isUnlocked = ConditionManager.Instance != null &&
                     ConditionManager.Instance.CheckConditions(requiredConditions, requireAllConditions);
    }

    public void TriggerAction()
    {
        RefreshState();

        if (isUnlocked)
        {
            loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
        }
        else
        {
            ShowHintPanel();
        }
    }

    private void ShowHintPanel()
    {
        if (hintPanel == null || hintText == null)
        {
            Debug.LogError("Lack Hint Panel & Hint Text！");
            return;
        }

        List<string> missingConditions = new List<string>();
        foreach (var id in requiredConditions)
        {
            if (ConditionManager.Instance != null && !ConditionManager.Instance.CheckConditions(new string[] { id }, true))
            {
                string displayName = config != null ? config.GetDisplayName(id) : id;
                missingConditions.Add(displayName);
            }
        }

        string finalText = "去到下一个地方前，似乎还有些地方需要探索。\n\n";
        //foreach (var item in missingConditions)
        //{
        //    finalText += $"• {item}\n";
        //}

        hintText.text = finalText;

        hintPanel.SetActive(true);

        CancelInvoke();
        Invoke("CloseHintPanel", 3.0f);
    }

    public void CloseHintPanel()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }
    }
}