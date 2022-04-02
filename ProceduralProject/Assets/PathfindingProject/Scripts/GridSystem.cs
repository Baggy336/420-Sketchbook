using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// This script is used to create a grid on the map. Can be used for building placement, pathfinding, map building
/// Extend this script by referencing it in another, with the behavior that script should contain using the functions in this script.
/// </summary>
/// <typeparam name="TGridObject"></typeparam>
// Namespace would go here
public class GridSystem<TGridObject>
{
    // This integer is used for the in world text
    public const int sortingOrderDefault = 5000;

    // Use the event handler to make behaviors when a grid value has changed
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

    // This class is used to get the grid space x and z that has changed
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }

    // Used only for debugging the grid
    [SerializeField]
    private bool boolDebug = false;

    // The size of the grid on the x axis
    private int width;

    // The size of the grid on the z axis
    private int height;

    // How large each grid cell will be
    private float cellSize;

    // The origin in worldspace where the grid will start
    private Vector3 origin;

    // A x, z multidimensional array to hold all grid squares defined by the Grid Objects
    private TGridObject[,] gridSquares;

    // A multidimensional array used to display the x, z coordinates for each grid square in the scene
    private TextMesh[,] debugText;

    /// <summary>
    /// This function calls the GridSystem itself. Passes in the variables defined above
    /// Use the width on the x axis
    /// the height on the z axis
    /// the cellsize for each grid square
    /// the origin where the grid will begin
    /// A function to create the first grid square of the entire grid
    /// </summary>
    public GridSystem(int width, int height, float cellSize, Vector3 origin, Func<GridSystem<TGridObject>, int, int, TGridObject> gridObject)
    {
        // Set the width equal to the width on the x axis passed into the function
        this.width = width;

        // Set the height equal to the height on the z axis passed into the function
        this.height = height;

        // Set the size of each cell to the cellsize passed in
        this.cellSize = cellSize;

        // Set the origin to the point passed in
        this.origin = origin;

        // Instantiate the multidimensional array of TGridObjects
        gridSquares = new TGridObject[width, height];

        // Instantiate the TextMesh array for displaying coordinates
        debugText = new TextMesh[width, height];

        ///
        /// Loop through the gridSquares array by the x and z sizes
        ///
        for (int x = 0; x < gridSquares.GetLength(0); x++)
        {
            for(int z = 0; z < gridSquares.GetLength(1); z++)
            {
                // Set each square to the gridobject
                gridSquares[x, z] = gridObject(this, x, z);
            }
        }

        // If the text debug is turned on
        boolDebug = true;
        if (boolDebug)
        {
            ///
            /// Loop through each dimension of the gridsquares array
            ///
            for (int x = 0; x < gridSquares.GetLength(0); x++)
            {
                for (int z = 0; z < gridSquares.GetLength(1); z++)
                {
                    // On each square, create text in it's center to display the coordinates
                    debugText[x, z] = CreateWorldText(gridSquares[x, z].ToString(), null, GetWorldPos(x, z) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPos(x, z), GetWorldPos(x, z + 1), Color.white, 100f);                   
                    Debug.DrawLine(GetWorldPos(x, z), GetWorldPos(x + 1, z), Color.white, 100f);

                }
            }
            Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), Color.white, 100f);
        }       
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public float GetWidth()
    {
        return this.width;
    }

    public float GetHeight()
    {
        return this.height;
    }

    public Vector3 GetWorldPos(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + origin;
    }

    public void GetXZ(Vector3 worldPos, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPos - origin).x / cellSize);
        z = Mathf.FloorToInt((worldPos - origin).z / cellSize);
    }

    public void SetVal(int x, int z, TGridObject val)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            gridSquares[x, z] = val;
            debugText[x, z].text = gridSquares[x, z].ToString();
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
        }        
    }

    public void GridObjectChanged(int x, int z)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
    }

    public void SetVal(Vector3 WorldPos, TGridObject val)
    {
        int x, z;
        GetXZ(WorldPos, out x, out z);
        SetVal(x, z, val);
    }

    public TGridObject GetVal(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridSquares[x, z];
        }
        else
        {
            return default(TGridObject);
        }
    }

    /// <summary>
    /// This function gets the world position of a certain grid square
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public TGridObject GetVal(Vector3 worldPos)
    {
        int x, z;
        GetXZ(worldPos, out x, out z);
        return GetVal(x, z);
    }

    /// <summary>
    /// Creates text within the grid square in the world
    /// </summary>
    /// <param name="text"></param>
    /// <param name="parent"></param>
    /// <param name="localPosition"></param>
    /// <param name="fontSize"></param>
    /// <param name="color"></param>
    /// <param name="textAnchor"></param>
    /// <param name="textAlignment"></param>
    /// <param name="sortingOrder"></param>
    /// <returns></returns>
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 30, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition + new Vector3(0, 0, 2.5f), fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    /// <summary>
    /// Defines parameters for the text within the grid spaces
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="text"></param>
    /// <param name="localPosition"></param>
    /// <param name="fontSize"></param>
    /// <param name="color"></param>
    /// <param name="textAnchor"></param>
    /// <param name="textAlignment"></param>
    /// <param name="sortingOrder"></param>
    /// <returns></returns>
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition - new Vector3(0, localPosition.y + 2, 0);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}
