using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwanball : MonoBehaviour
{
    public GameObject prfab;
    public float speedspwan;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            GameObject spwanball = Instantiate (prfab,transform.position, Quaternion.identity); 
            Rigidbody spwanballrb = spwanball.GetComponent<Rigidbody>();
            spwanballrb.velocity= transform.forward*speedspwan;
        }
        
    }
}
