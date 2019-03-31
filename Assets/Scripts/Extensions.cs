using UnityEngine;

public static class Extensions
{
    public static Bounds OrthographicBounds(this Camera camera)
    {
        if (!camera.orthographic)
        {
            Debug.Log(string.Format("The camera {0} is not Orthographic!", camera.name), camera);
            return new Bounds();
        }

        var t = camera.transform;
        var x = t.position.x;
        var y = t.position.y;
        var size = camera.orthographicSize * 2;
        var width = size * (float)Screen.width / Screen.height;
        var height = size;

        return new Bounds(new Vector3(x, y, 0), new Vector3(width, height, 0));
    }
}
