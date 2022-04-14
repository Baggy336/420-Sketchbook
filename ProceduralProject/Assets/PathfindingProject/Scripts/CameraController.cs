using UnityEngine;

public class CameraController : MonoBehaviour
{
    // How quickly the camera moves across the screen
    public float panSpeed = 20;

    // The screen edge threshold before the camera will start to move
    public float edgePan = 20;

    // The speed at which the scrollwheel will increase or decrease the y position 
    public float scrollSpeed = 600000;

    // Minimum and maximum y position
    public float minScroll = 10;
    public float maxScroll = 140;

    // Set the size of the map in the editor
    public Vector2 mapSize;

    private void Start()
    {
        // Set this object's current position
        transform.position = transform.position;
    }

    private void Update()
    {
        MovementInput();
    }

    void MovementInput()
    {
        // Temporary position variable
        Vector3 pos;

        // This object's transform will be the temporary position 
        pos = transform.position;

        // If the player presses the up arrow key, or the mouse vertical is close enough to the top of the screen
        if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - edgePan)
        { // increase the z position by speed and time
            pos.z += panSpeed * Time.deltaTime;
        }
        // If the player presses the down arrow key, or the mouse vertical is close to the bottom of the screen
        if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= edgePan)
        { // Decrease the z position by speed and time
            pos.z -= panSpeed * Time.deltaTime;
        }
        // If the player presses the right arrow key, or the mouse horizontal is close enough to the right side of the screen
        if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - edgePan)
        { // Increase the x position by speed and time
            pos.x += panSpeed * Time.deltaTime;
        }
        // If the player presses the left arrow key, or the mouse horizontal is close enough to the left of the scree,
        if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= edgePan)
        { // Decrease the x position by speed and time
            pos.x -= panSpeed * Time.deltaTime;
        }

        // Temporary variable for the scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Decrease the y if the scroll wheel is activated, by speed and time
        pos.y -= scroll * scrollSpeed * Time.deltaTime * 5f;

        // Clamp the x and z to the map size determined in the editor
        pos.x = Mathf.Clamp(pos.x, 0, mapSize.x);
        pos.z = Mathf.Clamp(pos.z, 0, mapSize.y);

        // Clamp the zoom by min and max scroll
        pos.y = Mathf.Clamp(pos.y, minScroll, maxScroll);

        // Apply changes to the transform
        transform.position = pos;
    }
}
