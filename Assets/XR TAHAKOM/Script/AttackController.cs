using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{

    public float movementSpeed;
    
    public int damage; 
    public bool isAOE; 
    public float aoeRadius = 2f; 
    public float slowingImpact; 


    void Update()
    {
       
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (isAOE)
            {
               
                Vector3 impactPosition = other.ClosestPointOnBounds(transform.position);

               
                ApplyAOEEffect(impactPosition, aoeRadius);
            }
            else
            {
              
                ApplySingleTargetEffect(other.gameObject);
            }

           
           
            gameObject.SetActive(false);

          
            ObjectpoolVR.Instance.ReturnObjectToPool(gameObject.tag, gameObject);

        }
        else if (other.gameObject.tag == "Boundary")
        {
          

            gameObject.SetActive(false);

           
            ObjectpoolVR.Instance.ReturnObjectToPool(gameObject.tag, gameObject);
        }
    }

    private void ApplyAOEEffect(Vector3 impactPosition, float aoeRadius)
    {
      
        Collider[] colliders = Physics.OverlapSphere(impactPosition, aoeRadius);


        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                ApplySingleTargetEffect(collider.gameObject);
            }
        }

       
    }

    private void ApplySingleTargetEffect(GameObject enemy)
    {
        EnemyControllerVR enemyController = enemy.GetComponent<EnemyControllerVR>();
        if (enemyController != null)
        {
           
            enemyController.UpdateHealth(damage);

          
            if (slowingImpact > 0)
            {
                enemyController.SlowDown(slowingImpact, 2f);
            }
        }
    }

}