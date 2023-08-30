using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCamera : SingletonMonobehaviour<CinemachineCamera>
{
    public CinemachineVirtualCamera virtualCamera;
    Transform curFollow;

    protected override void Awake()
    {
        base.Awake();

    }

    public void ChangeCameraFollow(Transform transform)
    {
        if (curFollow != transform)
        {
            curFollow = transform;
            virtualCamera.Follow = transform;
        }
    }
}
