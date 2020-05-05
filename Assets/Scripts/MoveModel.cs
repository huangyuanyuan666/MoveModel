using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通过拖动小坐标轴 移动模型
/// </summary>
public class MoveModel : MonoBehaviour
{
    #region 常量
    //移动速度
    const float MOVE_SPEED = 0.0001F;
    #endregion

    #region 字段
    public Transform m_camera;
    //模型
    Transform m_model;
    //坐标轴
    Transform m_axis;
    //坐标轴颜色 分别对应x、y、z、选中轴
    Color[] m_axisColors = new Color[] {Color.red,Color.green,Color.blue,Color.yellow };
    //是否正在移动物体
    bool m_isMoveModel = false;
    //上一帧鼠标位置
    Vector3 m_lastMousePos;
    //当前选中坐标轴
    AxisState m_axisState = AxisState.Idle;
    //坐标轴的三个轴
    Transform[] m_xyz = new Transform[3];
    #endregion

    #region unity回调
    void Start()
    {
        Initialized();
    }

    void Update()
    {
         if (Input.GetMouseButton(0) && m_isMoveModel)
        {
            MovingModel();
        }
        //移动完成
        else if (Input.GetMouseButtonUp(0) && m_isMoveModel)
        {
          
            MoveComplete();
        }

    }
    #endregion

    #region 方法
    //初始化
    void Initialized()
    {
        m_model = GameObject.Find("Player").transform;
        m_axis = GameObject.Find("Axis").transform;

        for (int i = 0; i < m_axis.childCount; i++)
        {
            m_xyz[i] = m_axis.GetChild(i);
        }
        //坐标轴颜色初始化
        m_xyz[0].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[0]);
        m_xyz[1].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[1]);
        m_xyz[2].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[2]);
    }
    //移动
    void MovingModel()
    {
        //鼠标在屏幕上的方向
        Vector3 mouseDir = Input.mousePosition - m_lastMousePos;
        Vector3 start = Camera.main.ScreenToWorldPoint(m_lastMousePos);
        Vector3 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 startWorld = Camera.main.transform.TransformPoint(start);
        Vector3 endWorld = Camera.main.transform.TransformPoint(end);
        Vector3 mouseWorldDir = endWorld - startWorld;
        Vector3 offset = Vector3.zero;
        Vector3 axisStart = Camera.main.WorldToScreenPoint(m_axis.position);
        switch (m_axisState)
        {
            case AxisState.X:
                Transform x = m_axis.Find("X");
                Vector3 screen = Camera.main.WorldToScreenPoint(x.forward+axisStart)-axisStart;
                float similar=  Vector3.Dot(mouseWorldDir, x.forward);
                offset = new Vector3(similar * Time.deltaTime * MOVE_SPEED, 0, 0);
             
                break;
            case AxisState.Y:
                Transform y = m_axis.Find("Y");
                 screen = Camera.main.WorldToScreenPoint(y.forward+axisStart)-axisStart;
                similar = Vector3.Dot(mouseWorldDir, y.forward);
                offset = new Vector3(0,similar * Time.deltaTime * MOVE_SPEED, 0);
                break;
            case AxisState.Z:
                Transform z = m_axis.Find("Z");
                 screen= Camera.main.WorldToScreenPoint(z.forward+axisStart)-axisStart;
                
                similar = Vector3.Dot(mouseWorldDir, z.forward);
               // Debug.Log("mouseWorld==" + mouseWorld + "z forward==" + z.forward);
                offset = new Vector3(0, 0,-similar * Time.deltaTime * MOVE_SPEED);
                break;
            default:break;
        }
        m_model.position += offset;
        m_axis.position += offset;

        m_lastMousePos = Input.mousePosition;
    }

    //完成本次移动
    void MoveComplete()
    {
        m_isMoveModel = false;

        switch (m_axisState)
        {
            case AxisState.X:
                m_xyz[0].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[0]);
                break;
            case AxisState.Y:
                m_xyz[1].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[1]);
                break;
            case AxisState.Z:
                m_xyz[2].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[2]);
                break;
        }
    }

    #endregion

    #region 事件回调
    //鼠标悬浮坐标轴 黄色材质
    void MoseHoverEnter(string axisName)
    {
        if (!m_isMoveModel)
        {
            switch (axisName)
            {
                case "X":
                    m_xyz[0].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[3]);
                    break;
                case "Y":
                    m_xyz[1].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[3]);
                    break;
                case "Z":
                    m_xyz[2].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[3]);
                    break;
            }
        }
    }

    void MouseHoverExis(string axisName)
    {
        if (!m_isMoveModel)
        {
            switch (axisName)
            {
                case "X":
                    m_xyz[0].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[0]);
                    break;
                case "Y":
                    m_xyz[1].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[1]);
                    break;
                case "Z":
                    m_xyz[2].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[2]);
                    break;
            }
        }
    }

    //选中坐标轴
    void MouseDown(string axisName)
    {
        m_isMoveModel = true;
        m_lastMousePos = Input.mousePosition;
        switch (axisName)
        {
            case "X":
                m_axisState = AxisState.X;
                break;
            case "Y":
                m_axisState = AxisState.Y;
                break;
            case "Z":
                m_axisState = AxisState.Z;
                break;
            default:
                m_axisState = AxisState.Idle;
                break;
        }
    }
    #endregion
    

    enum AxisState
    {
        X,
        Y,
        Z,
        Idle
    }
}

//将单个轴forward转世界方向=》转屏幕方向 TODO 失败
//将屏幕点转空间点=》求得空间向量=》直接与forward求相似