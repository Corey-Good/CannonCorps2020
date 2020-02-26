/************************************************************************/
/* Author:             CannonCorps                                      */
/* Date Created:       1/27/2020                                        */
/* Last Modified Date: 2/26/2020                                        */
/* Modified By:        M. Agamalian                                     */
/************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeCharacter : MonoBehaviour
{
    private Tank tankInstance;
    private int  model = 0;

    public List<GameObject> tankModels = new List<GameObject>();

    // Set the base tank as the starting tank
    void Start()
    {
        tankInstance = GetTankInstance();
        tankModels[0].SetActive(true);
        tankModels[1].SetActive(false);
        tankModels[2].SetActive(false);
        tankModels[3].SetActive(false);
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
    }

    // Set the selected tank as the player's tank
    public void OnClickClose()
    {
        tankInstance.CreateTank(tankModels[model].name);
    }
    
    public Tank GetTankInstance()
    {
        return GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
    }
}
