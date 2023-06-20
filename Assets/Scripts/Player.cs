using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public System.Action killed;
    public bool isShoot{get;private set;}
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
            isShoot = true;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (killed != null)
            {
                killed();
            }
            Destroy(gameObject);
        }
    }

    private void Shoot(){
        if(isShoot){
            isShoot = false;
            // GameObject bullet = Instantiate(Resources.Load<GameObject>("Bullet"));
            // bullet.transform.position = transform.position;
        }
    }
}
