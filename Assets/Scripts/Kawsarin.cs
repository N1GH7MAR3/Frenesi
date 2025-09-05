using UnityEngine;
using UnityEngine.InputSystem;

public class Kawsarin : MonoBehaviour
{
    public float velocidad = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoviminetoPersonaje();
    }
    void MoviminetoPersonaje()
    {
        float inputMovimiento = 0f;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (Keyboard.current.aKey.isPressed) inputMovimiento = -1f;
        if (Keyboard.current.dKey.isPressed) inputMovimiento = 1f;

        rb.linearVelocity = new Vector2(inputMovimiento * velocidad, rb.linearVelocity.y);


    }

}
