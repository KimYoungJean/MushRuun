using UnityEngine.SceneManagement;
using UnityEngine;

public class OnQuit : MonoBehaviour
{
    public void OnQuibtnClick()
    {
        SceneManager.LoadScene("Login");
    }
}
