using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    public Transform personaje;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

       
        transform.position = new Vector3(
        Mathf.Clamp(personaje.position.x, 0, 500), // Limita el eje X
        Mathf.Clamp(personaje.position.y, 0, 200), // Limita el eje Y
        -10f);
        //transform.position = new Vector3(personaje.position.x, personaje.position.y, -10f);
    }
}
