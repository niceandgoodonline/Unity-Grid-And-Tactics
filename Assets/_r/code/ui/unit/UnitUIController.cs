using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIController : MonoBehaviour
{
    public GameObject  mainObject;
    public UnitMenu    unitMenu;
    public UnitPreview unitPreview;
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
            CameraService.EmitUnitPreview += UpdateUnitPreview;
            CameraService.EmitUnitSelect  += UpdateUnitSelect;
            CameraService.EmitNoUnit      += ClearUnitPreview;
            CameraService.EmitNoUnitMenu  += ClearUnitMenu;
        }
        else
        { 
            CameraService.EmitUnitPreview -= UpdateUnitPreview;
            CameraService.EmitUnitSelect  -= UpdateUnitSelect;
            CameraService.EmitNoUnit      -= ClearUnitPreview;
            CameraService.EmitNoUnitMenu  -= ClearUnitMenu;
        }
    }

    private void UpdateUnitPreview(UnitBase _unit)
    {
        unitPreview.UpdateUI(_unit);
    }

    private void UpdateUnitSelect(UnitBase _unit)
    {
        unitMenu.UpdateUI(_unit);
    }

    private void ClearUnitPreview()
    {
        unitPreview.gameObject.SetActive(false);
    }

    private void ClearUnitMenu()
    {
        unitMenu.gameObject.SetActive(false);
    }
}
