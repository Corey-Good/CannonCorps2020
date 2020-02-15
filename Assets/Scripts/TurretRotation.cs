/************************************************************************/
/* Author:             Jaben Calas                                                  */
/* Date Created:       02/14/2020                                              */
/* Last Modified Date:                                                    */
/* Modified By:                                                         */
/************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    public  GameObject turretObject;

    // Update is called once per frame
    void Update()
    {
        turretObject.transform.Rotate(0f, 0, Input.GetAxis("Mouse X") * 300 * Time.deltaTime);
    }

}
