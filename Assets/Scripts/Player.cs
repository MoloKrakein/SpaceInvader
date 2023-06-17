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

        // use unity new input system
        position.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        position.y += Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 screenRight = Camera.main.ScreenToWorldPoint(Vector3.right);

        position.x = Mathf.Clamp(position.x, screenLeft.x, screenRight.x);

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
}
