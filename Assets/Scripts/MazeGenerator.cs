using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeGenerator : MonoBehaviour
{
    //define width and height grid
    public int width = 10; //for now predefined, changeable later
    public int height = 10;
    private Cell[,] grid;
    void Start()
    {
        InitialGrid();

    }

    void InitialGrid()
    {
        grid = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Cell();
            }
        }
        Debug.Log("Grid has " + width + " x " + height + " cells");
    }
}
