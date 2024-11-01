using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviourPunCallbacks
{
    public TMP_InputField emailInputField;  // 이메일 입력 필드
    public TMP_InputField passwordInputField;  // 비밀번호 입력 필드
    public FirebaseManager authManager;

    public void Start()
    {
        AudioManager.instance.SkipSound();
        AudioManager.instance.ChangeVolume(1.0f);
        AudioManager.instance.PlaySound(AudioManager.instance.audioClips[0]);
    }
    public void OnLoginButtonClicked()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        authManager.Login(email, password, LoginScucess);  // 로그인 함수 호출
    }

    public void LoginScucess()
    {
        UIManager.userLogin = true;
        SceneManager.LoadScene("InGame");
    }

    

}