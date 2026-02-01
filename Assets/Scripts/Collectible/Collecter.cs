using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Collecter : MonoBehaviour
{
    private int Score = 0; 

    [SerializeField] TextMeshProUGUI scoreText;

    public void AddScore(int amount)
    {
        Score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "[}}-<" + Score + ">-{]";
    }
}
