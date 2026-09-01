using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class ArkanoidPlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float ballShootSpeed;
    Rigidbody2D rb;

    public Transform ballPos;
    public GameObject ballPrefab;
    GameObject currentBall;

    bool isPlaying = false;

    public float leftBoundary = -2.0f;
    public float rightBoundary = 2.0f;

    public int maxHealth = 3;
    private int currentHealth;
    public Text healthText;

    public int maxBalls = 3;
    private int ballsRemaining;
    public Text ballsText;

    public GameObject gameOverPanel;
    private bool isGameOver = false;

    public ReturnToPrevSceneEventSO returnEvent;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        UpdateHealthUI();

        ballsRemaining = maxBalls;
        UpdateBallsUI();

        SpawnNewBall();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        rb.velocity = Vector2.right * h * moveSpeed;

        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, leftBoundary, rightBoundary);
        transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Space) && !isPlaying)
        {
            isPlaying = true;
            ShootBall();
        }
    }

    public void SpawnNewBall()
    {
        isPlaying = false;
        currentBall = Instantiate(ballPrefab, ballPos.position, ballPrefab.transform.rotation);
        currentBall.transform.parent = transform;
        currentBall.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    void ShootBall()
    {
        currentBall.transform.parent = null;
        currentBall.GetComponent<Rigidbody2D>().isKinematic = false;
        currentBall.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), 1).normalized * ballShootSpeed;
    }

    public void OnPlayerHit()
    {
        if (isGameOver) return;

        currentHealth--;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            ShowGameOver();
        }
        // SpawnNewBall()
    }

    public void OnBallLost()
    {
        if (isGameOver) return; 

        ballsRemaining--;
        UpdateBallsUI();

        if (ballsRemaining > 0)
        {
            SpawnNewBall();
        }
        else
        {
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("Game Over!");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            GameWorldPauser.Pause();
        }

        Invoke(nameof(ReturnToPreviousScene), 1.5f);
    }

    private void ReturnToPreviousScene()
    {
        GameWorldPauser.Resume();
        returnEvent?.RaiseEvent();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString();
        }
    }

    private void UpdateBallsUI()
    {
        if (ballsText != null)
        {
            ballsText.text = "Balls: " + ballsRemaining.ToString();
        }
    }
}