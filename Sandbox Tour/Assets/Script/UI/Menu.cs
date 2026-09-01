using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject newGameButton;
    public GameObject continueGameButton;

    private void OnEnable()
    {
        if (newGameButton != null)
            EventSystem.current.SetSelectedGameObject(newGameButton);

        if (continueGameButton != null)
        {
            bool hasSave = PlayerPrefs.HasKey("Save_PosX");
            continueGameButton.SetActive(hasSave);
        }
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        TimelineController timelineCtrl = FindObjectOfType<TimelineController>();
        if (timelineCtrl != null)
        {
            timelineCtrl.PlayCutscene();
        }
        else
        {
            Debug.LogWarning("Lack TimelineController");
            SceneLoader loader = FindObjectOfType<SceneLoader>();
            if (loader != null) loader.NewGame();
        }
    }

    public void ContinueGame()
    {
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader == null)
        {
            Debug.LogError("Lack SceneLoader");
            return;
        }

        loader.LoadFromSave();
    }

    public void ExitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}