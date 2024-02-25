using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float mouseEdgeThreshhold;

    private Vector3 _keysScrollDirection;
    private Vector3 _mouseScrollDirection;

    public void Update()
    {
        if (_keysScrollDirection != Vector3.zero)
        {
            followTarget.position += Time.deltaTime * scrollSpeed * RotateToLookDirection(_keysScrollDirection);
        }
        else if(_mouseScrollDirection != Vector3.zero)
        {
            followTarget.position += Time.deltaTime * scrollSpeed * RotateToLookDirection(_mouseScrollDirection);
        }
    }

    private Vector3 RotateToLookDirection(Vector3 scrollDirection)
    {
        return Quaternion.AngleAxis(followTarget.transform.rotation.eulerAngles.y, Vector3.up) * scrollDirection;
    }

    private void HandleMouseAt(Vector2 mousePosition)
    {
        Vector3 newScrollDirection = Vector3.zero;
        newScrollDirection.x = (mousePosition.x <= Screen.width * mouseEdgeThreshhold ? -1 : 0) +
            (mousePosition.x >= Screen.width * (1 - mouseEdgeThreshhold) ? 1 : 0);
        newScrollDirection.z = (mousePosition.y >= Screen.height * (1 - mouseEdgeThreshhold) ? 1 : 0) +
                               (mousePosition.y <= Screen.height * mouseEdgeThreshhold ? -1 : 0);
        _mouseScrollDirection = newScrollDirection.normalized;
    }

    private void RotateTargetBy(float degree)
    {
        var rotation = followTarget.transform.rotation.eulerAngles;
        rotation += new Vector3(0, degree, 0);
        followTarget.transform.rotation = Quaternion.Euler(rotation);
    }
    
    public void OnRotateLeft(InputAction.CallbackContext context)
    {
        if(context.phase != InputActionPhase.Performed) return;
        RotateTargetBy(90);
    }

    public void OnRotateRight(InputAction.CallbackContext context)
    {
        if(context.phase != InputActionPhase.Performed) return;
        RotateTargetBy(-90);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        _keysScrollDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if(context.phase != InputActionPhase.Performed) return;
        Vector2 mousePosition = context.ReadValue<Vector2>();
        HandleMouseAt(mousePosition);
    }
}
