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
    // Start is called before the first frame update

    public List<GameObject> tankModels = new List<GameObject>();
    void Start()
    {
        tankInstance = GetTankInstance();
        tankModels[0].SetActive(true);
        tankModels[1].SetActive(false);
        tankModels[2].SetActive(false);
        tankModels[3].SetActive(false);

        R.value = r_value;
        G.value = g_value;
        B.value = b_value;        
    }

    // Update is called once per frame
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
    }

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
    }

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
    }
}
