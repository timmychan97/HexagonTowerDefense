using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PanelLeft : MonoBehaviour
{
    public GameObject unitsView;
    public GameObject buildingsView;
    // Start is called before the first frame update
    void Start()
    {
        OnClickUnits();
    }

    // Update is called once per frame
    void Update()
    {
        
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
