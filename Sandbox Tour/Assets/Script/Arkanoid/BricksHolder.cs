using System.Collections;
using UnityEngine;

public class BricksHolder : MonoBehaviour
{
    int bricksAmount;

    public GameObject winPanel;

    public ReturnToPrevSceneEventSO returnEvent;

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (!child.GetComponent<Brick>().isGoldBrick)
                bricksAmount++;
        }

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void BrickGetDestroy()
    {
        bricksAmount--;
        if (bricksAmount <= 0)
        {
            print("You Win!");
            if (winPanel != null)
            {
                winPanel.SetActive(true);
                GameWorldPauser.Pause();
            }

            string bossID = BossEncounter.currentBossID;
            if (!string.IsNullOrEmpty(bossID))
            {
                ConditionManager.Instance?.SetCondition("BossDefeated_" + bossID);
            }

            Invoke(nameof(ReturnToPreviousScene), 1f);
        }
    }

    private void ReturnToPreviousScene()
    {
        GameWorldPauser.Resume();
        BossEncounter.currentBossID = null;
        returnEvent?.RaiseEvent();
    }
}