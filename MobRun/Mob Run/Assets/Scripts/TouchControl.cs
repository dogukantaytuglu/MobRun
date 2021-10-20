using UnityEngine;

public class TouchControl : MonoBehaviour
{
    #region Public Variables
    public static bool canMoveLeft, canMoveRight;
    #endregion

    #region Private Variables
    private Touch touch;
    #endregion

    void Start()
    {
        //Enable movements in both directions
        canMoveLeft = true;
        canMoveRight = true;
    }

    void Update()
    {
        SlideToMove();
    }

    void SlideToMove()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                var touchDirection = touch.deltaPosition.x < 0 ? "left" : "right";

                if (touchDirection == "left")
                {
                    if (canMoveLeft)
                    {
                        transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * Time.deltaTime * 2 < -5f ? -5f : transform.position.x + touch.deltaPosition.x * Time.deltaTime,
                                transform.position.y, transform.position.z);
                    }
                }
                else if (touchDirection == "right")
                {
                    if (canMoveRight)
                    {
                        transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * Time.deltaTime * 2 > 5f ? 5f : transform.position.x + touch.deltaPosition.x * Time.deltaTime,
                            transform.position.y, transform.position.z);
                    }
                }
            }
        }
    }

}
