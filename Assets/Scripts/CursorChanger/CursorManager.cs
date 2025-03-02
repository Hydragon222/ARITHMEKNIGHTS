using UnityEngine;

public class CursorManager : MonoBehaviour
{
    
    [Header("Cursor textures")]
    [SerializeField] private Texture2D[] cursors;

    private bool isOverUi = false;

    public static CursorManager CursorManagerInstance { get; private set; }

    private void Awake()
    {
        if(CursorManagerInstance != null && CursorManagerInstance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            CursorManagerInstance = this; 
        } 
    }

    private void Start()=> SetCursor(0);

    public void ToggleUiCursor(bool toggle)
    {
        isOverUi = toggle;
        if(isOverUi)
        {
            SetCursor(2);
        }
        else
        {
            SetCursor(0);
        }

    }

    private void SetCursor(int cursorIndex)
    {
        if (cursors[cursorIndex] == null) return;

        Vector2 hotspot = new Vector2(cursors[cursorIndex].width / 2, cursors[cursorIndex].height / 2);
        Cursor.SetCursor(cursors[cursorIndex], hotspot, CursorMode.Auto);
    }

    private void Update()
    {
        if(!isOverUi)
        {
            if(Input.GetMouseButton(0))
            {
                SetCursor(1);
            } 
            else
            {
                SetCursor(0);
            }
        }
    }
}
