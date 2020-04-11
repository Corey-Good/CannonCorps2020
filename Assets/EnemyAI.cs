using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject target;
    private float speed = 10f;
    private float stoppingDistance = 15f;
    private float retreatDistance = 10f;
    Quaternion direction;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");    
    }

    // Update is called once per frame
    void Update()
    {
        direction = Quaternion.LookRotation(target.transform.position - transform.position);

        if (Vector3.Distance(transform.position, target.transform.position) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, direction, speed * 20 * Time.deltaTime);
            Debug.Log("Follow");
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < stoppingDistance && Vector3.Distance(transform.position, target.transform.position) > retreatDistance)
        {
            // do nothing
            transform.rotation = Quaternion.RotateTowards(transform.rotation, direction, speed * 40 * Time.deltaTime);
            Debug.Log("Stay");
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < retreatDistance)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, -speed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, direction, -speed * 20 * Time.deltaTime);
            Debug.Log("Retreat");
        }
    }
}
