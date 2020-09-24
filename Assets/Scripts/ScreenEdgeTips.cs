using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScreenEdgeTips : MonoBehaviour
{

    //UI
    public Text distanceLabel;
    //父对象的rect
    public RectTransform directContainer;
    //本图片标签的宽度
    public float prefabWidth;
    //物体A
    public GameObject fromHero;
    //物体B
    public GameObject toHero;

    //cameras
    private Camera mainCamera;
    public Camera uiCamera;

    //param
    private List<Line2D> screenLines;

    private RectTransform rectTransform;

    private void LateUpdate()
    {
        UpdateRotation();
        UpdatePosition();
    }

    private void Start()
    {
        rectTransform = transform as RectTransform;

        float offsetWidth = prefabWidth * 0.5f; //偏差     
        Vector2 point1 = new Vector2(-directContainer.sizeDelta.x * 0.5f + offsetWidth, directContainer.sizeDelta.y * 0.5f - offsetWidth);
        Vector2 point2 = new Vector2(directContainer.sizeDelta.x * 0.5f - offsetWidth, directContainer.sizeDelta.y * 0.5f - offsetWidth);
        Vector2 point3 = new Vector2(-directContainer.sizeDelta.x * 0.5f + offsetWidth, -directContainer.sizeDelta.y * 0.5f + offsetWidth);
        Vector2 point4 = new Vector2(directContainer.sizeDelta.x * 0.5f - offsetWidth, -directContainer.sizeDelta.y * 0.5f + offsetWidth);

        screenLines = new List<Line2D>();
        screenLines.Add(new Line2D(point1, point2));
        screenLines.Add(new Line2D(point1, point3));
        screenLines.Add(new Line2D(point2, point4));
        screenLines.Add(new Line2D(point3, point4));
        mainCamera = Camera.main;
    }

    //点是否在屏幕内
    private bool IsInScreen(Vector2 pos)
    {
        if (pos.x <= screenLines[1].point1.x
            || pos.x >= screenLines[0].point2.x
            || pos.y <= screenLines[0].point1.y
            || pos.y >= screenLines[3].point2.y)
        {
            return true;
        }
        return false;
    }

    //世界坐标转换为屏幕坐标
    private Vector2 WorldToScreenPoint(Vector3 pos)
    {
        if (null != mainCamera)
        {
            return mainCamera.WorldToScreenPoint(pos);
        }
        return Vector2.zero;
    }


    /// <summary>
    /// 世界坐标转ugui坐标
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    private Vector2 WorldToUGUIPosition(Vector3 worldPos)
    {
        Vector2 world2ScreenPos = mainCamera.WorldToScreenPoint(worldPos);
        Vector2 uiPos = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(directContainer, world2ScreenPos, uiCamera, out uiPos);
        return uiPos;
    }

    private bool IsInMainCameraView(Transform tran)
    {
        Transform camTransform = mainCamera.transform;
        Vector2 viewPos = mainCamera.WorldToViewportPoint(tran.position);
        Vector3 dir = (tran.position - camTransform.position).normalized;
        float dot = Vector3.Dot(camTransform.forward, dir);//判断物体是否在相机前面

        if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            return true;
        else
            return false;
    }

    private void UpdateRotation()
    {
        Vector3 playerForward = new Vector3(fromHero.transform.forward.x, 0, fromHero.transform.forward.z);
        Vector3 dir = new Vector3(toHero.transform.position.x, 0, toHero.transform.position.z) - new Vector3(fromHero.transform.position.x, 0, fromHero.transform.position.z);
        float angle = Vector3.Angle(playerForward, dir);
        angle *= -Mathf.Sign(Vector3.Cross(playerForward, dir).y);
        transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private void UpdatePosition()
    {
        if (IsInMainCameraView(toHero.transform) && false)
        {
            Vector2 toPos = WorldToUGUIPosition(toHero.transform.position);
            rectTransform.anchoredPosition = toPos;//ui的绝对布局
        }
        else
        {
            Vector2 intersecPos = new Vector2();
            Vector2 fromPos = WorldToUGUIPosition(fromHero.transform.position);
            Line2D line = new Line2D(fromPos, transform.up * 5000);
            foreach (Line2D l in screenLines)
            {
                if (line.Intersection(l, out intersecPos) == Line2D.CROSS)
                {
                    break;
                }
            }
            rectTransform.anchoredPosition = intersecPos;//ui的绝对布局
        }
        if (distanceLabel != null)
        {
            distanceLabel.text = String.Format("{0}米", Mathf.Round((toHero.transform.position - fromHero.transform.position).magnitude));
        }
    }
}
