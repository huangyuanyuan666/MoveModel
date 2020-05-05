using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMouseEvent : MonoBehaviour
{
    Transform m_gameManager;
    void Start()
    {
        m_gameManager = GameObject.Find("GameManager").transform;
    }

     void OnMouseEnter()
    {
        //Debug.Log("enter");
        m_gameManager.SendMessage("MoseHoverEnter",gameObject.name);
    }

     void OnMouseDown()
    {
        //Debug.Log("down");
        m_gameManager.SendMessage("MouseDown", gameObject.name);
    }

     void OnMouseExit()
    {
        //Debug.Log("exit");
        m_gameManager.SendMessage("MouseHoverExis", gameObject.name);
    }

}
