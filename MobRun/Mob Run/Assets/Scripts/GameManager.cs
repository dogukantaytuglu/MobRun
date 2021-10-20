using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Public Variables
    public static bool gameIsActive;
    #endregion

    #region Serialized Private Variables
    [SerializeField] private GameObject[] levels;
    [SerializeField] private GameObject youLostScreen, endGameScreen, startScreen, mountainOne, mountainTwo, playButtonObject;
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private Text countdownText, levelText;
    #endregion

    #region Private Variables
    private PoolController poolController;
    private MobBehaviour mobBehaviour;
    private Button playButton;
    private GameObject mobGameObject, canvas;
    private int currentLevelIndex;
    #endregion

    void Awake()
    {
        currentLevelIndex = 0;
        levelText.text = "Level " + (currentLevelIndex + 1).ToString();
        LoadLevel(currentLevelIndex);

        playButton = playButtonObject.GetComponent<Button>();

        mobBehaviour = GetComponentInChildren<MobBehaviour>();
        mobGameObject = mobBehaviour.gameObject;
        foreach (Transform childTransform in mobGameObject.transform)
        {
            if (childTransform.gameObject.name == "Mob Canvas")
            {
                canvas = childTransform.gameObject;
            }
        }

        gameIsActive = false;
    }

    public void EnableYouLostScreen()
    {
        mobGameObject.SetActive(false);
        youLostScreen.SetActive(true);
    }

    public void EnableEndGameScreen()
    {
        endGameScreen.GetComponentInChildren<TextMeshProUGUI>().text = ScoreScript.resultScore.ToString();
        endGameScreen.SetActive(true);
    }

    public void LoadLevel(int levelIndex)
    {
        levelText.text = "Level " + (currentLevelIndex + 1).ToString();
        foreach (Transform childTransform in transform)
        {
            if (childTransform.gameObject.tag == "Level")
            {
                Destroy(childTransform.gameObject);
            }
        }
        GameObject obj = (GameObject)Instantiate(levels[levelIndex]);
        obj.transform.parent = gameObject.transform;
    }

    public void PlayButtonFunction()
    {
        playButton.interactable = false;
        countdownText.enabled = true;
        StartCoroutine(DelayGameStart());
    }

    IEnumerator DelayGameStart()
    {
        for (int i = 0; i < 2; i++)
        {
            countdownText.text = (3 - (i + 1)).ToString();
            yield return new WaitForSeconds(1);
        }
        startScreen.SetActive(false);
        playButton.interactable = true;
        countdownText.text = "3";
        gameIsActive = true;
        countdownText.enabled = false;
        StopCoroutine(DelayGameStart());
    }

    public void RestartLevel()
    {
        mountainOne.transform.position = new Vector3(0, 0, 0);
        mountainTwo.transform.position = new Vector3(0, 0, 100);

        LoadLevel(currentLevelIndex);
        mobGameObject.transform.position = new Vector3(0, 0.5f, 15);
        mobGameObject.gameObject.SetActive(true);
        MobBehaviour.desiredPlayerCount = 1;
        MobBehaviour.warModeOn = false;
        mobBehaviour.SpawnPlayer();
        youLostScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    public void LoadNextLevel()
    {
        MobBehaviour.desiredPlayerCount = 1;
        MobBehaviour.activePlayerCount = 0;

        mountainOne.transform.position = new Vector3(0, 0, 0);
        mountainTwo.transform.position = new Vector3(0, 0, 100);

        mobGameObject.transform.position = new Vector3(0, 0.5f, 15);
        RestoreCameraToInitialPosition();

        var clones = GetComponentsInChildren<CloneBehaviour>();
        poolController = GetComponentInChildren<PoolController>();
        foreach (var clone in clones)
        {
            clone.gameObject.transform.parent = mobGameObject.transform;
            clone.gameObject.transform.position = mobGameObject.transform.position;
            clone.gameObject.SetActive(false);
            poolController.pooledGameObjects.Add(clone.gameObject);
        }

        canvas.gameObject.SetActive(true);

        mobBehaviour.SpawnPlayer();
        if (currentLevelIndex + 1 > levels.Length - 1)
        {
            currentLevelIndex = 0;
        }
        else
        {
            currentLevelIndex += 1;
        }
        LoadLevel(currentLevelIndex);
        endGameScreen.SetActive(false);
        startScreen.SetActive(true);

    }

    void RestoreCameraToInitialPosition()
    {
        var transposer = camera.GetCinemachineComponent<CinemachineTransposer>();

        transposer.m_FollowOffset.x = 0;
        transposer.m_FollowOffset.y = 13;
        camera.LookAt = mobGameObject.transform;
        camera.m_Lens.FieldOfView = 60;
    }

}

