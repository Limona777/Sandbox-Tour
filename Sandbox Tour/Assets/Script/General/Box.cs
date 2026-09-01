using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    [Header("Save UI")]
    public GameObject savePanelPrefab;

    [Header("Box")]
    public string boxID;
    public Sprite closedSprite;
    public Sprite openedSprite;

    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private bool isOpened = false;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        spriteRenderer = GetComponent<SpriteRenderer>();

        isOpened = PlayerPrefs.GetInt("BoxOpened_" + boxID, 0) == 1;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.sprite = isOpened ? openedSprite : closedSprite;
    }

    public void TriggerAction()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Unable to find player location, unable to save.");
            return;
        }

        Vector3 playerPos = playerTransform.position;

        HashSet<string> conditions = ConditionManager.Instance?.GetAllCompletedConditions()
                                     ?? new HashSet<string>();

        string currentSceneName = GetCurrentSceneIdentifier();

        SaveData(playerPos, conditions, currentSceneName);

        if (!isOpened)
        {
            isOpened = true;
            PlayerPrefs.SetInt("BoxOpened_" + boxID, 1);
            PlayerPrefs.Save();
            UpdateAppearance();
        }

        if (savePanelPrefab != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                GameObject panel = Instantiate(savePanelPrefab, canvas.transform);
                panel.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Archive successful! (Prefabricated panel not configured)");
        }
    }

    private string GetCurrentSceneIdentifier()
    {
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader != null && loader.CurrentLoadedScene != null)
            return loader.CurrentLoadedScene.name;
        return "UnknownScene";
    }

    private void SaveData(Vector3 pos, HashSet<string> conditions, string sceneName)
    {
        PlayerPrefs.SetFloat("Save_PosX", pos.x);
        PlayerPrefs.SetFloat("Save_PosY", pos.y);
        PlayerPrefs.SetFloat("Save_PosZ", pos.z);
        PlayerPrefs.SetString("Save_SceneName", sceneName);
        string condStr = string.Join(",", conditions);
        PlayerPrefs.SetString("Save_Conditions", condStr);
        PlayerPrefs.Save();
        Debug.Log($"Save：Scene={sceneName}，Position({pos.x:F2},{pos.y:F2},{pos.z:F2})，Condition={conditions.Count}");
    }
}