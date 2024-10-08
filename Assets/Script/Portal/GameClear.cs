using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Clear");
            Time.timeScale = 0;

            // ���� Ŭ���� UI�� Ȱ��ȭ
        }
    }
}
