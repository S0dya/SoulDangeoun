using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCamera : SingletonMonobehaviour<CinemachineCamera>
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer framingTransposer;

    Transform curFollow;

    protected override void Awake()
    {
        base.Awake();

        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void ChangeCameraFollow(Transform transform)
    {
        if (curFollow != transform)
        {
            curFollow = transform;
            virtualCamera.Follow = transform;
        }
    }

    public void MoveCameraCloser()
    {
        virtualCamera.m_Lens.OrthographicSize = 2;
    }

    public void MoveCameraFurther()
    {
        virtualCamera.m_Lens.OrthographicSize = 5f;
    }

    public void MoveCameraForPlayer()
    {
        MoveCameraFurther();
        framingTransposer.m_DeadZoneWidth = 0.1f;
        framingTransposer.m_DeadZoneHeight = 0.15f;
    }
}
