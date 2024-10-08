using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;  // 점프 힘
    public Animator animator;  // Animator 컴포넌트
    private Rigidbody2D rb;

    public GameObject jumpEffect;

    private bool isGrounded;  // 캐릭터가 땅에 있는지 여부
    private int jumpCount = 0;  // 점프 카운트
    private const int maxJumpCount = 1;  // 최대 2단 점프 가능

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D 컴포넌트 가져오기
        animator = GetComponent<Animator>();  // Animator 컴포넌트 가져오기
    }

    private void Update()
    {
        // 점프 입력 처리 (최대 2단 점프 허용)
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            Jump();
        }

        // 공중에 있을 때 애니메이션 멈춤
        if (!isGrounded)
        {
            animator.speed = 0;
        }
    }

    private void Jump()
    {
        // 점프 이펙트 생성
        jumpEffect.transform.position = transform.position;  // 이펙트를 캐릭터 위치에 생성
        jumpEffect.GetComponent<Animator>().SetTrigger("jump");

        // Rigidbody2D에 위쪽으로 힘을 더해 점프
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpCount++;  // 점프할 때마다 카운트 증가

    }

    // 충돌이 지속될 때 호출됨 (땅에 닿아있을 때)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥 태그를 가진 오브젝트에 닿아 있을 때
        if (collision.gameObject.name == "Level")
        {
            isGrounded = true;
            jumpCount = 0;  // 착지하면 점프 카운트 초기화
            animator.speed = 1;  // 애니메이션 다시 재생
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 바닥 태그를 가진 오브젝트에 닿아 있을 때
        if (collision.gameObject.name == "Level")
        {
            isGrounded = true;
            jumpCount = 0;  // 착지하면 점프 카운트 초기화
            animator.speed = 1;  // 애니메이션 다시 재생
        }
    }

    // 충돌에서 벗어났을 때 호출됨 (공중에 있을 때)
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 바닥 태그를 가진 오브젝트에서 벗어날 때
        if (collision.gameObject.name == "Level")
        {
            isGrounded = false;  // 땅에서 떨어짐
        }
    }
}
