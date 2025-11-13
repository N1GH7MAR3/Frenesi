using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Enemy: MonoBehaviour
{
    public int health=5;
    public float speed =3f;
    public Transform player;
    protected Rigidbody2D rb;
    public bool followPlayer = false;
    public float radioDeteccion;
    public float speedReturn =1.5f;
    public Vector2 initialPosition;
    public bool isFacingRight = true;
    public bool isPlayerRight;
    public float attackCooldown ;
    public float nextAttackTime ;

    public Animator animator;
    void Start()
    {
        initialPosition = transform.position;
        radioDeteccion = 10f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsAttacking", false);
    }
    void Update()
    {
        DetectPlayer();
        if (followPlayer)
        {
            Vector2 directionToPlayer = player.position - transform.position;
            isPlayerRight = directionToPlayer.x > 0;
            GestionarOrientacion(isPlayerRight);
            FollowToPlayer();  
        }
        else
        {
            float targetXToLookAway= transform.position.x-(player.position.x - transform.position.x);
            bool isLookingAwayRight = targetXToLookAway > transform.position.x;
            GestionarOrientacion(isLookingAwayRight);
            ReturnToInitialPosition();
            
        }
    }

    public abstract void Attack();
    public abstract void FollowToPlayer();
    public abstract void DetectPlayer();
    public abstract void ReturnToInitialPosition();

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        
    }

    private void Die()
    {
        Destroy(gameObject);
    }


    private void GestionarOrientacion(bool isPlayerRight)
    {
        if ((isFacingRight && !isPlayerRight ) || (!isFacingRight && isPlayerRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 escala = transform.localScale;
            escala.x *= -1;
            transform.localScale = escala;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Kawsarin playerScript = collision.gameObject.GetComponent<Kawsarin>();
            playerScript.TakeDamage(1);
        }
    }
}
