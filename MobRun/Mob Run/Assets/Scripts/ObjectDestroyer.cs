using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    #region Private Variables
    private MobBehaviour mobBehaviour;
    #endregion

    void Start()
    {
        mobBehaviour = FindObjectOfType<MobBehaviour>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();

            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            rb.drag = 10;
            mobBehaviour.KillPlayer(other.gameObject);
        }
    }
}
