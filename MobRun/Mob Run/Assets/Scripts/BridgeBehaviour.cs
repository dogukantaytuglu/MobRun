using UnityEngine;

public class BridgeBehaviour : MonoBehaviour
{
    #region Public Variables
    public bool canOpen = false;
    #endregion
    #region Private Variables
    [SerializeField]
    private GameObject parentPartOne, partOne, parentPartTwo, partTwo;
    [SerializeField]
    private BoxCollider boxCollider;
    #endregion

    void Update()
    {
        if (canOpen)
        {
            OpenTheBridge();
        }
    }
    
    private void OpenTheBridge()
    {
        boxCollider.enabled = false;

        if (partOne.transform.rotation.x > 0)
        {
            partOne.transform.RotateAround(parentPartOne.transform.position, new Vector3(-1f, 0, 0), 60 * Time.deltaTime);
            partTwo.transform.RotateAround(parentPartTwo.transform.position, new Vector3(1f, 0, 0), 60 * Time.deltaTime);
        }
        else
        {
            canOpen = false;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            MobGravity.clonesToBeAttractedList.Remove(other.gameObject.GetComponent<CloneBehaviour>());

            MobBehaviour.desiredPlayerCount--;
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            rb.drag = 0;
            rb.useGravity = true;
        }
    }
}
