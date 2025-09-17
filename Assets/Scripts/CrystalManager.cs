using TMPro;
using UnityEngine;

public class CrystalManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI crystalsCollectedText;
    private int crystalsCollected = 0;

    private void Start()
    {
        crystalsCollectedText.text = crystalsCollected.ToString();
    }


    public void CrystalCollected()
    {
        crystalsCollected++;
        crystalsCollectedText.text = crystalsCollected.ToString();
    }
}
