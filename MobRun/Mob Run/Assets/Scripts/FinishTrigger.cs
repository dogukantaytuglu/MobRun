using Cinemachine;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    #region Private Variables
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private MobBehaviour mobBehaviour;
    [SerializeField] private CinemachineVirtualCamera camera;
    private bool cameraCanMove;
    #endregion

    void Start()
    {
        mobBehaviour = FindObjectOfType<MobBehaviour>();
        camera = FindObjectOfType<CinemachineVirtualCamera>();
        cameraCanMove = false;
    }

    void Update()
    {
        if (cameraCanMove)
        {
            MoveCamera();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mobBehaviour.StartCoroutine("CreatePyramid");
            boxCollider.enabled = false;

            cameraCanMove = true;
        }
    }

    void MoveCamera()
    {
        var transposer = camera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer.m_FollowOffset.x < 10)
        {
            transposer.m_FollowOffset.x += Time.deltaTime * 6;
            transposer.m_FollowOffset.y += Time.deltaTime * 3;
            camera.m_Lens.FieldOfView -= Time.deltaTime * 3;
        }

    }
}
