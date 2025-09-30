using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    
    [SerializeField] Color goodColor = Color.green;
    [SerializeField] Color badColor = Color.red;
    [SerializeField] Color normalColor = Color.yellow;

    private bool isCanLogFPS = false;
    
    private int frameCount = 0;
    private float elapsedTime = 0f;
    
    void Start()
    {
        isCanLogFPS = true;     
        fpsText.gameObject.SetActive(isCanLogFPS);
    }
    
    void Update()
    {
        if (!isCanLogFPS) return;
        
        LogAvgFPS();
    }
    
    private void LogAvgFPS()
    {
        frameCount++;
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1f)
        {
            // calculate average fps
            float averageFPS = frameCount / elapsedTime;

            fpsText.text = new StringBuilder().Append("FPS: ").AppendFormat(CultureInfo.InvariantCulture, "{0:F2}", averageFPS).ToString();

            frameCount = 0;
            elapsedTime = 0f;

            fpsText.color = averageFPS switch
            {
                // change color based on fps
                >= 50f => goodColor,
                >= 30f => normalColor,
                _ => badColor
            };
        }
    }
}
