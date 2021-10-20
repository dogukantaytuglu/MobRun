using System.Collections.Generic;
using UnityEngine;

public class CloneBehaviour : MonoBehaviour
{
    #region Public Variables
    public Rigidbody rb;
    public bool isTopClone;
    #endregion

    #region Private Variables
    private MobBehaviour mobBehaviour;
    private EnemyMobBehaviour enemyMobBehaviour;
    private GameManager gameManager;
    #endregion

    void OnEnable()
    {
        if (gameObject.tag == "Player")
        {
            mobBehaviour = GetComponentInParent<MobBehaviour>();
        }
        else
        {
            enemyMobBehaviour = GetComponentInParent<EnemyMobBehaviour>();
        }

        if (MobGravity.clonesToBeAttractedList == null)
        {
            MobGravity.clonesToBeAttractedList = new List<CloneBehaviour>();
        }

        gameManager = GetComponentInParent<GameManager>();
        MobGravity.clonesToBeAttractedList.Add(this);
        isTopClone = false;
    }

    void OnDisable()
    {
        MobGravity.clonesToBeAttractedList.Remove(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag == "Player")
        {
            if (other.gameObject.tag == "Enemy")
            {
                mobBehaviour.KillPlayer(gameObject);
            }

            if (other.gameObject.tag == "Stairway")
            {
                gameObject.transform.parent = gameManager.transform;
                if (isTopClone)
                {
                    gameManager.EnableEndGameScreen();
                    isTopClone = false;
                    GameManager.gameIsActive = false;
                }
            }
        }

        if (this.gameObject.tag == "Enemy" && other.gameObject.tag == "Player")
        {
            enemyMobBehaviour.KillEnemy(gameObject);
        }
    }
}
