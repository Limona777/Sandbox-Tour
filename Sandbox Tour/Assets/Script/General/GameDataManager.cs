using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    private Dictionary<string, bool> boxStates = new Dictionary<string, bool>();

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

    public void SetBoxState(string boxID, bool isOpen)
    {
        boxStates[boxID] = isOpen;
    }

    public bool GetBoxState(string boxID)
    {
        return boxStates.ContainsKey(boxID) && boxStates[boxID];
    }

    public void ClearAllData()
    {
        boxStates.Clear();
    }
}