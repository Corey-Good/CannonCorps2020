using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ColorChanger : MonoBehaviourPun
{


    private void Start()
    {

    }
    public static void ChangeColor(int code)
    {

    }

    public void ChangeColor()
    {
        tank.RPC("ChangeColor", RpcTarget.All);
    }
    [PunRPC]
    void ChangeColor_RPC(int code)
    {
        Color tankColor;
        if (code == 0)
        {
            tankColor = Color.red;
        }
        else
        {
            tankColor = Color.blue;
        }

        if (gameObject.name == "baseTank")
        {
            Renderer[] rends = GameObject.FindGameObjectWithTag("PlayerGO").GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[0].color = tankColor;
            }
        }
        if (gameObject.name == "futureTank")
        {
            Renderer[] rends = GameObject.FindGameObjectWithTag("PlayerGO").GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[1].color = tankColor;
            }
        }
        if (gameObject.name == "cartoonTank")
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
    }


    [PunRPC]
    void ChangeColor_RPC()
    {
        Color tankColor = Color.red;
        if (gameObject.name == "baseTank")
        {
            Renderer[] rends = GameObject.FindGameObjectWithTag("PlayerGO").GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[0].color = tankColor;
            }
        }
    }
}



        //if (player.teamCode == 0)
        //{
        //    tankPhotonView.RPC("ChangeColor", RpcTarget.All, new object[] { 0 });
        //}
        //else if (player.teamCode == 1)
        //{
        //    tankPhotonView.RPC("ChangeColor", RpcTarget.All, new object[] { 1 });
        //}
