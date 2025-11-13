using UnityEngine;

public class Melee : Enemy
{
    public float attackRange =4f;
 


    public override void Attack()
    {
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsFollow", false);
    }

   
   
    

    public override void FollowToPlayer()
    {
        attackCooldown = 2f;
        Vector2 direction = (player.position - transform.position).normalized;
        float targetSpeedX = direction.x * speed;
        rb.linearVelocity = new Vector2(targetSpeedX, rb.linearVelocity.y);


        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;
                Attack();
            }
            
            
        }
        else
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    public override void DetectPlayer()
    {
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < radioDeteccion)
        {

            followPlayer = true;
            animator.SetBool("IsFollow", true);
        }
        else
        {
            followPlayer = false;
            
        }
    }

    public override void ReturnToInitialPosition()
    {
        
        if (transform.position != (Vector3)initialPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, speedReturn * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
        {
            transform.position = initialPosition;
            
        }
        if (transform.position == (Vector3)initialPosition)
        {
            animator.SetBool("IsFollow", false);
        }
    }


}
