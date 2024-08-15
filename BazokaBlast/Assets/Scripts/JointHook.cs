using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JointHook : MonoBehaviour
{
    
    public GameObject Obj_1,Obj_2;
    private bool isJoint=false;
    public CannonController CC;
    public float CannonEndPos;
    public bool MinValue, MaxValue;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Obj_1 != null && Obj_2 != null)
        {
            isJoint = true;
        }
        else
        {
            isJoint = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            if (isJoint)
            {
                CC.MinMove = -45;
                CC.MaxMove = 45;
            }
            else
            {
                if (MinValue)
                {
                    CC.MinMove = CannonEndPos;
                }
                if (MaxValue)
                {
                    CC.MaxMove = CannonEndPos;
                }
            }
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            if (isJoint)
            {
                CC.MinMove = -45;
                CC.MaxMove = 45;
            }
            else
            {
                if (MinValue)
                {
                    CC.MinMove = CannonEndPos;
                }
                if (MaxValue)
                {
                    CC.MaxMove = CannonEndPos;
                }
            }

        }
    }
}
