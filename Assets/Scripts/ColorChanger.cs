/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       2/26/2020                                        */
/* Last Modified Date: 2/28/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/
using UnityEngine;
using Photon.Pun;

public class ColorChanger : MonoBehaviourPun
{
    [PunRPC]
    void ChangeColor_RPC(int teamCode, string tankName)
    {
        Color tankColor;
        if (teamCode == 0)
        { 
             tankColor = Color.red;
        }
        else
        {
            tankColor = Color.blue;
        }

        if (tankName == "baseTank")
        {
            Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[0].color = tankColor;
            }
        }
        if (tankName == "futureTank")
        {
            Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[1].color = tankColor;
            }
        }
        if (tankName == "cartoonTank")
        {
            Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();

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
        if (tankName == "Catapult")
        {
            Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[1].color = tankColor;
            }
        }
    }

    [PunRPC]
    void ChangeColor_RPC()
    {
        Color tankColor = Color.red;

        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in rends)
        {
            Material[] materials = rend.materials;
            materials[0].color = tankColor;
        }
        
    }

    [PunRPC]
    void ChangeColor_RPC(string tankName, float r, float g, float b)
    {
        Color tankColor = new Color(r, g, b, 1);

        if (tankName == "baseTank")
        {
            Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[0].color = tankColor;
            }
        }
        if (tankName == "futureTank")
        {
            Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[1].color = tankColor;
            }
        }
        if (tankName == "cartoonTank")
        {
            Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
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
        if (tankName == "Catapult")
        {
            Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[1].color = tankColor;
            }
        }
    }
}