using UnityEngine;

public class BridgeButton : MonoBehaviour
{
    [SerializeField] private BridgeBehaviour bridgeBehaviour;
    [SerializeField] private BoxCollider boxCollider;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boxCollider.enabled = false;
            bridgeBehaviour.canOpen = true;
        }
    }
}
