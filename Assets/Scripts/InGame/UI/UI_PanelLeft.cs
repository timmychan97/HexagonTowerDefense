using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PanelLeft : MonoBehaviour
{
    public GameObject unitsView;
    public GameObject buildingsView;

    void Start()
    {
        OnClickUnits();
    }

    public void OnClickUnits()
    {
        unitsView.SetActive(true);
        buildingsView.SetActive(false);
    }

    public void OnClickBuildings()
    {
        unitsView.SetActive(false);
        buildingsView.SetActive(true);
    }
}
