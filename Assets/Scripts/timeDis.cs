
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

public class timeDis : UdonSharpBehaviour
{
    public float timer;
    public TextMeshProUGUI timerText;
    void Start()
    {
        timer = 0.0f;
    }
    public void Update()
    {
        timer += Time.deltaTime;
        UpdateTimerDisplay();
    }
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = $"your Time in here: {timer:F2} s"; // Format to 2 decimal places
        }
        else
        {
            Debug.LogWarning("Timer Text is not assigned!");
        }
    }
}
