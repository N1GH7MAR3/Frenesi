using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Kawsarin : MonoBehaviour
{
    public int vida = 3;
    public float fuerzaSalto = 15f;
    public float velocidad = 8f;
    public float fuerzaLanzamiento = 20f;
    public Transform puntoDeDisparo;
    public GameObject proyectilPrefab;

    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private Animator animator;

    public LayerMask suelo;

    private bool miraDerecha = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        proyectilPrefab.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        MovimientoPersonaje();
        Salto();
        Ataque();
    }

    void MovimientoPersonaje()
    {
        float inputMovimiento = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) inputMovimiento = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) inputMovimiento = 1f;
        if(inputMovimiento!=0) animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);
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
        
        RaycastHit2D raycastHit = Physics2D.BoxCast(cc.bounds.center, new Vector2(cc.bounds.size.x * 0.1f, 0.005f), 0f, Vector2.down, 3f, suelo);
        return raycastHit.collider!=null;
    }
    void Salto()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && EstaEnSuelo())
        {
            animator.SetBool("isJumping", true);
            
            rb.AddForce(Vector2.up * fuerzaSalto,ForceMode2D.Impulse);

         }
        else {animator.SetBool("isJumping", false); }
    }
    void Ataque() {
        if(Mouse.current.leftButton.wasPressedThisFrame) {
            if (!animator.GetBool("isAttacking"))
            {
                StartCoroutine(CoolDown());
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                mousePosition.z = transform.position.z;

                Vector2 direccionLanzamiento = (mousePosition - transform.position).normalized;

                if (direccionLanzamiento.x > 0 && !miraDerecha)
                {
                    GestionarOrientacion(1f);
                }
                else if (direccionLanzamiento.x < 0 && miraDerecha)
                {
                    GestionarOrientacion(-1f);
                }

                GameObject proyectil = Instantiate(proyectilPrefab, puntoDeDisparo.position, Quaternion.identity);
                proyectil.SetActive(true);
                SpriteRenderer srProyectil = proyectil.GetComponent<SpriteRenderer>();
                if (srProyectil != null)
                {
                    srProyectil.enabled = true; 
                }


                Rigidbody2D rbProyectil = proyectil.GetComponent<Rigidbody2D>();
         
                if (rbProyectil != null)
                {
                    rbProyectil.AddForce(direccionLanzamiento * fuerzaLanzamiento, ForceMode2D.Impulse);
                }
            }
        }
        
    }
    IEnumerator CoolDown() {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isAttacking", false);
    }

}
