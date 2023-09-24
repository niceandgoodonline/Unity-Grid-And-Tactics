using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitMenu : MonoBehaviour
{
    public UnitBase unit;

    public void UpdateUI(UnitBase _unit)
    {
        unit = _unit;
        gameObject.SetActive(true);
    }

    public void MoveUnit()
    {
        unit.MovePreview();
    }

    public void OpenActions()
    {
        Debug.Log("TODO: OpenActions()");
    }

    public void CloseActions()
    {
        Debug.Log("TODO: CloseActions()");
    }

    public void Inspect()
    {
        Debug.Log("TODO: Inspect()");
    }

    public void PassTurn()
    {
        Debug.Log("TODO: PassTurn()");
        gameObject.SetActive(false);
    }
}
