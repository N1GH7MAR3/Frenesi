using UnityEngine;
using UnityEngine.InputSystem;

public class Kawsarin : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private bool miraDerecha = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoviminetoPersonaje();
    }
    void MoviminetoPersonaje()
    {
        float inputMovimiento = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) inputMovimiento = -1f;
        if (Keyboard.current.dKey.isPressed|| Keyboard.current.rightArrowKey.isPressed) inputMovimiento = 1f;
        rb.linearVelocity = new Vector2(inputMovimiento * velocidad, rb.linearVelocity.y);
        GestionarOrientacion(inputMovimiento);  
    }
    void GestionarOrientacion(float inputMovimiento)
    {
        if (miraDerecha == true && inputMovimiento < 0 || miraDerecha == false && inputMovimiento > 0)
        {
            miraDerecha = !miraDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

        }
    }

}
