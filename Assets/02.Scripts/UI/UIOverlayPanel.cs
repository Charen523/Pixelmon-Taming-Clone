using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlayPanel : MonoBehaviour
{
    [SerializeField] ToggleGroup toggleGroup;
    [SerializeField] GameObject[] panels;

    public void OnOverlayClicked()
    {
        int i = 0; 

        foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            if (toggle.isOn)
            {
                toggle.onValueChanged.Invoke(false);

                if (panels != null) //임시: 아직 없는 판넬도 있어서
                {
                    panels[i].SetActive(false);
                }

                gameObject.SetActive(false);
            }
        }
    }
}
