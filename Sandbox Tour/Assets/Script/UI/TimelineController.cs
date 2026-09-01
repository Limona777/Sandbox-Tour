using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    private PlayableDirector director;

    public GameObject introCanvas;

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += OnTimelineStopped;
    }

    public void PlayCutscene()
    {
        if (director == null) return;

        if (introCanvas != null)
        {
            introCanvas.SetActive(true);
        }
        else
        {
            GameObject found = GameObject.Find("IntroCanvas");
            if (found != null) found.SetActive(true);
            else Debug.LogWarning("TimelineController: Lack IntroCanvas");
        }

        Time.timeScale = 0f;

        PlayerMove player = FindObjectOfType<PlayerMove>();
        if (player != null) player.enabled = false;

        director.Play();
    }

    private void OnTimelineStopped(PlayableDirector dir)
    {
        if (introCanvas != null)
        {
            introCanvas.SetActive(false);
        }
        else
        {
            GameObject found = GameObject.Find("IntroCanvas");
            if (found != null) found.SetActive(false);
            else Debug.LogWarning("TimelineController: Lack IntroCanvas");
        }

        Time.timeScale = 1f;

        PlayerMove player = FindObjectOfType<PlayerMove>();
        if (player != null) player.enabled = true;

        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader != null)
        {
            loader.NewGame();
        }
    }

    private void OnDestroy()
    {
        if (director != null)
            director.stopped -= OnTimelineStopped;
    }
}