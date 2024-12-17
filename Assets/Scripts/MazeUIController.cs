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

    void Start()
    {
        //add listener to start button
        startButton.onClick.AddListener(OnStartButtonClicked);
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


        //update width and height for the maze generator
        mazeGenerator.width = widthInput;
        mazeGenerator.height = heightInput;

        //generate a new maze
        mazeGenerator.RestartMaze();
    }
}
