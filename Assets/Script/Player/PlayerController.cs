using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;  // ���� ��
    public Animator animator;  // Animator ������Ʈ
    private Rigidbody2D rb;

    public GameObject jumpEffect;

    private bool isGrounded;  // ĳ���Ͱ� ���� �ִ��� ����
    private int jumpCount = 0;  // ���� ī��Ʈ
    private const int maxJumpCount = 1;  // �ִ� 2�� ���� ����

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D ������Ʈ ��������
        animator = GetComponent<Animator>();  // Animator ������Ʈ ��������
    }

    private void Update()
    {
        // ���� �Է� ó�� (�ִ� 2�� ���� ���)
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            Jump();
        }

        // ���߿� ���� �� �ִϸ��̼� ����
        if (!isGrounded)
        {
            animator.speed = 0;
        }
    }

    private void Jump()
    {
        // ���� ����Ʈ ����
        jumpEffect.transform.position = transform.position;  // ����Ʈ�� ĳ���� ��ġ�� ����
        jumpEffect.GetComponent<Animator>().SetTrigger("jump");

        // Rigidbody2D�� �������� ���� ���� ����
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpCount++;  // ������ ������ ī��Ʈ ����

    }

    // �浹�� ���ӵ� �� ȣ��� (���� ������� ��)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �ٴ� �±׸� ���� ������Ʈ�� ��� ���� ��
        if (collision.gameObject.name == "Level")
        {
            isGrounded = true;
            jumpCount = 0;  // �����ϸ� ���� ī��Ʈ �ʱ�ȭ
            animator.speed = 1;  // �ִϸ��̼� �ٽ� ���
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // �ٴ� �±׸� ���� ������Ʈ�� ��� ���� ��
        if (collision.gameObject.name == "Level")
        {
            isGrounded = true;
            jumpCount = 0;  // �����ϸ� ���� ī��Ʈ �ʱ�ȭ
            animator.speed = 1;  // �ִϸ��̼� �ٽ� ���
        }
    }

    // �浹���� ����� �� ȣ��� (���߿� ���� ��)
    private void OnCollisionExit2D(Collision2D collision)
    {
        // �ٴ� �±׸� ���� ������Ʈ���� ��� ��
        if (collision.gameObject.name == "Level")
        {
            isGrounded = false;  // ������ ������
        }
    }
}
