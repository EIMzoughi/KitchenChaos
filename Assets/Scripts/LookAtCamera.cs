using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LookAtCamera : MonoBehaviour
{

    enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraBackward,
    }

    [SerializeField] private Mode mode;

    private void LateUpdate()
    {
        switch(mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFormCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFormCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraBackward:
                transform.forward = - Camera.main.transform.forward;
                break;
        }
        
    }
}
