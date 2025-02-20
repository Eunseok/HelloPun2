using TMPro;
using UnityEngine;

namespace MiniGame
{
    public class MiniGameManager : MonoBehaviour
    {
        private const float ViewportTargetY = 0.2f; // 뷰포트 Y 타겟 상수값
        private const float ScoreMultiplier = 10.0f; // 점수 증가 배율

        public TMP_Text scoreText;
        public TMP_Text bestScoreText;
        public GameObject gamePanel;
        public GameObject gameOverPanel;
        public Camera cam;
        public GameObject obstaclePrefab;
        public Transform obstacleSpawnPoint;
        public float minSpawnInterval = 1.0f;
        public float maxSpawnInterval = 2.0f;
        public float spawnInterval = 1.0f;

        private float score;
        private float spawnTimer;
        private bool isGameOver;
        private bool isViewportAdjusted;

        public bool IsGameOver => isGameOver;
        bool isActive;

        void Start()
        {
            int bestScore = PlayerPrefs.GetInt("BestScore", 0);
            bestScoreText.text = $"Best Score: {bestScore}";
            
            Invoke(nameof(GameStart), 1f);
        }

        void GameStart()
        {
            isActive = true;
        }
        
        private void Update()
        {
            if (isGameOver) return;

            UpdateViewportAdjustment();

            if (!isActive) return;
            
            UpdateScore();
            UpdateObstacleSpawning();
        }

        private void UpdateViewportAdjustment()
        {
            if (isViewportAdjusted) return;

            AdjustViewportY(ViewportTargetY);
            if (Mathf.Approximately(cam.rect.y, ViewportTargetY))
            {
                isViewportAdjusted = true;
            }
        }

        private void UpdateScore()
        {
            score += Time.deltaTime * ScoreMultiplier; // 시간에 비례한 점수 증가
            scoreText.text = $"Score: {Mathf.FloorToInt(score)}";
        }

        private void UpdateObstacleSpawning()
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnInterval)
            {
                spawnTimer = 0f;
                SpawnObstacle();
            }
        }

        private void AdjustViewportY(float viewportTargetY)
        {
            float currentY = cam.rect.y;
            float smoothY = Mathf.Lerp(currentY, viewportTargetY, Time.deltaTime * 8f); // 부드러운 보간
            cam.rect = new Rect(cam.rect.x, smoothY, cam.rect.width, cam.rect.height);
        }

        private void SpawnObstacle()
        {
            spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            GameObject obstacle = Instantiate(obstaclePrefab, obstacleSpawnPoint.position, Quaternion.identity);
            obstacle.transform.SetParent(obstacleSpawnPoint);
        }

        public void GameOver()
        {
            isGameOver = true;
            gameOverPanel.SetActive(true);

            int bestScore = PlayerPrefs.GetInt("BestScore", 0);
            if (score > bestScore)
            {
                bestScore = Mathf.FloorToInt(score);
                PlayerPrefs.SetInt("BestScore", bestScore);
            }

            bestScoreText.text = $"Best Score: {bestScore}";

            Invoke(nameof(DestroyMySelf), 3.0f); // nameof를 사용
        }

        private void DestroyMySelf()
        {
            Destroy(this.gameObject);
        }
    }
    
}