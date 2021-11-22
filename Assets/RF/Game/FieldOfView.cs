using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class FieldOfView : MonoBehaviour
{
    #region 유니티 기본 내장 함수
    private void Start()
    {
        SetupVision();
    }

    private void Update()
    {
        for (int fx = 0; fx < visionGrid.width; fx++)
        {
            for (int fy = 0; fy < visionGrid.height; fy++)
            {
                visionGrid.grids[fx,fy] = 0;
            }
        }
        

        foreach (var unit in units)
        {
            int px, py;
            px = Mathf.RoundToInt(unit.transform.position.x);
            py = Mathf.RoundToInt(unit.transform.position.z);

            for (int x = -Mathf.RoundToInt(unit.GetRange() / 100F / 2); x <= Mathf.RoundToInt(unit.GetRange() / 100F / 2); x++)
            {
                for (int y = -Mathf.RoundToInt(unit.GetRange() / 100F / 2); y <= Mathf.RoundToInt(unit.GetRange() / 100F / 2); y++)
                {
                    //Debug.Log(py);
                    // px = 9, width = 10, height = 10
                    // x = 0, x = 1, x = 2;
                    if ((px + x >= 0 && px + x < visionGrid.width) && (py + y >= 0 && py + y < visionGrid.height))
                    {
                        visionGrid.grids[px + x, py + y] = 1;
                        //Handles.DrawWireCube(new Vector3(px + x + 0.5F, 0, py + y + 0.5F), new Vector3(1F, 1F, 1F));
                    }

                    //colors[x + y * width] = 1;
                }
                
               
            }
        }
        
        //fogTexture.SetPixels(colors);
    }

    #endregion
    
    #region FOV 적 찾기
    public VisionGrid visionGrid;
    [SerializeField] private int width, height;
    [SerializeField] private Texture2D fogTexture;

    private Color[] colors = new Color[2000];
    
    public List<UnitBase> units;
    private void SetupVision()
    {
        visionGrid = new VisionGrid();
        visionGrid.width = width;
        visionGrid.height = height;

        visionGrid.grids = new int[width, height];

        units = FindObjectOfType<PlayerController>().GetUnits();
    }
    #endregion
}

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
       FieldOfView fov = target as FieldOfView;
       
       for (int x=0;x<fov.visionGrid.width; x++)
       {
           for (int y = 0; y < fov.visionGrid.height; y++)
           {
               Handles.DrawWireCube(new Vector3(x+0.5F,0,y+0.5F), new Vector3(1F, 0.1F, 1F));
           }
       }
       
       foreach (var unit in fov.units)
       {
           int px, py;
           px = Mathf.RoundToInt(unit.transform.position.x);
           py = Mathf.RoundToInt(unit.transform.position.z);

           
           for (int x = -Mathf.RoundToInt(unit.GetRange() / 100F / 2); x <= Mathf.RoundToInt(unit.GetRange() / 100F / 2); x++)
           {
               for (int y = -Mathf.RoundToInt(unit.GetRange() / 100F / 2); y <= Mathf.RoundToInt(unit.GetRange() / 100F / 2); y++)
               {
                   //Debug.Log(py);
                   // px = 9, width = 10, height = 10
                   // x = 0, x = 1, x = 2;
                   if ((px + x >= 0 && px + x < fov.visionGrid.width) && (py + y >= 0 && py + y < fov.visionGrid.height))
                   {
                       //Handles.DrawWireCube(new Vector3(px + x + 0.5F, 0, py + y + 0.5F), new Vector3(1F, 1F, 1F));
                   }
               }
           }
       }
       
    }
}

public struct VisionGrid
{
    public int width, height;
    public int players;
    public float range;
    public int[,] grids;
}
