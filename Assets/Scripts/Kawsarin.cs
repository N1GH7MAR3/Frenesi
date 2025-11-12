using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Kawsarin : MonoBehaviour
{
    public int vida = 3;
    public float fuerzaSalto = 15f;
    public float velocidad = 8f;
    public float fuerzaLanzamiento = 20f;
    public Transform puntoDeDisparo;
    public GameObject proyectilPrefab;
    public float limiteAltura = -23f;
    public Vector3 posicionRespawn;
    public float limiteDerechaInicialX=37f;
    public float limiteIzquierdaInicialX=0f;
    public float distanciaScroll=69f;

    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private Animator animator;

    public GameObject pantallaGameOver;
    public GameObject camaraSeguimiento;
    public GameObject camaraMuerte;

    public LayerMask suelo;

    private bool miraDerecha = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        proyectilPrefab.SetActive(false);
        pantallaGameOver.SetActive(false);
        posicionRespawn = new Vector3(-30f,-8f,0);
        transform.position = posicionRespawn;
        ResetearPersonaje();

    }

 
    void Update()
    {
        MovimientoPersonaje();
        Salto();
        Ataque();
        VerificarCaida();
        VerificarScrollHorizontal();
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

        if (!EstaEnSuelo() && rb.linearVelocity.y < 0)
        {
           rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (3f - 1) * Time.deltaTime;
        }
        else if( rb.linearVelocity.y >0 && !Keyboard.current.spaceKey.isPressed)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (3f - 1) * Time.deltaTime;
        }
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

    void VerificarCaida()
    {
        if (transform.position.y < limiteAltura)
        {
            ActivarGameOver();
               
        }
    }

     
    void VerificarScrollHorizontal()
    {
        if (transform.position.x > limiteDerechaInicialX)
        {
            Console.WriteLine("Scroll Derecha");
            posicionRespawn = new Vector3(posicionRespawn.x+distanciaScroll, -8f, transform.position.z);
            limiteIzquierdaInicialX += distanciaScroll;
            limiteDerechaInicialX += distanciaScroll;

        }
        
    }
    void ActivarGameOver() {
        if (pantallaGameOver != null)
        {
            pantallaGameOver.SetActive(true);
            Time.timeScale = 0f;
        }
         if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (camaraMuerte != null)
        {
            camaraSeguimiento.SetActive(false);
            camaraMuerte.SetActive(true);

        }
    }
    public void ResetearPersonaje()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.WakeUp();
        }
        camaraSeguimiento.SetActive(true);
        camaraMuerte.SetActive(false);
        transform.position = posicionRespawn;
    }


    IEnumerator CoolDown() {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isAttacking", false);
    }
    public void TakeDamage (int damage)
    {
        vida -= damage;
        if (vida <= 0)
        {
            ActivarGameOver();

        }
    }

   
}


