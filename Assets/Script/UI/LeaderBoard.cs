using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    public GameObject leaderboardEntryPrefab;  // 리더보드 항목을 표시할 프리팹
    public Transform leaderboardContainer;  // 리더보드를 표시할 부모 오브젝트

    private DatabaseReference dbReference;

    private void OnEnable()
    
    {
        // Firebase 데이터베이스 참조 설정
        dbReference = FirebaseDatabase.DefaultInstance.GetReference("players");

        Debug.Log("Leaderboard enabled");
        // 리더보드 데이터를 Firebase에서 가져오기
        LoadLeaderboardData();
    }

    // 리더보드 데이터를 Firebase에서 읽어와 정렬 후 UI에 표시하는 함수
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

                // 점수 순으로 내림차순 정렬
                leaderboardData = leaderboardData.OrderByDescending(player => player.playerScore).ToList();

                // 리더보드 UI 업데이트
                UpdateLeaderboardUI(leaderboardData);
            }
            else
            {
                Debug.LogError("Failed to retrieve leaderboard data from Firebase.");
            }
        });
    }

    // 리더보드 UI를 업데이트하는 함수
    private void UpdateLeaderboardUI(List<PlayerData> leaderboardData)
    {
        Debug.Log("Updating leaderboard UI...");
        // 기존 리더보드 항목 제거
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        // 정렬된 데이터를 순서대로 UI에 표시
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


