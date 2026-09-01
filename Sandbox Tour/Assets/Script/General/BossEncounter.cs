using UnityEngine;
using UnityEngine.UI;

public class BossEncounter : MonoBehaviour
{
    [Header("Boss")]
    public string bossID = "Boss1";
    public GameObject bossVisual;

    [Header("UI")]
    public GameObject interactionIndicator;

    [Header("Prologue")]
    public string[] requiredConditions;
    public bool requireAllConditions = true;
    public ConditionConfigSO config;
    public ConditionEventSO conditionEvent;
    public GameObject hintPanel;
    public Text hintText;

    [Header("Scene Load")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO battleScene;

    [Header("Prelude")]
    public PrologueSystem prologueSystem;

    public static string currentBossID;

    private bool playerInRange = false;
    private SceneLoader sceneLoader;
    private bool isConditionMet = false;
    private Collider bossCollider;

    private void Awake()
    {
        if (interactionIndicator != null)
            interactionIndicator.SetActive(false);
        bossCollider = GetComponent<Collider>();
        if (bossCollider != null)
            bossCollider.enabled = true;
    }

    private void OnEnable()
    {
        if (conditionEvent != null)
            conditionEvent.OnConditionRaised += OnConditionUpdated;
    }

    private void OnDisable()
    {
        if (conditionEvent != null)
            conditionEvent.OnConditionRaised -= OnConditionUpdated;
    }

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();

        if (ConditionManager.Instance != null &&
            ConditionManager.Instance.CheckConditions(new string[] { "BossDefeated_" + bossID }, true))
        {
            Destroy(gameObject);
            return;
        }

        RefreshConditionState();
    }

    private void OnConditionUpdated(string conditionID)
    {
        RefreshConditionState();
    }

    private void RefreshConditionState()
    {
        bool met = ConditionManager.Instance != null &&
                   ConditionManager.Instance.CheckConditions(requiredConditions, requireAllConditions);
        if (met != isConditionMet)
        {
            isConditionMet = met;
            UpdateVisualAndCollider();
        }
    }

    private void UpdateVisualAndCollider()
    {
        if (bossVisual != null)
            bossVisual.SetActive(isConditionMet);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionIndicator != null)
                interactionIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionIndicator != null)
                interactionIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.I))
        {
            if (!isConditionMet)
            {
                ShowHintPanel();
                return;
            }

            currentBossID = bossID;

            playerInRange = false;
            if (interactionIndicator != null)
                interactionIndicator.SetActive(false);

            if (prologueSystem != null)
            {
                prologueSystem.onPrologueEnd.RemoveListener(StartBattle);
                prologueSystem.onPrologueEnd.AddListener(StartBattle);
                prologueSystem.PlayPrologue();
            }
            else
            {
                StartBattle();
            }
        }
    }

    private void ShowHintPanel()
    {
        if (hintPanel == null || hintText == null)
        {
            Debug.LogWarning("BossEncounter: Lack UI");
            return;
        }

        System.Collections.Generic.List<string> missing = new System.Collections.Generic.List<string>();
        foreach (var id in requiredConditions)
        {
            if (ConditionManager.Instance != null && !ConditionManager.Instance.CheckConditions(new string[] { id }, true))
            {
                string displayName = config != null ? config.GetDisplayName(id) : id;
                missing.Add(displayName);
            }
        }

        string message = "这团“东西”发出着微弱的“呜呜”声，但你无法厘清这是什么。\n\n";
        //foreach (var item in missing)
        //    message += "• " + item + "\n";

        hintText.text = message;
        hintPanel.SetActive(true);

        CancelInvoke();
        Invoke(nameof(CloseHintPanel), 3.0f);
    }

    public void CloseHintPanel()
    {
        if (hintPanel != null)
            hintPanel.SetActive(false);
    }

    private void StartBattle()
    {
        if (sceneLoader != null)
        {
            sceneLoader.SaveCurrentAsPrevious(sceneLoader.CurrentLoadedScene, sceneLoader.playerTrans.position);
        }

        if (loadEventSO == null || battleScene == null)
        {
            Debug.LogError("BossEncounter: Lack loadEventSO & battleScene");
            return;
        }

        loadEventSO.RaiseLoadRequestEvent(battleScene, Vector3.zero, true);
    }

    private void OnDrawGizmosSelected()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
    }
}