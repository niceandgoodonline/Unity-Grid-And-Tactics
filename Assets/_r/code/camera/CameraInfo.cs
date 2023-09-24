using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraInfo : MonoBehaviour
{
    public Camera                   main;
    public CinemachineVirtualCamera vCam;

    public void __init()
    {
        
    }

    private void OnEnable()
    {
        __init();
        SubscribeToEvents(true);
    }

    private void OnDisable()
    {
        SubscribeToEvents(false);
    }

    private void SubscribeToEvents(bool state)
    {
        if(state)
        {
            TurnInfo.EmitUnitTurn += HandleUnitTurn;
        }
        else
        {
            TurnInfo.EmitUnitTurn -= HandleUnitTurn;
        }
    }

    private void HandleUnitTurn(UnitTurn _unit)
    {
        vCam.m_Follow = _unit.transform;
        vCam.m_LookAt = _unit.transform;
    }
    
    //void Start() {}
    //void Update() {}
}
