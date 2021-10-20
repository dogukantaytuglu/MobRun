using UnityEngine;

public class EnemyMobBehaviour : MonoBehaviour
{
    #region Serialized Private Variables
    [SerializeField]
    private PoolController poolController;
    [SerializeField]
    private MobGravity mobGravity;
    [SerializeField]
    private MobCountUI mobCountUi;
    #endregion

    #region Private Variables
    [SerializeField]
    [Range(1, 300)]
    private int desiredEnemyCount = 1;
    [Range(1, 300)]
    private int activeEnemyCount;

    private Vector3 playPosition;
    private bool canMove;
    #endregion

    void Start()
    {
        canMove = false;
        activeEnemyCount = CountActiveChild();
        mobCountUi.SetMobUIText(activeEnemyCount);
        SpawnEnemy();
    }

    void Update()
    {
        if (canMove)
        {
            MoveToPlayer();
        }

        if (activeEnemyCount <= 0)
        {
            MobBehaviour.warModeOn = false;
            Destroy(gameObject);
        }
    }

    void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playPosition, Time.deltaTime);
    }

    public void KillEnemy(GameObject gameObject)
    {
        if (desiredEnemyCount < activeEnemyCount)
        {
            gameObject.transform.position = transform.position;
            gameObject.SetActive(false);

            AdjustMobToCloneChange();
        }
    }

    void SpawnEnemy()
    {
        for (int i = activeEnemyCount; i < desiredEnemyCount; i++)
        {
            //Select a random range around mobs zero point
            float randomXPosition = Random.Range(-0.2f, 0.2f);
            float randomZPosition = Random.Range(-0.2f, 0.2f);
            Vector3 spawnPosition =
                new Vector3(transform.position.x + randomXPosition, transform.position.y, transform.position.z + randomZPosition);

            //Spawn clone from pool on selected random point
            poolController.pooledGameObjects[i].transform.position = spawnPosition;
            poolController.pooledGameObjects[i].SetActive(true);

            AdjustMobToCloneChange();
        }
    }

    void AdjustMobToCloneChange()
    {
        //Count spawned Players
        activeEnemyCount = CountActiveChild();

        //Set gravitational distance multiplier
        mobGravity.SetMinDistanceMultiplier(activeEnemyCount);

        //Set UI Text
        mobCountUi.SetMobUIText(activeEnemyCount);
    }

    int CountActiveChild()
    {
        int count = 0;
        foreach (Transform childTransform in transform)
        {
            if (childTransform.gameObject.activeSelf && childTransform.gameObject.tag == "Enemy")
            {
                count++;
            }
        }

        return count;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!MobBehaviour.warModeOn && other.gameObject.tag == "Player")
        {
            playPosition = other.gameObject.transform.position;
            canMove = true;
            desiredEnemyCount -= MobBehaviour.activePlayerCount;
            mobGravity.enabled = false;
            MobBehaviour.warModeOn = true;
            MobBehaviour.desiredPlayerCount -= activeEnemyCount;
        }
    }


}
