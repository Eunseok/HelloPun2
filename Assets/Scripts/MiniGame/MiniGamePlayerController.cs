using UnityEngine;

namespace MiniGame
{
    public class MiniGamePlayerController : MonoBehaviour
    {
        private Rigidbody2D rb;
        private bool isGrounded;

        public float jumpForce = 5.0f;

        // MiniGameScoreManager 참조
        public MiniGameManager scoreManager;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (scoreManager.IsGameOver) return;
        
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
        }

        private void Jump()
        {
            rb.velocity = Vector2.up * jumpForce;
            isGrounded = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
         
                isGrounded = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Obstacle")) // 장애물과 충돌 감지
            {
                scoreManager.GameOver();
            }
        }
    }
}