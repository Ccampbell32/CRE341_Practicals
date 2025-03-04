using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedBreakpoint : MonoBehaviour
{

    public delegate void EventHandler();
    public  event EventHandler AddSpeed;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            
               //playerController.IncreaseSpeed();
                
                if(AddSpeed != null)
                {
                    AddSpeed();
                }
                    Destroy(this.gameObject);
            
        }
    }
}
