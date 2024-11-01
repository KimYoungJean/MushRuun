using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    int currentHealth;
    int maxHealth = 100;

    public Slider healthSlider;
    public GameObject GameoverPanel;
    public Animator animator;
    public Collider2D playerCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void OnDamaged(int damage)
    {
        AudioManager.instance.PlaySound(AudioManager.instance.audioClips[6]);
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        healthSlider.value = currentHealth;
        Debug.Log("Player Health: " + currentHealth);
        animator.SetTrigger("OnDamaged");
        StartCoroutine(Hero());


    }

    void Die()
    {
        Debug.Log("Player is dead");
        animator.SetTrigger("Die");
        string playerName = FirebaseManager.Nickname;
        float playerScore = UIManager.time;  // UIManager에서 점수 가져오기
        // playerScore 둘쨰자리 까지만 표시
        playerScore = Mathf.Round(playerScore * 100) / 100;


        // Firebase에 데이터 저장
        FindObjectOfType<FirebaseManager>().SavePlayerData(playerName, playerScore);
        UIManager.instance.GameOver();


    }

    IEnumerator Hero()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);  // 충돌 무시
            }
        }

        GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f, 1.0f);
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1.0f);

        GameObject[] _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in _enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, false);  // 충돌 무시 해제
            }
        }
    }
}
