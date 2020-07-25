using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ScaleToSizeCamera : MonoBehaviour
{
    void Start()
    {
        ScaleObject(Camera.main);
    }

    private void ScaleObject(Camera camera)
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        Rect rect = BoundsToScreenRect(gameObject.GetComponent<MeshRenderer>().bounds);
        float xScale = (float)Camera.main.pixelWidth / rect.width;
        float yScale = (float)Camera.main.pixelHeight / rect.height;
        float commonScale = xScale > yScale ? yScale : xScale;
        gameObject.transform.localScale = new Vector3(commonScale * gameObject.transform.localScale.x, commonScale * gameObject.transform.localScale.y, commonScale * gameObject.transform.localScale.z);
    }

    private Rect BoundsToScreenRect(Bounds bounds)
    {
        Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
        Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));
        return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
    }
}
