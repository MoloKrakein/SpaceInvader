using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 20f;
    public Vector3 direction;

    public System.Action<Projectile> OnProjectileDestroyed;

    public new BoxCollider2D collider {get; private set;}
    private void Awake() {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnDestroy() {
        if(OnProjectileDestroyed != null){
            OnProjectileDestroyed(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;


    }

    private void collisionCheck(Collider2D collision){
        if(collision.gameObject.tag == "Enemy"){
            
            Destroy(gameObject);
        }
    }
}
