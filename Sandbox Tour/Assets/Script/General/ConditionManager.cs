using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    public static ConditionManager Instance;

    private HashSet<string> completedConditions = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCondition(string conditionID)
    {
        if (!string.IsNullOrEmpty(conditionID))
        {
            completedConditions.Add(conditionID);
            Debug.Log($"<color=green>[Conditions met]</color> {conditionID}");
        }
    }

    public bool CheckConditions(string[] conditionIDs, bool requireAll = true)
    {
        if (conditionIDs == null || conditionIDs.Length == 0) return true;

        if (requireAll)
        {
            foreach (var id in conditionIDs)
            {
                if (!completedConditions.Contains(id)) return false;
            }
            return true;
        }
        else
        {
            foreach (var id in conditionIDs)
            {
                if (completedConditions.Contains(id)) return true;
            }
            return false;
        }
    }

    public HashSet<string> GetAllCompletedConditions()
    {
        return new HashSet<string>(completedConditions);
    }
}