using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    public List<Texture2D> clickSprite;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Sprite를 Texture2D로 변환
        
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Click());
        }
    }
    IEnumerator Click()
    {

        Texture2D texture = clickSprite[0];
        Cursor.SetCursor(texture, hotspot, cursorMode);
        yield return new WaitForSeconds(0.1f);

        texture = clickSprite[1];
        Cursor.SetCursor(texture, hotspot, cursorMode);
        yield return new WaitForSeconds(0.1f);

        texture = clickSprite[2];
        Cursor.SetCursor(texture, hotspot, cursorMode);
        yield return new WaitForSeconds(0.1f);

        texture = cursorTexture;
        Cursor.SetCursor(texture, hotspot, cursorMode);
    }
}
