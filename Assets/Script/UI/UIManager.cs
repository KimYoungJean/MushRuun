using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI timeText;
    public static float time;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        time = 0;
    }

    public void Update()
    {
        time += Time.deltaTime;
        timeText.text = "Á¡¼ö: " + time.ToString("F2");
    }

    public void TimeReset()
    {
        time = 0;
    }
}
