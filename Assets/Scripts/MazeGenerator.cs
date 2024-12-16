using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeGenerator : MonoBehaviour
{
    //define width and height grid
    public int width = 10; //for now predefined, changeable later
    public int height = 10;

    public bool firstPersonMode = false;

    private GameObject mazeParent;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject playerPrefab;
    public GameObject playerFirstPersonPrefab;
    public Camera mainCamera;
    public Camera firstPersonCamera;
    private Cell[,] grid;

    private Vector2Int startPoint;
    private Vector2Int endPoint;

    void Start()
    {
        InitialGrid();
        GenerateMaze();
        DrawMaze();
        AdjustCamera();
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
    }

    void GenerateMaze()
    {
        //Depth-first search algorithm
        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        startPoint = new Vector2Int(0, 0); //start at top left
        endPoint = new Vector2Int(width - 1, height - 1); //end at the bottom right

        Vector2Int current = startPoint;
        grid[current.x, current.y].visited = true;

        do
        {
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(current);

            if (neighbors.Count > 0)
            {
                Vector2Int chosen = neighbors[Random.Range(0, neighbors.Count)];
                RemoveWallBetween(current, chosen); //remove the wall between the cells
                stack.Push(current);
                current = chosen;
                grid[current.x, current.y].visited = true;//set the current cell to visited
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
            }
            else //a is above b
            {
                grid[a.x, a.y].topWall = false;
                grid[b.x, b.y].bottomWall = false;
            }
        }
        else if (a.y == b.y) //horizontally aligned
        {
            if (a.x > b.x) //a is to the right of b
            {
                grid[a.x, a.y].leftWall = false;
                grid[b.x, b.y].rightWall = false;
            }
            else //a is to the right of b
            {
                grid[a.x, a.y].rightWall = false;
                grid[b.x, b.y].leftWall = false;
            }
        }
    }

    void DrawMaze()
    {
        //destroy any existing parent object
        if (mazeParent != null)
        {
            Destroy(mazeParent);
        }

        //destroy player
        GameObject playerInstance = null;

        mazeParent = new GameObject("Maze");

        //grid[startPoint.x, startPoint.y].leftWall = false;  // Open the left wall at the start
        grid[endPoint.x, endPoint.y].rightWall = false;    // Open the right wall at the end


        //check if the walls should be placed based on true or false bools
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, 0, y);
                GameObject floor = Instantiate(floorPrefab, pos, Quaternion.identity);
                floor.transform.SetParent(mazeParent.transform);

                if (grid[x, y].topWall == true)
                {
                    GameObject wall = Instantiate(wallPrefab, pos + Vector3.forward * 0.5f + Vector3.up * 0.5f, Quaternion.identity);
                    wall.transform.SetParent(mazeParent.transform);
                }
                if (grid[x, y].bottomWall == true)
                {
                    GameObject wall = Instantiate(wallPrefab, pos - Vector3.forward * 0.5f + Vector3.up * 0.5f, Quaternion.identity);
                    wall.transform.SetParent(mazeParent.transform);
                }
                if (grid[x, y].leftWall == true)
                {
                    GameObject wall = Instantiate(wallPrefab, pos - Vector3.right * 0.5f + Vector3.up * 0.5f, Quaternion.Euler(0, 90, 0));
                    wall.transform.SetParent(mazeParent.transform);
                }
                if (grid[x, y].rightWall == true)
                {
                    GameObject wall = Instantiate(wallPrefab, pos + Vector3.right * 0.5f + Vector3.up * 0.5f, Quaternion.Euler(0, 90, 0));
                    wall.transform.SetParent(mazeParent.transform);
                }

                //place player in the top at the starting position
                if (x == 0 && y == 0 && playerPrefab != null && playerFirstPersonPrefab != null)
                {
                    //check what player mode we are in.
                    if (firstPersonMode == true)
                    {
                        playerInstance = Instantiate(playerFirstPersonPrefab, pos + Vector3.up * 1f, Quaternion.identity);
                        playerInstance.transform.SetParent(mazeParent.transform);
                    }
                    else
                    {
                        playerInstance = Instantiate(playerPrefab, pos + Vector3.up * 1f, Quaternion.identity);
                        playerInstance.transform.SetParent(mazeParent.transform);
                    }

                }
            }
        }

        if (playerInstance == null)
        {
            Debug.LogError("Player prefab not assigned or failed to spawn.");
        }
    }

    void AdjustCamera()
    {
        if (mainCamera != null)
        {
            float mazeWidth = width;
            float mazeHeight = height;

            //position camera at center of maze
            mainCamera.transform.position = new Vector3(mazeWidth / 2f, Mathf.Max(mazeWidth, mazeHeight), mazeHeight / 2f);

            //adjust camera ortographic size to fit maze
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = Mathf.Max(mazeWidth, mazeHeight) / 2 + 1;//added padding 
        }
    }

    public void RestartMaze()
    {
        if (mazeParent != null)
        {
            Destroy(mazeParent);
        }
        InitialGrid();
        GenerateMaze();
        DrawMaze();
        AdjustCamera();
    }
}
