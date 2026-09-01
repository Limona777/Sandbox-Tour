using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Condition Config")]
public class ConditionConfigSO : ScriptableObject
{
    [System.Serializable]
    public class ConditionData
    {
        public string conditionID;
        public string displayName;
        public Sprite icon;
    }

    public List<ConditionData> conditions = new List<ConditionData>();

    public string GetDisplayName(string id)
    {
        var data = conditions.Find(c => c.conditionID == id);
        return data != null ? data.displayName : id;
    }
}