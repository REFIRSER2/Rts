using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region 플레이어
    private Player player = new Player();

    private void InitPlayer()
    {
        //player.SetTeam();
    }
    
    public Player GetPlayer()
    {
        return player;
    }
    #endregion
    
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

    public Camera GetCamera()
    {
        return camera;
    }
    #endregion
 
    #region 드래그
    public class Box
    {
        public Vector3 min, max;

        public Vector3 center
        {
            get
            {
                var c = min + (max - min) * 0.5F;
                c.y = (max - min).magnitude * 0.5F;
                return c;
            }
        }

        public Vector3 size
        {
            get
            {
                return new Vector3(Mathf.Abs(max.x - min.x), (max - min).magnitude, Mathf.Abs(max.z - min.z));
            }
        }

        public Vector3 extents
        {
            get
            {
                return size * 0.5F;
            }
        }
    }
    
    [SerializeField] private List<UnitBase> my_Units = new List<UnitBase>();
    [SerializeField] private List<UnitBase> selected_Units = new List<UnitBase>();

    [SerializeField] private RectTransform dragBox;

    [SerializeField] private List<Transform> debugCubes;

    [SerializeField] private LayerMask unitMask;
    
    private Box box = new Box();
    private Vector3 startPos = new Vector3(0,0,0);

    private bool isDrag = false;
    public void DragSelectStart()
    {
        if (!isDrag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
                dragBox.position = startPos;

                selected_Units.Clear();
                
                Vector3 p1 = camera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,camera.farClipPlane));
                if (Physics.Raycast(camera.transform.position, p1, out hit, Mathf.Infinity, worldMask))
                {
                    //debugCubes[0].position = hit.point;
                    box.min = hit.point;
                }
			}

            if (Input.GetMouseButton(0))
            {
                dragBox.gameObject.SetActive(true);

                float width = Input.mousePosition.x - startPos.x;
                float height = Input.mousePosition.y - startPos.y;
                dragBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
                dragBox.anchoredPosition = (Vector2)startPos + new Vector2(width/2, height/2);

                Vector3 p1 = camera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,camera.farClipPlane));
                if (Physics.Raycast(camera.transform.position, p1, out hit, Mathf.Infinity, worldMask))
                {
                    //debugCubes[1].position = hit.point;
                    box.max = hit.point;
                }

            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDrag = true;
                DragSelectEnd();
            }
        }

    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void DragSelectEnd()
    {
        Collider[] colliders = Physics.OverlapBox(box.center, box.extents, Quaternion.identity, unitMask);
        foreach (var col in colliders)
        {
            UnitBase unit = col.GetComponent<UnitBase>();
            if (unit.GetTeam() != player.GetTeam())
            {
                continue;
            }
            
            unit.Select();
            selected_Units.Add(unit);
        }

        dragBox.gameObject.SetActive(false);
        isDrag = false;
        //if(Physics.Raycast())
        Debug.Log("Drag End");
    }

    public List<UnitBase> GetUnits()
    {
        return my_Units;
    }
    #endregion
    
    #region 유닛 컨트롤
    
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

                UnitBase target = hit.transform.GetComponent<UnitBase>();
                if (target == null)
                {
                    if (selected_Units.Count > 0)
                    {
                        foreach (var unit in selected_Units)
                        {
                            unit.MovePos(hit.point);
                        }
                    }    
                }
                else
                {
                    if (selected_Units.Count > 0)
                    {
                        if (target.GetTeam() == player.GetTeam())
                        {
                            foreach (var unit in selected_Units)
                            {
                                unit.MovePos(hit.point);
                            }   
                        }
                        else
                        {
                            foreach (var unit in selected_Units)
                            {
                                unit.MoveTarget(target);
                            }      
                        }
                    }     
                }
            }
        }
    }
    #endregion
    
    #region 유니티 기본 내장 함수
    private void Awake()
    {
        Setup();
        InitPlayer();
    }

    private void Update()
    {
        Move();
        Zoom();
        Click();
        DragSelectStart();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(box.center, box.size);
    }
    #endregion
}
