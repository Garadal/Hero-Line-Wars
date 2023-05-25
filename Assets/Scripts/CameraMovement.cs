using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed = 5f;  // Adjust this value to control the camera movement speed.
    public float boundarySize = 20f;  // Adjust this value to set the boundary size for mouse movement.

    private void Update()
    {
        // Get the mouse position.
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        // Get the screen dimensions.
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate the movement vector.
        Vector3 movement = Vector3.zero;

        // Calculate the camera's right and forward vectors in the world space.
        Vector3 cameraRight = transform.right;
        Vector3 cameraForward = transform.forward;
        cameraRight.y = 0f;  // Ignore vertical component.
        cameraForward.y = 0f;  // Ignore vertical component.
        cameraRight.Normalize();
        cameraForward.Normalize();

        // Check if the mouse is near the left boundary.
        if (mouseX < boundarySize)
        {
            movement -= cameraRight;
        }
        // Check if the mouse is near the right boundary.
        else if (mouseX > screenWidth - boundarySize)
        {
            movement += cameraRight;
        }

        // Check if the mouse is near the bottom boundary.
        if (mouseY < boundarySize)
        {
            movement -= cameraForward;
        }
        // Check if the mouse is near the top boundary.
        else if (mouseY > screenHeight - boundarySize)
        {
            movement += cameraForward;
        }

        // Normalize the movement vector and apply speed.
        movement.Normalize();
        movement *= movementSpeed * Time.deltaTime;

        // Move the camera.
        transform.position += movement;
    }
}