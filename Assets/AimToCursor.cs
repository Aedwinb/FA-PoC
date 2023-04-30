 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimToCursor : MonoBehaviour
{
    [SerializeField] private Camera _sourceCamera;
    [SerializeField] private Transform _targetObject;

    // Update is called once per frame
    void Update()
    {
        Vector3 worldCursor = _sourceCamera.ScreenToWorldPoint(Input.mousePosition);
        _targetObject.up = new Vector3(worldCursor.x,worldCursor.y,_targetObject.position.z) - _targetObject.position;
    }
}
