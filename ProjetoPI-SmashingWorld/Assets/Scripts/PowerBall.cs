using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBall : MonoBehaviour
{
    // Start is called before the first frame update
    public string creator_tag;
    public float damage;
    [SerializeField]
    int impulseForce = 1;
    public int direction = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
        {
            GameObject player = collision.gameObject;
            if(!player.CompareTag(creator_tag))
            {
                player.GetComponent<PlayerController>().TakeHit(damage);
                SelfDestruct();
            }
        }
    }

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 0)* direction * impulseForce, ForceMode2D.Impulse);
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
