using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    public GameObject tutorialPanel;

    private void Awake()
    {
        if (transform.parent == null)
            DontDestroyOnLoad(gameObject);
        else
            DontDestroyOnLoad(transform.root.gameObject);

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleTutorial();
        }
    }

    public void ToggleTutorial()
    {
        if (tutorialPanel == null)
        {
            Debug.LogWarning("TutorialUIController: Lack TutorialPanel");
            return;
        }

        bool isActive = tutorialPanel.activeSelf;
        tutorialPanel.SetActive(!isActive);

        Time.timeScale = tutorialPanel.activeSelf ? 0f : 1f;
    }
}