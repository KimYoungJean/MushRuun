using System.Collections.Generic;
using UnityEngine;

public class InfiniteLevel : MonoBehaviour
{
    public float speed = 8f;  // ������ �̵��ϴ� �ӵ�
    public float resetPositionX = -20f;  // ������ �缳���� X ��ġ (ȭ�� ���� ��)
    public float startPositionX = 20f;   // ������ ó�� ��Ÿ���� X ��ġ (ȭ�� ������ ��)

    public List<GameObject> SpawnPoints;
    public List<GameObject> Enemys;
    public GameObject Portal;

    public GameObject levelPrefab; // ������ ������ ������

    private Vector3 initialPosition; // ���� ��ġ�� �����ϴ� ����

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // ������ �������� �̵�
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // ������ ȭ�� �������� ������� ��ġ�� �缳��
        if (transform.position.x <= resetPositionX)
        {
            // ������ ���������� ���ġ

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
        // ���� ��ġ�� ���������� ���ġ
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

        speed += 1.0f; // �ӵ��� ������Ŵ



        // �ʿ��ϴٸ� ���ο� ������ �����ϰų� ���� ������ ��Ȱ�� ����
        // Instantiate(levelPrefab, new Vector3(startPositionX, transform.position.y, transform.position.z), Quaternion.identity);
    }
}
