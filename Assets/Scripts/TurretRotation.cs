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

    private Vector3   cursorPosition;
    private Vector3   turretFinalLookDirection;

    private float     turretLagSpeed = 0.7f;
    float degrees = 0;
    float input = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        turretObject.transform.Rotate(0f, 0, Input.GetAxis("Mouse X") * 300 * Time.deltaTime);
    }

}
