using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    #region Public Variables
    [HideInInspector] public List<GameObject> pooledGameObjects;
    public int poolCount;
    #endregion

    #region Private Variables
    [SerializeField] 
    private GameObject objectToPool, parentObject, firstClone;
    #endregion


    void Awake()
    {
        poolCount = 199;
        pooledGameObjects = new List<GameObject>();
        pooledGameObjects.Add(firstClone);

        for (int i = 0; i < poolCount; i++)
        {
            GameObject obj = (GameObject) Instantiate(objectToPool);
            obj.SetActive(false);
            obj.transform.parent = parentObject.transform;
            obj.name = obj.name + i;
            pooledGameObjects.Add(obj);
        }
    }
}
