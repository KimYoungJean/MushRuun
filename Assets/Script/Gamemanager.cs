using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public static bool isClear = false;
    public static bool portalSpawned = false;

    public Transform startPosition;
    public GameObject PlayerPrefab;



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

        isClear = false;
    }
    private void Start()
    {
        Instantiate(PlayerPrefab, startPosition.position, Quaternion.identity);
        
        AudioManager.instance.ChangeVolume(0.1f);
        AudioManager.instance.SkipSound();
        AudioManager.instance.PlaySound(AudioManager.instance.audioClips[4]);
    }

    public void Restart()
    {
        Destroy(GameObject.Find("Player(Clone)"));

        UIManager.instance.TimeReset();
        Instantiate(PlayerPrefab, startPosition.position, Quaternion.identity);
        UIManager.instance.GameStart();

    }
    /*
        private void Update()
        {
            if (UIManager.time >= 100)
            {
                isClear = true;
            }
        }*/
}
