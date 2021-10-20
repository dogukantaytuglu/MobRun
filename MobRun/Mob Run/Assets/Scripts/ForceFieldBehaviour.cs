using TMPro;
using UnityEngine;

public class ForceFieldBehaviour : MonoBehaviour
{
    #region Private Variables

    private int test;

    [SerializeField]
    private BoxCollider neighbourBoxCollider;

    [SerializeField]
    private TextMeshPro textMesh;

    private enum OperationType
    {
        Sum,
        Multiplication
    }

    [SerializeField]
    private OperationType operationType;

    [SerializeField]
    private int operationNumber;

    [SerializeField]
    private BoxCollider boxCollider;

    private MobBehaviour mobBehaviour;
    #endregion


    void Start()
    {
        test = 0;
        textMesh.text = (operationType == OperationType.Sum ? "+" : "x") + operationNumber;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boxCollider.enabled = false;
            neighbourBoxCollider.enabled = false;
            mobBehaviour = other.gameObject.GetComponentInParent<MobBehaviour>();
            CalculateNewMobCount(operationType,operationNumber);
        }
    }

    void CalculateNewMobCount(OperationType operationType, int operationNumber)
    {
        int newMobCount = 0;
        if (operationType == OperationType.Sum)
        {
            newMobCount = MobBehaviour.activePlayerCount + operationNumber;
        }
        else
        {
            newMobCount = MobBehaviour.activePlayerCount * operationNumber;
        }

        if (newMobCount > 200)
        {
            Debug.Log("Mob can not be bigger than 200");
            newMobCount = 200;
        }

        if (newMobCount < 1)
        {
            Debug.Log("Mob can not be smaller than 1");
            newMobCount = 1;
        }

        MobBehaviour.desiredPlayerCount = newMobCount;
        mobBehaviour.SpawnPlayer();
    }
}
