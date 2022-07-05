using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 10;
    public Transform target;
    public float dstFromTarget = 2;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public Vector3 offsetPos = new Vector3(0, 0, 0);

    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    private void Awake()
    {
        GameManager.StageChanged += OnStageChanged;
    }
    
    private void OnStageChanged(GameStage stage, bool isGamePaused)
    {
        enabled = !isGamePaused;
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity,
            rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position + offsetPos - transform.forward * dstFromTarget;
    }
}