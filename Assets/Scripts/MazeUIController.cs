using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeUIController : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public InputField widthInputField;
    public InputField heightInputField;
    public Button startButton;
    public Button startInFirstPersonButton;

    void Start()
    {
        //add listener to start button
        startButton.onClick.AddListener(OnStartButtonClicked);
        startInFirstPersonButton.onClick.AddListener(OnStartFirstPersonButtonClicked);
    }

    void OnStartButtonClicked()
    {
        //Get width and height from input fields
        int widthInput = int.Parse(widthInputField.text);
        int heightInput = int.Parse(heightInputField.text);

        //validate inputs
        if (widthInput < 10 || heightInput < 10)
        {
            Debug.LogError("Width and height must be at least 10.");
            return;
        }
        if (widthInput > 250 || heightInput > 250)
        {
            Debug.LogError("Width and height must be 250 or less.");
            return;
        }

        //set first person mode to false
        mazeGenerator.firstPersonMode = false;

        //update width and height for the maze generator
        mazeGenerator.width = widthInput;
        mazeGenerator.height = heightInput;

        //generate a new maze
        mazeGenerator.RestartMaze();
    }

    void OnStartFirstPersonButtonClicked()
    {
        //same logic as above
        int widthInput = int.Parse(widthInputField.text);
        int heightInput = int.Parse(heightInputField.text);

        if (widthInput < 10 || heightInput < 10)
        {
            Debug.LogError("Width and height must be at least 10.");
            return;
        }
        if (widthInput > 250 || heightInput > 250)
        {
            Debug.LogError("Width and height must be 250 or less.");
            return;
        }

        //set first person mode to true
        mazeGenerator.firstPersonMode = true;
        mazeGenerator.width = widthInput;
        mazeGenerator.height = heightInput;
        mazeGenerator.RestartMaze();
    }
}
