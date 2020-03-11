/************************************************************************/
/* Author:             CannonCorps                                      */
/* Date Created:       1/27/2020                                        */
/* Last Modified Date: 2/26/2020                                        */
/* Modified By:        M. Agamalian                                     */
/************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeCharacter : MonoBehaviour
{
    private Tank tankInstance;
    private int model = 0;

    public Slider R;
    public Slider G;
    public Slider B;

    float r_value = 1.0f;
    float g_value = 1.0f;
    float b_value = 1.0f;
    Color tankColor;
    public Color[] defaultColors;

    public List<GameObject> tankModels = new List<GameObject>();

    // Set the base tank as the starting tank
    void Start()
    {
        tankInstance = GetTankInstance();
        SetDefaultModel();
        UpdateDefault();

    }

    // Scroll to the left within the tank menu
    public void OnClickLeft() 
    {
        if(model - 1 > -1)
        {
            tankModels[model--].SetActive(false);
            tankModels[model].SetActive(true);
        }
        else
        {
            tankModels[model].SetActive(false);
            model = 3;
            tankModels[model].SetActive(true);
        }
        UpdateDefault();
        tankInstance.tankColor = tankColor;
    }

    // Scroll to the right within the tank menu
    public void OnClickRight()
    {
        if (model + 1 < tankModels.Count)
        {
            tankModels[model++].SetActive(false);
            tankModels[model].SetActive(true);
        }
        else
        {
            tankModels[model].SetActive(false);
            model = 0;
            tankModels[model].SetActive(true);
        }
        UpdateDefault();
    }

    // Set the selected tank as the player's tank
    public void OnClickClose()
    {
        tankInstance.CreateTank(tankModels[model].name);
        tankInstance.tankColor = tankColor;
    }
    
    public Tank GetTankInstance()
    {
        return GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
    }

    public void OnValueChanged()
    {
        r_value = R.value;
        g_value = G.value;
        b_value = B.value;

        tankColor = new Color(r_value, g_value, b_value);
        ChangeTankColor(tankColor);
    }

    void ChangeTankColor(Color tankColor)
    {        
        if (tankModels[model].name == "baseTank")
        {
            Renderer[] rends = tankModels[model].GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[0].color = tankColor;
            }
        }
        if (tankModels[model].name == "futureTank")
        {
            Renderer[] rends = tankModels[model].GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[1].color = tankColor;
            }
        }
        if (tankModels[model].name == "cartoonTank")
        {
            Renderer[] rends = tankModels[model].GetComponentsInChildren<Renderer>();

            foreach (Renderer rend in rends)
            {
                if (rend.name != "Tread")
                {
                    Material[] materials = rend.materials;
                    foreach (Material material in materials)
                    {
                        material.color = tankColor;
                    }
                }
            }
        }
        if (tankModels[model].name == "catapult")
        {
            Renderer[] rends = tankModels[model].GetComponentsInChildren<Renderer>();

            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[1].color = tankColor;
            }
        }
    }

    void UpdateDefault ()
    {
        R.value = defaultColors[model].r;
        G.value = defaultColors[model].g;
        B.value = defaultColors[model].b;
        g_value = G.value;
        b_value = B.value;
        r_value = R.value;
    }

    void SetDefaultModel()
    {
        tankModels[0].SetActive(false);
        tankModels[1].SetActive(false);
        tankModels[2].SetActive(false);
        tankModels[3].SetActive(false);

        if (tankInstance.tankModel == "baseTank")
        {
            tankModels[0].SetActive(true);
            model = 0;
        }
        else if (tankInstance.tankModel == "cartoonTank")
        {
            tankModels[1].SetActive(true);
            model = 1;
        }
        else if (tankInstance.tankModel == "catapult")
        {
            tankModels[2].SetActive(true);
            model = 2;
        }
        else if (tankInstance.tankModel == "futureTank")
        {
            tankModels[3].SetActive(true);
            model = 3;
        }
        else
        {
            tankModels[0].SetActive(true);
            model = 0;
        }
    }
}
