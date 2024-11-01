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
        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Firebase ���� ����
                Debug.Log("Firebase Initialized");

                // Realtime Database ��� ����
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
        // �÷��̾� �����Ͱ� ����� ��� ����
        DatabaseReference playerRef = dbReference.Child("players").Child(playerName);

        // ���� ������ ��������
        playerRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    // ���� ������ ������
                    float existingScore = float.Parse(snapshot.Child("playerScore").Value.ToString());

                    // ���� ������ �� ������ ��
                    if (playerScore > existingScore)
                    {
                        Debug.Log("New score is higher. Updating score.");
                        // �� ������ �� ������ �����
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
                    // �÷��̾� �����Ͱ� ���� ��� ���� ����
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
                // task.Result�� AuthResult ��ü�� ��ȯ�մϴ�.
                AuthResult authResult = task.Result; // AuthResult ��ü
                FirebaseUser newUser = authResult.User; // FirebaseUser ��ü
           

                Debug.Log($"User created successfully: {newUser.Email}, {newUser.UserId}");
            }
            else
            {
                // ������ �߻��� ��� ó��
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
                // �α��� ����
                AuthResult authResult = task.Result; // AuthResult ��ü
                FirebaseUser User = authResult.User; // FirebaseUser ��ü
                Nickname = User.Email.Split('@')[0];
                Debug.Log($"User logged in successfully: {User.Email}, {User.UserId}");
                LoginScucess?.Invoke();
            }
            else
            {
                // �α��� ���� ó��
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
                AuthResult authResult = task.Result; // AuthResult ��ü
                FirebaseUser newUser = authResult.User; // FirebaseUser ��ü
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
// �÷��̾� ������ Ŭ����
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
