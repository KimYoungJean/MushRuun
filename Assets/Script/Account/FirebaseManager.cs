using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine;
using Unity.Mathematics;
using System;
using UnityEngine.SceneManagement;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference dbReference;
    private FirebaseAuth auth;
    public static string Nickname;

    public static FirebaseManager instance;

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

        DontDestroyOnLoad(gameObject);

    }


    private void Start()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Firebase 연결 성공
                Debug.Log("Firebase Initialized");

                // Realtime Database 사용 설정
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });
    }


    public void SavePlayerData(string playerName, float playerScore)
    {
        // 플레이어 데이터가 저장된 경로 참조
        DatabaseReference playerRef = dbReference.Child("players").Child(playerName);

        // 기존 데이터 가져오기
        playerRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    // 기존 점수를 가져옴
                    float existingScore = float.Parse(snapshot.Child("playerScore").Value.ToString());

                    // 기존 점수와 새 점수를 비교
                    if (playerScore > existingScore)
                    {
                        Debug.Log("New score is higher. Updating score.");
                        // 새 점수가 더 높으면 덮어쓰기
                        PlayerData data = new PlayerData(playerName, playerScore);
                        string jsonData = JsonUtility.ToJson(data);
                        playerRef.SetRawJsonValueAsync(jsonData).ContinueWithOnMainThread(updateTask =>
                        {
                            if (updateTask.IsCompleted)
                            {
                                Debug.Log("Score updated successfully.");
                            }
                            else
                            {
                                Debug.LogError("Failed to update score.");
                            }
                        });
                    }
                    else
                    {
                        Debug.Log("Existing score is higher. Keeping the old score.");
                    }
                }
                else
                {
                    // 플레이어 데이터가 없을 경우 새로 저장
                    Debug.Log("No existing data. Saving new score.");
                    PlayerData data = new PlayerData(playerName, playerScore);
                    string jsonData = JsonUtility.ToJson(data);
                    playerRef.SetRawJsonValueAsync(jsonData).ContinueWithOnMainThread(saveTask =>
                    {
                        if (saveTask.IsCompleted)
                        {
                            Debug.Log("Data successfully written to Firebase.");
                        }
                        else
                        {
                            Debug.LogError("Failed to write data to Firebase.");
                        }
                    });
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve data from Firebase.");
            }
        });
    }
    public void SignUp(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                // task.Result는 AuthResult 객체를 반환합니다.
                AuthResult authResult = task.Result; // AuthResult 객체
                FirebaseUser newUser = authResult.User; // FirebaseUser 객체
           

                Debug.Log($"User created successfully: {newUser.Email}, {newUser.UserId}");
            }
            else
            {
                // 에러가 발생한 경우 처리
                FirebaseException firebaseEx = task.Exception?.Flatten().InnerExceptions[0] as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                switch (errorCode)
                {
                    case AuthError.WeakPassword:
                        Debug.LogError("Password is too weak.");
                        break;
                    case AuthError.EmailAlreadyInUse:
                        Debug.LogError("Email is already in use.");
                        break;
                    case AuthError.InvalidEmail:
                        Debug.LogError("Invalid email.");
                        break;
                    default:
                        Debug.LogError($"Sign up failed: {task.Exception}");
                        break;
                }
            }
        });
    }
    public void Login(string email, string password, Action LoginScucess)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                // 로그인 성공
                AuthResult authResult = task.Result; // AuthResult 객체
                FirebaseUser User = authResult.User; // FirebaseUser 객체
                Nickname = User.Email.Split('@')[0];
                Debug.Log($"User logged in successfully: {User.Email}, {User.UserId}");
                LoginScucess?.Invoke();
            }
            else
            {
                // 로그인 실패 처리
                FirebaseException firebaseEx = task.Exception?.Flatten().InnerExceptions[0] as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                switch (errorCode)
                {
                    case AuthError.InvalidEmail:
                        Debug.LogError("Invalid email format.");
                        break;
                    case AuthError.WrongPassword:
                        Debug.LogError("Incorrect password.");
                        break;
                    case AuthError.UserNotFound:
                        Debug.LogError("User not found.");
                        break;
                    default:
                        Debug.LogError($"Login failed: {task.Exception}");
                        break;
                }
            }
        });
    }
    public void GuestLogin()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                AuthResult authResult = task.Result; // AuthResult 객체
                FirebaseUser newUser = authResult.User; // FirebaseUser 객체
                int Ran = UnityEngine.Random.Range(0, 1000);
                Nickname = $"Guest_{Ran}";
                Debug.Log($"Guest user logged in: {newUser.UserId}");

                SceneManager.LoadScene("InGame");
            }
            else
            {
                Debug.LogError($"Guest login failed: {task.Exception}");
            }
        });

    }

}
// 플레이어 데이터 클래스
[System.Serializable]
public class PlayerData
{
    public string playerName;
    public float playerScore;

    public PlayerData(string playerName, float playerScore)
    {
        this.playerName = playerName;
        this.playerScore = playerScore;
    }
}
