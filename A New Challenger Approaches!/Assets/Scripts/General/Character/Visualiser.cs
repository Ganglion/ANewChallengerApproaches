using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Visualiser {

    public static void DrawRectangle(Vector3 position, Vector2 rectangleSize, Color color) {
        float halfWidth = rectangleSize.x / 2;
        float halfHeight = rectangleSize.y / 2;
        Vector2 topLeft = new Vector2(position.x - halfWidth, position.y + halfHeight);
        Vector2 topRight = new Vector2(position.x + halfWidth, position.y + halfHeight);
        Vector2 botLeft = new Vector2(position.x - halfWidth, position.y - halfHeight);
        Vector2 botRight = new Vector2(position.x + halfWidth, position.y - halfHeight);
        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topLeft, botLeft, color);
        Debug.DrawLine(botRight, topRight, color);
        Debug.DrawLine(botRight, botLeft, color);
    }

}
