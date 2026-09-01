using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;

    [Header("Event Listeners ")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;

    [Header("Scene")]
    public GameSceneSO menuScene;
    public GameSceneSO firstLoadScene;

    private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;

    public float fadeDuration;

    public GameSceneSO[] allGameScenes;

    private Dictionary<string, GameSceneSO> sceneNameMap;

    public ReturnToPrevSceneEventSO returnEvent;

    private GameSceneSO previousScene;
    private Vector3 previousPosition;

    public GameSceneSO CurrentLoadedScene => currentLoadedScene;

    private void Awake()
    {
        sceneNameMap = new Dictionary<string, GameSceneSO>();
        if (allGameScenes != null)
        {
            foreach (var scene in allGameScenes)
            {
                if (scene != null && !sceneNameMap.ContainsKey(scene.name))
                    sceneNameMap.Add(scene.name, scene);
            }
        }
    }

    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;

        if (returnEvent != null)
            returnEvent.OnReturnRequested += OnReturnToPreviousScene;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;

        if (returnEvent != null)
            returnEvent.OnReturnRequested -= OnReturnToPreviousScene;
    }

    public void SaveCurrentAsPrevious(GameSceneSO scene, Vector3 pos)
    {
        previousScene = scene;
        previousPosition = pos;
    }

    private void OnReturnToPreviousScene()
    {
        if (previousScene == null)
        {
            Debug.LogWarning("Cannot return the previous scene that has not been saved.");
            return;
        }

        loadEventSO.RaiseLoadRequestEvent(previousScene, previousPosition, true);
    }

    public void NewGame()
    {
        sceneToLoad = firstLoadScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if (currentLoadedScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {

        }
        yield return new WaitForSeconds(fadeDuration);
        yield return currentLoadedScene.sceneReference.UnLoadScene();
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadedScene = sceneToLoad;
        playerTrans.position = positionToGo;

        if (fadeScreen)
        {

        }
    }

    public void LoadFromSave()
    {
        if (!PlayerPrefs.HasKey("Save_PosX"))
        {
            Debug.LogWarning("No save files, unable to continue the game.");
            return;
        }

        Vector3 savedPos = new Vector3(
            PlayerPrefs.GetFloat("Save_PosX"),
            PlayerPrefs.GetFloat("Save_PosY"),
            PlayerPrefs.GetFloat("Save_PosZ")
        );

        string sceneName = PlayerPrefs.GetString("Save_SceneName", "");
        string condStr = PlayerPrefs.GetString("Save_Conditions", "");

        if (!string.IsNullOrEmpty(condStr))
        {
            string[] conds = condStr.Split(',');
            foreach (var id in conds)
            {
                if (!string.IsNullOrEmpty(id))
                    ConditionManager.Instance?.SetCondition(id);
            }
        }

        GameSceneSO targetScene = null;
        if (!string.IsNullOrEmpty(sceneName) && sceneNameMap.ContainsKey(sceneName))
        {
            targetScene = sceneNameMap[sceneName];
        }
        else
        {
            targetScene = firstLoadScene;
        }

        loadEventSO.RaiseLoadRequestEvent(targetScene, savedPos, true);
    }
}