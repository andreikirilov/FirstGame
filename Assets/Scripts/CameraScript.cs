using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public Vector3 currentVelocity = Vector3.zero;

    public float smoothTime = 0.15f;

    void FixedUpdate()
    {
        if (target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(new Vector3(target.position.x + 1f, -0.75f, target.position.z));
            Vector3 delta = new Vector3(target.position.x + 1f, 0, target.position.z) - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref currentVelocity, smoothTime);
        }
    }
}