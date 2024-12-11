using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeGenerator : MonoBehaviour
{
    //define width and height grid
    public int width = 10; //for now predefined, changeable later
    public int height = 10;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    private Cell[,] grid;
    void Start()
    {
        InitialGrid();
        GenerateMaze();
        PrintGrid();
        DrawMaze();


    }

    void InitialGrid()
    {
        //create an initial grid with cells based on the width and heigth
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

    void GenerateMaze()
    {
        //Depth-first search algorithm
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int current = new Vector2Int(0, 0); //start at top left
        grid[current.x, current.y].visited = true;


        do
        {
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(current);

            if (neighbors.Count > 0)
            {
                Vector2Int chosen = neighbors[Random.Range(0, neighbors.Count)];
                RemoveWallBetween(current, chosen);
                stack.Push(current);
                current = chosen;
                grid[current.x, current.y].visited = true;
            }
            else if (stack.Count > 0)
            {
                current = stack.Pop();
            }
        }
        while (stack.Count > 0);

    }

    List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        if (cell.x > 0 && !grid[cell.x - 1, cell.y].visited)
        {
            neighbors.Add(new Vector2Int(cell.x - 1, cell.y));
        }
        if (cell.x < width - 1 && !grid[cell.x + 1, cell.y].visited)
        {
            neighbors.Add(new Vector2Int(cell.x + 1, cell.y));
        }
        if (cell.y > 0 && !grid[cell.x, cell.y - 1].visited)
        {
            neighbors.Add(new Vector2Int(cell.x, cell.y - 1));
        }
        if (cell.y < height - 1 && !grid[cell.x, cell.y + 1].visited)
        {
            neighbors.Add(new Vector2Int(cell.x, cell.y + 1));
        }
        return neighbors;
    }

    void RemoveWallBetween(Vector2Int a, Vector2Int b)
    {
        if (a.x == b.x) //vertically aligned
        {
            if (a.y > b.y) //a is below b
            {
                grid[a.x, a.y].bottomWall = false;
                grid[b.x, b.y].topWall = false;
                Debug.Log("Wall removed");
            }
            else //a is above b
            {
                grid[a.x, a.y].topWall = false;
                grid[b.x, b.y].bottomWall = false;
                Debug.Log("Wall removed");
            }
        }
        else if (a.y == b.y) //horizontally aligned
        {
            if (a.x > b.x) //a is to the right of b
            {
                grid[a.x, a.y].leftWall = false;
                grid[b.x, b.y].rightWall = false;
                Debug.Log("Wall removed");
            }
            else //a is to the right of b
            {
                grid[a.x, a.y].rightWall = false;
                grid[b.x, b.y].leftWall = false;
                Debug.Log("Wall removed");
            }
        }
    }

    void DrawMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, 0, y);
                Instantiate(floorPrefab, pos, Quaternion.identity);

                if (grid[x, y].topWall == true)
                {
                    Instantiate(wallPrefab, pos + Vector3.forward * 0.5f, Quaternion.identity);
                }
                if (grid[x, y].bottomWall == true)
                {
                    Instantiate(wallPrefab, pos - Vector3.forward * 0.5f, Quaternion.identity);
                }
                if (grid[x, y].leftWall == true)
                {
                    Instantiate(wallPrefab, pos - Vector3.right * 0.5f, Quaternion.Euler(0, 90, 0));
                }
                if (grid[x, y].rightWall == true)
                {
                    Instantiate(wallPrefab, pos + Vector3.right * 0.5f, Quaternion.Euler(0, 90, 0));
                }

            }
        }
    }

    void PrintGrid()
    {
        for (int y = 0; y < height; y++)
        {
            string row = "";
            for (int x = 0; x < width; x++)
            {
                row += grid[x, y].visited ? "V " : ". ";
            }
            Debug.Log(row);
        }
    }
}
