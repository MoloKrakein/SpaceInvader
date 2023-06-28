using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    public Projectile projectileManager;
    public float shootModifier = 1f;
    public System.Action killed;
    public bool isShoot{get;private set;}
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        // if horizontal input is not 0
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            position.x -= speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            position.x += speed * Time.deltaTime;
        }

        Vector3 screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        position.x = Mathf.Clamp(position.x, screenLeft.x, screenRight.x);
        transform.position = position;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")|| collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            if (killed != null)
            {
                killed.Invoke();
            }
            // Destroy(gameObject);
        }
    }

    private void Shoot(){
        if(!isShoot){
            Debug.Log("Shoot");
            isShoot = true;
            Vector3 shootPosition = transform.position + Vector3.up * shootModifier;
            Projectile projectile = Instantiate(projectileManager, shootPosition, Quaternion.identity);

            projectile.OnProjectileDestroyed += OnProjectileDestroyed;
        }
    }

    private void OnProjectileDestroyed(Projectile projectile){
        isShoot = false;
    }


}
