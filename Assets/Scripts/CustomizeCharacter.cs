using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeCharacter : MonoBehaviour
{
    private Tank tankInstance;
    private int model = 0;
    // Start is called before the first frame update

    public List<GameObject> tankModels = new List<GameObject>();
    void Start()
    {
        tankInstance = GetTankInstance();
        tankModels[0].SetActive(true);
        tankModels[1].SetActive(false);
        tankModels[2].SetActive(false);
        tankModels[3].SetActive(false);
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
    }
    public Tank GetTankInstance()
    {
        return GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
    }
}
