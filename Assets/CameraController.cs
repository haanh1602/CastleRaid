using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject checkPoint;
    private Vector3 offset;
    private Vector3 rotation;
    public GameObject trigger;
    public float start = 0.0f;
    public float end = 200.0f;

    private void Awake()
    {
        offset = GetComponent<Transform>().position;
        rotation = GetInspectorRotation();
    }

    // Start is called before the first frame update
    void Start()
    {
        checkPoint.transform.position = StandardPoint(transform.position);
        Debug.Log(checkPoint.transform.position);

    }

    public Vector3 StandardPoint(Vector3 vector)
    {
        return new Vector3(vector.x + Mathf.Tan(rotation.y / 360 * (2 * Mathf.PI)) * offset.z, vector.y + Mathf.Tan(rotation.x / 360 * (2 * Mathf.PI)) * offset.z);
    }

    public Vector3 GetInspectorRotation()
    {
        float x, y, z;
        x = UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).x;
        y = UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).y;
        z = UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).z;
        return new Vector3(x, y, z);
    }

    public void InitTriggers()
    {

    }
}
