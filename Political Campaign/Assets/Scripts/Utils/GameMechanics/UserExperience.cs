using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserExperience : MonoBehaviour
{

    public Texture2D cursorMainImage;

    private void Start() {
        Vector2 cursorHotspot = new Vector2 (cursorMainImage.width / 2, cursorMainImage.height / 2);
        Cursor.SetCursor(cursorMainImage, cursorHotspot, CursorMode.ForceSoftware);
    }
}
