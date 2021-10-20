using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public static float resultScore = 0;
    
    public static void CalculateScore(int activePlayerCount, float scoreMultiplier)
    {
        resultScore = activePlayerCount * scoreMultiplier;
    }
}
