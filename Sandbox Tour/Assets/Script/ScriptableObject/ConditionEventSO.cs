using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/Condition Event Channel")]
public class ConditionEventSO : ScriptableObject
{
    public UnityAction<string> OnConditionRaised;

    public void RaiseEvent(string conditionID)
    {
        OnConditionRaised?.Invoke(conditionID);
    }
}