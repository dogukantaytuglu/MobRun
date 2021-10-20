using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class MobBehaviour : MonoBehaviour
{
    #region Public Variables
    public static bool warModeOn;

    //[Range(1, 300)]
    [HideInInspector]
    public static int desiredPlayerCount = 1, activePlayerCount;

    public float movementSpeed;

    [HideInInspector]
    #endregion

    #region Private Variables
    [SerializeField] private PoolController poolController;
    [SerializeField] private MobGravity mobGravity;
    [SerializeField] private MobCountUI mobCountUi;
    [SerializeField] private GameObject pyramidParent, canvas;
    [SerializeField] private CinemachineVirtualCamera camera;
    private GameManager gameManager;
    #endregion

    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        activePlayerCount = CountActiveChild();
        mobCountUi.SetMobUIText(activePlayerCount);
        SpawnPlayer();
    }

    void Update()
    {
        if (!warModeOn && GameManager.gameIsActive)
        {
            TouchControl.canMoveLeft = true;
            TouchControl.canMoveRight = true;

            RunPlayerMob();
        }
        if (warModeOn)
        {
            //Stop touch controls after finish
            TouchControl.canMoveLeft = false;
            TouchControl.canMoveRight = false;

            MoveTowardsCenter();
        }
    }

    void RunPlayerMob()
    {
        float zSpeed = movementSpeed * Time.deltaTime;
        transform.Translate(0, 0, zSpeed);
    }

    void MoveTowardsCenter()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - transform.position.x, transform.position.y,
            transform.position.z + 12), Time.deltaTime);
    }

    public void KillPlayer(GameObject gameObject)
    {
        if (desiredPlayerCount < activePlayerCount)
        {
            gameObject.transform.position = transform.position;
            gameObject.SetActive(false);

            //Count spawned Players
            activePlayerCount--;

            if (activePlayerCount <= 0)
            {
                gameManager.EnableYouLostScreen();
                GameManager.gameIsActive = false;
            }

            AdjustMobToCloneChange();
        }

    }

    public void SpawnPlayer()
    {
        for (int i = 0; activePlayerCount < desiredPlayerCount; i++)
        {
            //Check if index is in bounds of pool array
            if (i >= poolController.poolCount-1)
            {
                i = 0;
            }
            //If selected object is already active continue
            if (poolController.pooledGameObjects[i].activeSelf)
            {
                continue;
            }

            //Select a random range around mobs zero point
            float randomXPosition = Random.Range(-0.2f, 0.2f);
            float randomZPosition = Random.Range(-0.2f, 0.2f);
            Vector3 spawnPosition =
                new Vector3(transform.position.x + randomXPosition, transform.position.y, transform.position.z + randomZPosition);

            //Spawn clone from pool on selected random point
            poolController.pooledGameObjects[i].transform.position = spawnPosition;
            poolController.pooledGameObjects[i].SetActive(true);

            activePlayerCount++;

            AdjustMobToCloneChange();
        }
    }

    public IEnumerator CreatePyramid()
    {
        //Stop touch controls after finish
        TouchControl.canMoveLeft = false;
        TouchControl.canMoveRight = false;

        canvas.SetActive(false);

        //Calculate every row of the pyramid and save it to a list
        int lastRowValue = Mathf.FloorToInt(Mathf.Sqrt(activePlayerCount));
        int remainder = activePlayerCount % lastRowValue;
        int tempPlayerCount = activePlayerCount;

        List<int> pyramidList = new List<int>();

        for (int j = 1; j <= lastRowValue; j++)
        {
            int rowCounter = 2;
            if (j == remainder)
            {
                rowCounter = 3;
            }
            else if (remainder == 0 && j == lastRowValue)
            {
                rowCounter = 3;
            }
            for (int i = 1; i <= rowCounter; i++)
            {
                if (tempPlayerCount > 0)
                {
                    tempPlayerCount -= j;
                    pyramidList.Add(j);
                }
                else
                {
                    break;
                }
            }
        }


        //Save the total row count so that the pyramid can keep track how many more rows left
        int totalRowCount = pyramidList.Count;

        //Calculate the final score
        float scoreMultiplier = 1 + ((totalRowCount - 2) * 0.2f);
        ScoreScript.CalculateScore(activePlayerCount, scoreMultiplier);

        //Iterate through list to get current row value
        foreach (var i in pyramidList)
        {
            //Iterate through pooled objects for every row
            for (int j = 0; j < i; j++)
            {

                //Calculate the start point of every row
                float lineStartPoint = (i - 1) * -0.5f / 2;

                //Select the game object from the pool
                int a = 0;
                GameObject selectedObject = poolController.pooledGameObjects[a];
                while (!selectedObject.activeSelf)
                {
                    if (a < poolController.pooledGameObjects.Count)
                    {
                        a += 1;
                        selectedObject = poolController.pooledGameObjects[a];
                    }
                    else
                    {
                        a = 0;
                    }
                }

                //Look at the player on the top and mark it as Top Clone
                if (totalRowCount == pyramidList.Count)
                {
                    camera.LookAt = selectedObject.transform;
                    selectedObject.GetComponent<CloneBehaviour>().isTopClone = true;
                }

                //Remove it from pool and gravity attraction list
                poolController.pooledGameObjects.Remove(selectedObject);
                MobGravity.clonesToBeAttractedList.Remove(selectedObject.GetComponent<CloneBehaviour>());

                //Make it kinematic
                var rb = selectedObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;


                //Set new game object parent and move object to the needed position
                selectedObject.transform.position = new Vector3(lineStartPoint + (j * 0.50f), 0.50f, pyramidParent.transform.position.z);
                selectedObject.transform.parent = pyramidParent.transform;

                yield return new WaitForSeconds(0.5f / activePlayerCount);
                rb.isKinematic = false;
            }

            //If there is no more row to be built stop creating vertical space
            totalRowCount--;
            if (totalRowCount > 0)
            {
                pyramidParent.transform.position = new Vector3(pyramidParent.transform.position.x, pyramidParent.transform.position.y + 1, pyramidParent.transform.position.z);
            }
        }
        StopCoroutine(CreatePyramid());
    }

    public void AdjustMobToCloneChange()
    {
        //Set gravitational distance multiplier
        mobGravity.SetMinDistanceMultiplier(activePlayerCount);

        //Set UI Text
        mobCountUi.SetMobUIText(activePlayerCount);
    }

    int CountActiveChild()
    {
        int count = 0;
        foreach (Transform childTransform in transform)
        {
            if (childTransform.gameObject.activeSelf && childTransform.gameObject.tag == "Player")
            {
                count++;
            }
        }

        return count;
    }


}
