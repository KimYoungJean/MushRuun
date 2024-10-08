using System.Collections;
using System.Collections.Generic;
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
        Time.timeScale = 0;
        GameoverPanel.SetActive(true);

    }

    IEnumerator Hero()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);  // 面倒 公矫
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
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, false);  // 面倒 公矫 秦力
            }
        }
    }
}
