using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] animationSprites = new Sprite[0];
    public float animationTime = 1f;
    public int animationFrame { get; private set; }
    public int score = 10;
    public System.Action<Enemy> killed;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animationSprites[0];
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);

        
    }

    private void AnimateSprite()
    {
        animationFrame++;

        // Loop back to the start if the animation frame exceeds the length
        if (animationFrame >= animationSprites.Length) {
            animationFrame = 0;
        }

        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile")) {
            killed?.Invoke(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
