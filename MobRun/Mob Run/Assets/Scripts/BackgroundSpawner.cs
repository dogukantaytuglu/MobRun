using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField]
    private BoxCollider boxCollider;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerMob"))
        {
            boxCollider.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 200);
            boxCollider.enabled = true;

        }
    }
}
