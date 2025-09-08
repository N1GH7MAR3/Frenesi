using UnityEngine;
using UnityEngine.InputSystem;

public class Kawsarin : MonoBehaviour
{
    public float fuerzaSalto = 15f;
    public float velocidad = 8f;
    private Rigidbody2D rb;
    private CapsuleCollider2D bc;
    public LayerMask suelo;
    private bool miraDerecha = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoviminetoPersonaje();
        Salto();
    }

    void MoviminetoPersonaje()
    {
        float inputMovimiento = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) inputMovimiento = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) inputMovimiento = 1f;
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
    bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, new Vector2(bc.bounds.size.x, bc.bounds.size.y), 0f, Vector2.down, 0.9f, suelo);
        return raycastHit.collider!=null;
    }
    void Salto()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && EstaEnSuelo())
        {
            rb.AddForce(Vector2.up * fuerzaSalto,ForceMode2D.Impulse);

         }
    }

}
