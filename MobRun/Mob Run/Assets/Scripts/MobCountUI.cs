using TMPro;
using UnityEngine;

public class MobCountUI : MonoBehaviour
{
    #region Private Variables
    [SerializeField]
    private TextMeshProUGUI textMesh;
    #endregion

    public void SetMobUIText(int count)
    {
        textMesh.text = count.ToString();
    }
}
