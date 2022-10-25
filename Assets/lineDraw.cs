using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class lineDraw : MonoBehaviour
{

    public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;

    // list to keep track of the position of the mouse/finger of the user
    public List<Vector2> fingerPositions;

    //holds all the lines to make implementing delete and undo easier
    private List<GameObject> lines;

    //color of the line
    public Color color;

    //button to take screenshot
    public Button screenshotButton;

    //these actions are subscribed by the makeinvisible script class to remove the canvas elements during screenshot
    public event Action onScreenShot;
    public event Action offScreenShot;


    // Start is called before the first frame update
    void Start()
    {
        lines = new List<GameObject>();
        color = new Color(0.0f, 0.0f, 0.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartLine();
        }   
        if (Input.GetMouseButton(0))
        {
            Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(tempPos, fingerPositions[fingerPositions.Count-1]) > .1f)
            {
                UpdateLine(tempPos);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(lines.Count);
        }
    }

    //when the screen is touched, a new line should start to be drawn
    void StartLine()
    {
        //instantiating a new line prefab and saving to the currentLine variable
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

        //adding the line GameObject to the list lines
        lines.Add(currentLine);

        lineRenderer = currentLine.GetComponent<LineRenderer>();

        fingerPositions.Clear();

        //current finger position, added to the list
        //if only touched, the start and end position should be the same position
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        lineRenderer.SetColors(color, color);

        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);

    }

    //function to update the line acording to the movement of the mouse/finger
    //essentially keeps on adding points to the current line according to the mouse position and draws a line there
    void UpdateLine(Vector2 newPos)
    {
        fingerPositions.Add(newPos);
        lineRenderer.positionCount++;
        //new position is added to the end of the line renderer
        lineRenderer.SetPosition(lineRenderer.positionCount-1, newPos);
    }

    //functions to change the color of the line
    public void BlueMarker()
    {
        //removing the point that appears on the back of the button when clicked
        Destroy(currentLine);
        color = new Color(0.16f, 0.651f, 0.8f, 1f);
    }

    public void GreenMarker()
    {
        Destroy(currentLine);
        color = new Color(0.192f, 0.8f, 0.16f, 1f);
    }

    public void RedMarker()
    {
        Destroy(currentLine);
        color = new Color(0.8f, 0.16f, 0.19f, 1f);
    }

    public void Eraser()
    {
        Destroy(currentLine);
        color = new Color(1f, 1f, 1f, 1f);
    }

    public void ScreenShot()
    {
        Destroy(currentLine);
        onScreenShot();
        ScreenCapture.CaptureScreenshot("ScreenCapture " + System.DateTime.Now.ToString("MM-dd-yy, HH-mm-ss") + ".png");
        Debug.Log("Taken Screenshot");
        offScreenShot();
    }

    public void UndoLine()
    {
        //check if list is empty
        if (lines.Count > 1)
        {
            //need to remove the end of the list twice because we need to remove the point that appears on the back
            //when undo button is clicked as well
            Destroy(lines[lines.Count - 1]);
            lines.RemoveAt(lines.Count - 1);
            Destroy(lines[lines.Count - 1]);
            lines.RemoveAt(lines.Count - 1);
        }
        else
        {
            Destroy(lines[lines.Count - 1]);
            lines.RemoveAt(lines.Count - 1);
        }
    }

    public void DeleteAll()
    {
        //remove everything from the list
        foreach (var cloneObj in lines)
            Destroy(cloneObj);
        lines.Clear();
    }
}
