using System.Collections.Generic;
using UnityEngine;

public class InfiniteLevel : MonoBehaviour
{
    public float speed = 8f;  // 레벨이 이동하는 속도
    public float resetPositionX = -20f;  // 레벨이 재설정될 X 위치 (화면 왼쪽 밖)
    public float startPositionX = 20f;   // 레벨이 처음 나타나는 X 위치 (화면 오른쪽 밖)

    public List<GameObject> SpawnPoints;
    public List<GameObject> Enemys;
    public GameObject Portal;

    public GameObject levelPrefab; // 재사용할 레벨의 프리팹

    private Vector3 initialPosition; // 원래 위치를 저장하는 변수

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // 레벨을 왼쪽으로 이동
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // 레벨이 화면 왼쪽으로 사라지면 위치를 재설정
        if (transform.position.x <= resetPositionX)
        {
            // 레벨을 오른쪽으로 재배치

            ResetLevelPosition();
        }

        if (Gamemanager.isClear && !Gamemanager.portalSpawned)
        {
            foreach (var spawnPoint in SpawnPoints)
            {
                if (spawnPoint.GetComponentInChildren<Enemy>() != null)
                    Destroy(spawnPoint.GetComponentInChildren<Enemy>().gameObject);
            }

            GameObject portal = Instantiate(Portal, new Vector3(SpawnPoints[1].transform.position.x, SpawnPoints[1].transform.position.y+1), Quaternion.identity);
            portal.transform.SetParent(SpawnPoints[1].transform);
            Gamemanager.portalSpawned = true;
        }
    }

    private void ResetLevelPosition()
    {
        // 기존 위치를 오른쪽으로 재배치
        transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);

        foreach (var spawnPoint in SpawnPoints)
        {
            if (spawnPoint.GetComponentInChildren<Enemy>() != null)
                Destroy(spawnPoint.GetComponentInChildren<Enemy>().gameObject);
        }

        if (!Gamemanager.isClear)
            foreach (var spawnPoint in SpawnPoints)
            {
                GameObject enemy = Instantiate(Enemys[Random.Range(0, Enemys.Count)], spawnPoint.transform.position, Quaternion.identity);

                enemy.transform.SetParent(spawnPoint.transform);
            }

        speed += 1.0f; // 속도를 증가시킴



        // 필요하다면 새로운 레벨을 생성하거나 기존 레벨을 재활용 가능
        // Instantiate(levelPrefab, new Vector3(startPositionX, transform.position.y, transform.position.z), Quaternion.identity);
    }
}
