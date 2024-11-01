using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviourPunCallbacks
{
    public TMP_InputField emailInputField;  // �̸��� �Է� �ʵ�
    public TMP_InputField passwordInputField;  // ��й�ȣ �Է� �ʵ�
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

        authManager.Login(email, password, LoginScucess);  // �α��� �Լ� ȣ��
    }

    public void LoginScucess()
    {
        UIManager.userLogin = true;
        SceneManager.LoadScene("InGame");
    }

    

}