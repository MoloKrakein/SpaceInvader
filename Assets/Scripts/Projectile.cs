using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 20f;
    public Vector3 direction = Vector3.up;

    public System.Action<Projectile> OnProjectileDestroyed;

    public new BoxCollider2D collider {get; private set;}
    private void Awake() {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnDestroy() {
        if(OnProjectileDestroyed != null){
            OnProjectileDestroyed.Invoke(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;


    }

    private void collisionCheck(Collider2D collision){
        CoverWall coverWall = collision.gameObject.GetComponent<CoverWall>();
        if(coverWall == null || coverWall.CheckCollision(collider,transform.position)){
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        collisionCheck(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        collisionCheck(collision);
    }
}
