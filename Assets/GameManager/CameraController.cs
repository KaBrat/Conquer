using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float cameraMoveSpeed = 5f;

    private Renderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GameObject.FindObjectsOfType<ProvincesMap>().FirstOrDefault().GetComponent<Renderer>();
    }

    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        // Move the camera based on key input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 newPosition = this.transform.position + new Vector3(horizontalInput, verticalInput, 0) * cameraMoveSpeed * Time.deltaTime;

        // Get the size of the sprite (image)
        float imageSizeX = _spriteRenderer.bounds.size.x / 2f;
        float imageSizeY = _spriteRenderer.bounds.size.y / 2f;

        // Calculate the boundaries of the image
        float maxX = imageSizeX - this.GetComponent<Camera>().orthographicSize * this.GetComponent<Camera>().aspect;
        float minX = this.GetComponent<Camera>().orthographicSize * this.GetComponent<Camera>().aspect - imageSizeX;
        float maxY = imageSizeY - this.GetComponent<Camera>().orthographicSize;
        float minY = this.GetComponent<Camera>().orthographicSize - imageSizeY;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        this. transform.position = newPosition;
    }
}