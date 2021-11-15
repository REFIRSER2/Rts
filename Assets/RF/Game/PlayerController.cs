using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    #region 카메라
    [SerializeField] private float zoomSensivity = 10F;
    [SerializeField] private float moveSensivity = 20F;
    [SerializeField] private float borderWidth = 10F;
    [SerializeField] private float borderHeight = 10F;
    
    [SerializeField] private Camera camera;
    [SerializeField] private RectTransform mouseCursor;
    [SerializeField] private GameObject clickEffect;
    [SerializeField] private GameObject debugCube;
    
    [SerializeField] private LayerMask worldMask;

    private RaycastHit hit;

    private void Setup()
    {
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        mouseCursor.pivot = Vector2.up;
        if (mouseCursor.GetComponent<Graphic>())
        {
            mouseCursor.GetComponent<Graphic>().raycastTarget = false;
        }
    }
    

    private void Move()
    {
        
        if (Input.mousePosition.x >= (Screen.width-borderWidth))
        {
            camera.transform.localPosition += new Vector3(Time.deltaTime*moveSensivity, 0, 0);
        }
        else if (Input.mousePosition.x <= (0 + borderWidth))
        {
            camera.transform.localPosition -= new Vector3(Time.deltaTime*moveSensivity, 0, 0);
        }

        if (Input.mousePosition.y >= (Screen.height - borderHeight))
        {
            camera.transform.localPosition += new Vector3(0, 0, Time.deltaTime*moveSensivity);
        }
        else if (Input.mousePosition.y < (0+borderHeight))
        {
            camera.transform.localPosition -= new Vector3(0, 0, Time.deltaTime*moveSensivity);  
        }

        Vector2 mousePos = Input.mousePosition;
        mouseCursor.transform.position = mousePos;
        //mouseCursor.transform.position = new Vector2(Mathf.Clamp(mouseCursor.transform.position.x, 0+borderWidth, Screen.width-borderWidth),
            //Mathf.Clamp(mouseCursor.transform.position.y, 0+borderHeight, Screen.height-borderHeight) );
    }

    private void Zoom()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel") * zoomSensivity;

        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - wheel, 20, 60);
    }

    private void Click()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.farClipPlane);
            Vector3 worldPos = camera.ScreenToWorldPoint(mousePos);

            if (Physics.Raycast(camera.transform.position, worldPos, out hit, Mathf.Infinity, worldMask))
            {
                clickEffect.transform.position = hit.point;
                clickEffect.gameObject.SetActive(false);
                clickEffect.gameObject.SetActive(true);  
            }
        }
    }
    
    public Camera GetCamera()
    {
        return camera;
    }
    #endregion
 
    #region 드래그
    [SerializeField] private RectTransform dragBox;
    private bool isDrag = false;
    public void DragSelectStart()
    {
        if (!isDrag)
        {
            if (Input.GetMouseButton(0))
            {
                dragBox.gameObject.SetActive(true);
                dragBox.position = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDrag = true;
                DragSelectEnd();
            }    
        }

    }

    public void DragSelectEnd()
    {
        dragBox.gameObject.SetActive(false);

        dragBox.transform.position = 
        
        isDrag = false;
    }
    #endregion
    
    #region 유니티 기본 내장 함수
    private void Awake()
    {
        Setup();
    }

    private void Update()
    {
        Move();
        Zoom();
        Click();
        DragSelectStart();
    }
    #endregion
}
