using TMPro;
using UnityEngine;

public class UnitPreview : MonoBehaviour
{
    public TMP_Text unitName;
    public TMP_Text unitClass;
    public TMP_Text currentEnergy;
    public TMP_Text maxEnergy;
    
    public void UpdateUI(UnitBase _unit)
    {
        unitName.text      = _unit.persistantData.unitName;
        unitClass.text     = _unit.persistantData.unitClass;
        currentEnergy.text = _unit.persistantData.currentEnergy;
        maxEnergy.text     = $"/{_unit.persistantData.maxEnergy}";
        gameObject.SetActive(true);
    }
}
