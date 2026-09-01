using UnityEngine;

public static class GameWorldPauser
{
    public static void Pause()
    {
        var player = Object.FindObjectOfType<ArkanoidPlayerController>();
        if (player != null) player.enabled = false;

        Boss boss = Object.FindObjectOfType<Boss>();
        if (boss != null) boss.enabled = false;

        Rigidbody2D[] allRb = Object.FindObjectsOfType<Rigidbody2D>();
        foreach (var rb in allRb)
        {
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }
        }

        Bullet[] bullets = Object.FindObjectsOfType<Bullet>();
        foreach (var b in bullets)
        {
            if (b != null) Object.Destroy(b.gameObject);
        }

        Ball[] balls = Object.FindObjectsOfType<Ball>();
        foreach (var b in balls)
        {
            if (b != null) Object.Destroy(b.gameObject);
        }
    }

    public static void Resume()
    {
        var player = Object.FindObjectOfType<ArkanoidPlayerController>();
        if (player != null) player.enabled = true;

        Boss boss = Object.FindObjectOfType<Boss>();
        if (boss != null) boss.enabled = true;

        Rigidbody2D[] allRb = Object.FindObjectsOfType<Rigidbody2D>();
        foreach (var rb in allRb)
        {
            if (rb != null) rb.isKinematic = false;
        }
    }
}