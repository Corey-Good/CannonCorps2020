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
    public  Camera    tankCamera;

    public  Transform turretObject;

    private Vector3   cursorPosition;
    private Vector3   turretFinalLookDirection;

    private float     turretLagSpeed = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxis("Mouse X"));

        Ray        screenRay = tankCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(screenRay, out hit))
        {
            cursorPosition = hit.point;
        }

        Vector3 turretLookDirection = cursorPosition - turretObject.position;
        turretLookDirection.y       = 0.0f;

        turretFinalLookDirection = Vector3.   RotateTowards(turretFinalLookDirection, turretLookDirection, turretLagSpeed * Time.deltaTime, 10.0f);
        turretObject.rotation    = Quaternion.LookRotation (turretFinalLookDirection);
    }
}
