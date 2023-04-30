using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookControl : MonoBehaviour
{
    [SerializeField] Transform hookSource;
    [SerializeField] LayerMask hitMask;
    [SerializeField] float maxDistance;
    [SerializeField] private SpringJoint2D hookJoint;
    [SerializeField] private LineRenderer hookLine;
    [SerializeField] private float hookMoveForce;
    [SerializeField] private Rigidbody2D targetRigidbody;
    [SerializeField] Camera refCamera;
    Vector3 lastMousePoint;
    private void Start()
    {
        hookJoint.enabled = false;
        hookLine.enabled = false;
        lastMousePoint = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!hookJoint.enabled)
            {
                //Raycast
                //find? hook
                //apply force
                RaycastHit2D hit;
                hit = Physics2D.Raycast(hookSource.position, hookSource.up, maxDistance, hitMask);
                if ( hit.collider!=null)
                {
                    hookJoint.enabled = true;
                    hookLine.enabled = true;
                    hookJoint.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
                    Vector3 hitObjectPosition = hookJoint.connectedBody.transform.position;
                    hookJoint.connectedAnchor = hit.point - new Vector2(hitObjectPosition.x,hitObjectPosition.y);
                    hookJoint.distance = 0;
                    lastMousePoint = refCamera.ScreenToWorldPoint(Input.mousePosition);
                }
            }
        }
        if (hookJoint.enabled)
        {
            hookLine.SetPosition(0, hookSource.position);
            Vector2 jointPoint = hookJoint.connectedBody.position + hookJoint.connectedAnchor;
            hookLine.SetPosition(1, jointPoint);
            Vector3 mousePoint3D = refCamera.ScreenToWorldPoint(Input.mousePosition);
            if (lastMousePoint != mousePoint3D)
            {
                targetRigidbody.AddForce((lastMousePoint - mousePoint3D)*hookMoveForce*Time.deltaTime);
                lastMousePoint = mousePoint3D;
            }

            //apply change to rigidbody based on where player was last 
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (hookJoint.enabled)
            {
                hookJoint.enabled = false;
                hookLine.enabled = false;
            }
        }
    }
}
