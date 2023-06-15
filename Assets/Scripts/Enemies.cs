using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public Enemy[] prefabs = new Enemy[5];
    public AnimationCurve speed = new AnimationCurve();
    public Vector3 direction { get; private set; } = Vector3.right;
    public Vector3 initialPosition { get; private set; }
    public System.Action<Enemy> killed;

    public int TotalKilled{ get; private set; }
    public int TotalAlive => TotalAmount - TotalKilled;
    public int TotalAmount => rows * columns;
    public float percentKilled => (float)TotalKilled / (float)TotalAmount;

    [Header("Grid Management")]
    public int rows = 5;
    public int columns = 11;

    public float spacingModifier = 2f;

    [Header("Missiles Management")]
    // public Projectile missilePrefab;
    public float missileSpawnRate = 1f;


    private void Awake() {
        initialPosition = transform.position;

        // Form the grid of enemies
        for (int i = 0; i < rows; i++) {
            float width = 2f * (columns - 1);
            float height = 2f * (rows - 1);

            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);

            for (int j = 0; j < columns; j++) {
                // Create an enemy and parent it to this transform
                Enemy enemy = Instantiate(prefabs[i], transform);
                enemy.killed += OnEnemyKilled;

                // Calculate and set the position of the enemy in the row
                Vector3 position = rowPosition;
                position.x += spacingModifier * j;
                position.y += spacingModifier * i;
                enemy.transform.localPosition = position;
            }
        }
    }
    void Start()
    {
        InvokeRepeating(nameof(MissileShoot), missileSpawnRate, missileSpawnRate);
        
    }

    private void MissileShoot()
    {
        int amountAlive = TotalAlive;
        if (amountAlive > 0) {
            // Pick a random invader to fire the missile
            int randomIndex = Random.Range(0, amountAlive);
            Enemy randomInvader = GetAliveEnemy(randomIndex);

            // Spawn the missile
            // Projectile missile = Instantiate(missilePrefab, transform);
            // missile.transform.position = randomInvader.transform.position;
            // missile.direction = Vector3.down;
        }
    }

    private Enemy GetAliveEnemy(int index)
    {
        int currentIndex = 0;
        foreach (Transform child in transform) {
            Enemy invader = child.GetComponent<Enemy>();
            if (invader != null) {
                if (currentIndex == index) {
                    return invader;
                }
                currentIndex++;
            }
        }
        return null;
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        TotalKilled++;
        killed?.Invoke(enemy);
    }

    // private void MoveRow()
    // {
    //     direction = new Vector3(-direction.x, 0f, 0f);

    //     Vector3 position = transform.position;
    //     position.y -= 1f;
    //     transform.position = position;
    // }

    // Update is called once per frame
    void Update()
    {
        
        float speed = this.speed.Evaluate(percentKilled);
        transform.position += direction * speed * Time.deltaTime;

        Vector3 min = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 max = Camera.main.ViewportToWorldPoint(Vector3.one);

        foreach (Transform enemy in transform) {
           if(!enemy.gameObject.activeInHierarchy) continue;
                 if (direction == Vector3.right && enemy.position.x >= (min.x - 1f))
            {
                // MoveRow();
                break;
            }
            else if (direction == Vector3.left && enemy.position.x <= (max.x + 1f))
            {
                // MoveRow();
                break;
            }
        }
    }
}
