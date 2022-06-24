using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform cameraToLookAt;
    public Transform screenOrientation;
    // Start is called before the first frame update
    void Start()
    {
        cameraToLookAt = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(screenOrientation.rotation);
        Vector3 v = screenOrientation.position;
        Vector3 r = screenOrientation.localRotation.eulerAngles;
        Debug.Log(r + "  ,  " + v);

        Vector3 newRotation = new Vector3(7.98f, 180, 0);
        transform.eulerAngles = r;
    }

    void RotateToCam()
    {
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(cameraToLookAt.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}
