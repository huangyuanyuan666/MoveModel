using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Transform Cube;
    public Button Btn;
    // Start is called before the first frame update
    void Start()
    {
        //Btn.onClick.AddListener(()=> PrintVector());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PrintVector();
        }
    }

    void PrintVector()
    {
        //Debug.Log("本地方向=="+Cube.forward);
        //Debug.Log("世界方向=="+Cube.TransformDirection(Vector3.forward));
        // Debug.Log("003=="+Cube.forward);
        //屏幕坐标
        Vector3 mouse = Input.mousePosition;
        Vector3 localMouse = Camera.main.ScreenToWorldPoint(mouse);
        Vector3 worldMouse = Camera.main.transform.TransformPoint(localMouse);
        Debug.Log("屏幕坐标" + mouse);
        Debug.Log("本地空间坐标：" + localMouse);
        //Debug.Log("世界本地坐标：" + worldMouse);
    }
}
