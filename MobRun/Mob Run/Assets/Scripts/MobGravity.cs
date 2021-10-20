using System.Collections.Generic;
using UnityEngine;

public class MobGravity : MonoBehaviour
{
    #region Public Variables
    public static List<CloneBehaviour> clonesToBeAttractedList;
    #endregion

    #region Private Variables
    [SerializeField]
    private Rigidbody rb;
    private float MinDistanceMultiplier;
    #endregion

    void Start()
    {
        MinDistanceMultiplier = 1;
        rb.mass = 800;
    }

    void FixedUpdate()
    {
        if (clonesToBeAttractedList != null)
        {
            foreach (CloneBehaviour cloneBehaviour in clonesToBeAttractedList)
            {
                Attract(cloneBehaviour);
            }
        }
    }
    void Attract(CloneBehaviour objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;

        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance < 0.7f * MinDistanceMultiplier)
            return;

        float forceMagnitude = (rb.mass * rbToAttract.mass) / (distance * distance);
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }

    public void SetMinDistanceMultiplier(int playerCount)
    {
        if (playerCount > 0 && playerCount < 5)
        {
            MinDistanceMultiplier = 1.2f;
        }
        else if (playerCount >= 5 && playerCount < 15)
        {
            MinDistanceMultiplier = 1.5f;
        }
        else if (playerCount >= 15 && playerCount < 30)
        {
            MinDistanceMultiplier = 1.875f;
        }
        else if (playerCount >= 30 && playerCount < 60)
        {
            MinDistanceMultiplier = 2.625f;
        }
        else if (playerCount >= 60 && playerCount < 200)
        {
            MinDistanceMultiplier = 3.25f;
        }
        else if (playerCount >= 200)
        {
            MinDistanceMultiplier = 5.875f;
        }
    }
}