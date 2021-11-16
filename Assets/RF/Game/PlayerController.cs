using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    [SerializeField] private List<UnitBase> unitList = new List<UnitBase>();
    [SerializeField] private List<UnitBase> selectedUnits = new List<UnitBase>();
    [SerializeField] private List<Transform> debugCubes;
    [SerializeField] private RectTransform dragBox;
    private Vector2 startMousePos;
    private bool isDrag = false;
    public void DragSelectStart()
    {
        if (!isDrag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startMousePos = Input.mousePosition;
                dragBox.position = startMousePos;
            }
            
            if (Input.GetMouseButton(0))
            {
                dragBox.gameObject.SetActive(true);
                //dragBox.position = Input.mousePosition;

                float width = Mathf.Abs(startMousePos.x - Input.mousePosition.x);
                float height = Mathf.Abs(startMousePos.y - Input.mousePosition.y);

                float xhalf;
                float yhalf;
                
                dragBox.sizeDelta = new Vector2(width,
                    Mathf.Abs(height));

                if (startMousePos.x - Input.mousePosition.x > 0)
                {
                    xhalf = width / 2;
                }
                else
                {
                    xhalf = width / 2 * -1;
                }
                
                if (startMousePos.y - Input.mousePosition.y > 0)
                {
                    yhalf = height / 2;
                }
                else
                {
                    yhalf = height / 2 * -1;
                }
                
                dragBox.position = Input.mousePosition + new Vector3(xhalf, yhalf);
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
        Vector3 scrPos = dragBox.position + new Vector3(0, 0, camera.farClipPlane);
        Vector3 centerPos = camera.ScreenToWorldPoint(scrPos);

        float width = Mathf.Abs(startMousePos.x - Input.mousePosition.x);
        float height = Mathf.Abs(startMousePos.y - Input.mousePosition.y);

        if (Physics.Raycast(camera.transform.position, centerPos, out hit, Mathf.Infinity, worldMask))
        {
            foreach (var unit in unitList)
            {
                var unitScrPos = camera.WorldToScreenPoint(unit.transform.position);
                Debug.Log(unitScrPos.x + " >= " + -width/2);
                Debug.Log(unitScrPos.x + " <= " + width/2);
                Debug.Log(unitScrPos.y + " <= " + height/2);
                Debug.Log(unitScrPos.y + " >= " + -height/2);
                if (unitScrPos.x >= dragBox.position.x + -width / 2 && unitScrPos.y >= dragBox.position.y + -height / 2 && unitScrPos.x <= dragBox.position.x + width / 2 &&
                    unitScrPos.y <= dragBox.position.y + height / 2)
                {
                    unit.gameObject.SetActive(false);
                }
            }
            
        }
        
        startMousePos = new Vector2(0,0);
        dragBox.gameObject.SetActive(false);
        isDrag = false;
        //if(Physics.Raycast())
        Debug.Log("Drag End");
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
