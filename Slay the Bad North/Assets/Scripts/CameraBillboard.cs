/*
 * Rotates an object towards the currently active camera
 * 
 * 1. Attach CameraBillboard component to a canvas or a game object
 * 2. Specify the offset and you're done
 * 
 **/

using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    public bool BillboardX = true;
    public bool BillboardY = true;
    public bool BillboardZ = true;
    public float OffsetToCamera;
    protected Vector3 localStartPosition;

    // Use this for initialization
    void Start()
    {
        localStartPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                                                               Camera.main.transform.rotation * Vector3.up);
        if(!BillboardX || !BillboardY || !BillboardZ)
            transform.rotation = Quaternion.Euler(BillboardX ? transform.rotation.eulerAngles.x : 0f, BillboardY ? transform.rotation.eulerAngles.y : 0f, BillboardZ ? transform.rotation.eulerAngles.z : 0f);
        transform.localPosition = localStartPosition;
        transform.position = transform.position + transform.rotation * Vector3.forward * OffsetToCamera;
    }
}