using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public bool isGoldBrick;
    public int maxHP = 1;
    int hp;

    public GameObject exploPrefab;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isGoldBrick)
            return;
        hp--;
        if(hp <= 0)
        {
            Destroy(gameObject);
            Instantiate(exploPrefab, transform.position, Quaternion.identity);
            FindObjectOfType<BricksHolder>().BrickGetDestroy();
        }
        
    }
}
