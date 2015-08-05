using UnityEngine;

public static class CameraExtensions
{
    public static Rect GetPositionedViewportBounds(this Camera camera)
    {
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));

        return new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

    private static Vector2 _viewportCenterToEdgeDistances = Vector2.zero;

    public static Vector2 GetViewportCenterToEdgeDistances(this Camera camera)
    {
        if (_viewportCenterToEdgeDistances == Vector2.zero)
        {
            Rect cameraViewport = camera.GetPositionedViewportBounds();
            _viewportCenterToEdgeDistances = new Vector2(cameraViewport.xMax - cameraViewport.xMin, cameraViewport.yMax - cameraViewport.yMin) / 2.0f;
        }

        return _viewportCenterToEdgeDistances;
    }

    public static bool IsOffScreen(this Camera camera, Vector2 position, Vector2 margins)
    {
        Rect cameraViewport = camera.GetPositionedViewportBounds();

        if (position.x + margins.x < cameraViewport.xMin) { return true; }
        if (position.x - margins.x > cameraViewport.xMax) { return true; }
        if (position.y + margins.y < cameraViewport.yMin) { return true; }
        if (position.y - margins.y > cameraViewport.yMax) { return true; }

        return false;
    }

    public static bool IsOffScreen(this Camera camera, Vector2 position)
    {
        return camera.IsOffScreen(position, Vector2.zero);
    }

    public static bool IsOnScreen(this Camera camera, Vector2 position, Vector2 margins)
    {
        return !camera.IsOffScreen(position, margins);
    }

    public static bool IsOnScreen(this Camera camera, Vector2 position)
    {
        return !camera.IsOffScreen(position, Vector2.zero);
    }

    public static bool IsOffBottomOfScreen(this Camera camera, Vector2 position, float margin)
    {
        return (camera.GetPositionedViewportBounds().yMin > position.y + margin);
    }
}
