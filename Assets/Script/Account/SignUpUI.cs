using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpUI : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Button signUpButton;

    private FirebaseManager authManager;

    private void Start()
    {
        authManager = FindObjectOfType<FirebaseManager>();
        signUpButton.onClick.AddListener(OnSignUpButtonClicked);
    }

    private void OnSignUpButtonClicked()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        authManager.SignUp(email, password);
        
    }
}
