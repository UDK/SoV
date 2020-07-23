using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToSizeCamera : MonoBehaviour
{
    void Start()
    {
        Camera camera = Camera.main;
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        Vector3 objectSize = Vector3.Scale(transform.localScale, mesh.bounds.size);
        Rect rect = BoundsToScreenRect(gameObject.GetComponent<MeshRenderer>().bounds);
        float xScale = (float)Camera.main.pixelWidth / rect.width;
        float yScale = (float)Camera.main.pixelHeight / rect.height;
        gameObject.transform.localScale = new Vector3(xScale * gameObject.transform.localScale.x, yScale * gameObject.transform.localScale.y);
    }

    public Rect BoundsToScreenRect(Bounds bounds)
    {
        Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
        Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));
        return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
    }
}
