using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 10)
        {
            transform.localEulerAngles = new Vector3(0, -90, 0);
        }
        if (transform.position.x < -10)
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        transform.Translate(transform.forward * Time.deltaTime * 5, Space.World);
    }
}
