using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CoverWall : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }

    public new BoxCollider2D collider { get; private set; }


    public Texture2D splashTexture;

    public Texture2D originalTexture{ get; private set; }

    public int health = 100;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        originalTexture = spriteRenderer.sprite.texture;

        resetWall();

        // debug log box collider size
        Debug.Log(collider.size);
    }

    private void resetWall()
    {
        Copycat(originalTexture);

        gameObject.SetActive(true);
    }

    private void Copycat(Texture2D source)
    {
        Texture2D copy = new Texture2D(source.width, source.height, source.format, false);
        copy.filterMode = source.filterMode;
        copy.anisoLevel = source.anisoLevel;
        copy.wrapMode = source.wrapMode;
        copy.SetPixels(source.GetPixels());
        copy.Apply();

        Sprite sprite = Sprite.Create(copy, spriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f), spriteRenderer.sprite.pixelsPerUnit);
        spriteRenderer.sprite = sprite;
    }

    public bool checkpoint(Vector3 hit, out int px, out int py)
    {
        Vector3 localPos = transform.InverseTransformPoint(hit);

        localPos.x += collider.size.x;
        localPos.y += collider.size.y;

        Texture2D texture = spriteRenderer.sprite.texture;
        
        px = (int)((localPos.x / collider.size.x) * texture.width);
        py = (int)((localPos.y / collider.size.y) * texture.height);

        // debug log pixel position
        // Debug.Log(px + " " + py);
        return texture.GetPixel(px, py).a == 0f;
    }

    public bool splash(Vector3 hitPoint)
    {
        int px;
        int py;

        if (checkpoint(hitPoint, out px, out py))
        {
            return false;
        }

        Texture2D texture = spriteRenderer.sprite.texture;
        px -= splashTexture.width;
        py -= splashTexture.height;

        int startX = px;

        for (int x = 0; x < splashTexture.width; x++)
        {
            for (int y = 0; y < splashTexture.height; y++)
            {
                Color splashColor = splashTexture.GetPixel(x, y);
                Color wallColor = texture.GetPixel(px, py);

                Color finalColor = Color.Lerp(wallColor, splashColor, splashColor.a);

                texture.SetPixel(px, py, finalColor);

                py++;
            }
            px++;
            py -= splashTexture.height;
        }
        texture.Apply();
        health--;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }

        return true;
    }

    public bool CheckCollision(BoxCollider2D other, Vector3 hit)
    {
        Vector2 offset = other.offset;

        return splash(hit) || splash(hit + (Vector3.down * offset.y)) || splash(hit + (Vector3.up * offset.y)) || splash(hit + (Vector3.left * offset.x)) || splash(hit + (Vector3.right * offset.x));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
    
 
}
