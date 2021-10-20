using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    //Stop movement in certain direction if a player touches left or right wall
    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Player" && gameObject.name == "Left Wall")
        {
            TouchControl.canMoveLeft = false;
        }
        if (other.gameObject.tag == "Player" && gameObject.name == "Right Wall")
        {
            TouchControl.canMoveRight = false;
        }
    }


    //Restart stopped movement in certain direction if all players stops touching left or right wall
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player" && gameObject.name == "Left Wall")
        {
            TouchControl.canMoveLeft = true;
        }
        if (other.gameObject.tag == "Player" && gameObject.name == "Right Wall")
        {
            TouchControl.canMoveRight = true;
        }
    }
}
