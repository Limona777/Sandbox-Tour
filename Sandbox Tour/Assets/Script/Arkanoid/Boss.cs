using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Move")]
    public Vector3[] movePoints;
    public float moveInterval = 2f;

    [Header("Bullet")]
    public bool enableFire = true;
    public bool enableSpread = true;
    public bool enableVerticalShoot = true;
    public GameObject bulletPrefab;
    public float fireInterval = 0.5f;
    public float spreadInterval = 3f;
    public int spreadCount = 12;
    public float bulletSpeed = 5f;
    public float verticalShootInterval = 1.5f;
    public float verticalMinX = -2.5f;
    public float verticalMaxX = 2.5f;
    public float topY = 5f; 
    public float bottomY = -5f;

    public Transform player;

    private int currentPointIndex = 0;

    void Start()
    {
        player = FindObjectOfType<ArkanoidPlayerController>().transform;

        if (enableFire)
            StartCoroutine(FireRoutine());
        if (enableSpread)
            StartCoroutine(SpreadRoutine());
        if (enableVerticalShoot)
            StartCoroutine(VerticalShootRoutine());

        if (movePoints.Length > 0)
        {
            StartCoroutine(MoveRoutine());
        }
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            transform.position = movePoints[currentPointIndex];
            yield return new WaitForSeconds(moveInterval);
            currentPointIndex = (currentPointIndex + 1) % movePoints.Length;
        }
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            if (player == null) continue;

            Vector2 dir = (player.position - transform.position).normalized;
            FireBullet(dir);
        }
    }

    IEnumerator SpreadRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spreadInterval);
            float angleStep = 360f / spreadCount;
            for (int i = 0; i < spreadCount; i++)
            {
                float angle = i * angleStep;
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                FireBullet(dir);
            }
        }
    }

    IEnumerator VerticalShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(verticalShootInterval);

            float randomXTop = Random.Range(verticalMinX, verticalMaxX);
            float randomXBottom = Random.Range(verticalMinX, verticalMaxX);

            Vector3 upPos = new Vector3(randomXTop, topY, 0);
            GameObject bulletUp = Instantiate(bulletPrefab, upPos, Quaternion.identity);
            Rigidbody2D rbUp = bulletUp.GetComponent<Rigidbody2D>();
            rbUp.velocity = Vector2.down * bulletSpeed;
            bulletUp.transform.rotation = Quaternion.Euler(0, 0, -90);

            Vector3 downPos = new Vector3(randomXBottom, bottomY, 0);
            GameObject bulletDown = Instantiate(bulletPrefab, downPos, Quaternion.identity);
            Rigidbody2D rbDown = bulletDown.GetComponent<Rigidbody2D>();
            rbDown.velocity = Vector2.up * bulletSpeed;
            bulletDown.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    void FireBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }
}