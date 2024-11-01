using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    public GameObject leaderboardEntryPrefab;  // �������� �׸��� ǥ���� ������
    public Transform leaderboardContainer;  // �������带 ǥ���� �θ� ������Ʈ

    private DatabaseReference dbReference;

    private void OnEnable()
    
    {
        // Firebase �����ͺ��̽� ���� ����
        dbReference = FirebaseDatabase.DefaultInstance.GetReference("players");

        Debug.Log("Leaderboard enabled");
        // �������� �����͸� Firebase���� ��������
        LoadLeaderboardData();
    }

    // �������� �����͸� Firebase���� �о�� ���� �� UI�� ǥ���ϴ� �Լ�
    private void LoadLeaderboardData()
    {
        Debug.Log("Loading leaderboard data...");
        dbReference.OrderByChild("playScore").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Leaderboard data loaded successfully.");
                DataSnapshot snapshot = task.Result;
                List<PlayerData> leaderboardData = new List<PlayerData>();

                Debug.Log($"Player count: {snapshot.ChildrenCount}");
                foreach (DataSnapshot playerSnapshot in snapshot.Children)
                {
                    Debug.Log("Player data: " + playerSnapshot.Child("playerName").Value + " - " + playerSnapshot.Child("playerScore").Value);
                    string nickname = playerSnapshot.Child("playerName").Value.ToString();
                    float score = float.Parse(playerSnapshot.Child("playerScore").Value.ToString());
                    leaderboardData.Add(new PlayerData(nickname, score));


                    
                }

                // ���� ������ �������� ����
                leaderboardData = leaderboardData.OrderByDescending(player => player.playerScore).ToList();

                // �������� UI ������Ʈ
                UpdateLeaderboardUI(leaderboardData);
            }
            else
            {
                Debug.LogError("Failed to retrieve leaderboard data from Firebase.");
            }
        });
    }

    // �������� UI�� ������Ʈ�ϴ� �Լ�
    private void UpdateLeaderboardUI(List<PlayerData> leaderboardData)
    {
        Debug.Log("Updating leaderboard UI...");
        // ���� �������� �׸� ����
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        // ���ĵ� �����͸� ������� UI�� ǥ��
        /*foreach (PlayerData player in leaderboardData)
        int rank = 1;
        {
            GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
            entry.transform.Find("Ranking").GetComponent<TextMeshProUGUI>().text = rank.ToString();
            entry.transform.Find("Nickname").GetComponent<TextMeshProUGUI>().text = player.playerName;
            entry.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = player.playerScore.ToString();

            rank++;
        }*/

        for (int i = 1; i < 7; i++)
        {
            GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
            entry.transform.Find("Ranking").GetComponent<TextMeshProUGUI>().text = i.ToString();
            entry.transform.Find("Nickname").GetComponent<TextMeshProUGUI>().text = leaderboardData[i - 1].playerName;
            entry.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = leaderboardData[i - 1].playerScore.ToString();
        }
    }
}


