using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/ReturnToPrevSceneEventSO")]
public class ReturnToPrevSceneEventSO : ScriptableObject
{
    public UnityAction OnReturnRequested;

    public void RaiseEvent()
    {
        OnReturnRequested?.Invoke();
    }
}