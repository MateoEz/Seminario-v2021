using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationInMenu : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 3f;
    public float transitionSpeed = 5f;
    public float returnSpeed = 5f;

    private Vector3 offsetFromTarget;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float yaw = 0f;
    private float pitch = 0f;

    private bool isRotating = false;
    private bool returningToOriginal = false;
    private bool transitioningToOrbit = false;

    private Vector3 orbitTargetPosition;
    private Quaternion orbitTargetRotation;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("No target assigned to CameraOrbit script.");
            enabled = false;
            return;
        }

        offsetFromTarget = transform.position - target.position;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            yaw = 0f;
            pitch = 0f;
            isRotating = false;
            returningToOriginal = false;
            transitioningToOrbit = true;

            offsetFromTarget = transform.position - target.position;
        }

        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 rotatedOffset = rotation * offsetFromTarget;
            orbitTargetPosition = target.position + rotatedOffset;
            orbitTargetPosition.y = Mathf.Clamp(orbitTargetPosition.y, 4.5f, 15f);
            orbitTargetRotation = Quaternion.LookRotation(target.position - orbitTargetPosition);

            if (transitioningToOrbit)
            {
                transform.position = Vector3.Lerp(transform.position, orbitTargetPosition, Time.deltaTime * transitionSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, orbitTargetRotation, Time.deltaTime * transitionSpeed);

                if (Vector3.Distance(transform.position, orbitTargetPosition) < 0.01f &&
                    Quaternion.Angle(transform.rotation, orbitTargetRotation) < 0.1f)
                {
                    transitioningToOrbit = false;
                    isRotating = true;
                }
            }
            else if (isRotating)
            {
                transform.position = orbitTargetPosition;
                transform.rotation = orbitTargetRotation;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
            transitioningToOrbit = false;
            returningToOriginal = true;
        }

        if (returningToOriginal)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * returnSpeed);

            if (Vector3.Distance(transform.position, originalPosition) < 0.01f &&
                Quaternion.Angle(transform.rotation, originalRotation) < 0.1f)
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                returningToOriginal = false;
            }
        }
    }
}

