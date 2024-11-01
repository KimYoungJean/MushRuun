using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public static bool userLogin;

    public TextMeshProUGUI timeText;

    public GameObject GameoverPanel;

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
        timeText.text = "����: " + time.ToString("F2");
    }
    public void GameStart()
    {
        Time.timeScale = 1;
        GameoverPanel.SetActive(false);

        //1�Ÿ����� Enemy�� ����
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Debug.Log(Vector3.Distance(enemy.transform.position, GameObject.Find("Player(Clone)").transform.position));
            if (Vector3.Distance(enemy.transform.position, GameObject.Find("Player(Clone)").transform.position) < 5)
            {
                Destroy(enemy);
                Debug.Log("Enemy Destroyed");
            }
        }

    }
    public void TimeReset()
    {
        time = 0;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameoverPanel.SetActive(true);
    }
}
