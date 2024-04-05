using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum MovementState { idle, running, jumping, falling}

    [SerializeField] private AudioSource jumpSoundEffect;
 


    [SerializeField] private float jumpStrength = 5.0F;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private bool limitJumps = false;
    private Rigidbody2D rigidbody;
    //private BoxCollider2D collider;
    private CapsuleCollider2D collider;
    private Animator anim;
    private SpriteRenderer sprite;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
//        collider = GetComponent<BoxCollider2D>();
        collider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

//        Debug.Log("Hello World");
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rigidbody.velocity = new Vector2(dirX * moveSpeed, rigidbody.velocity.y);


        if (Input.GetButtonDown("Jump") && IsGrounded())
        //if (Input.GetButtonDown("Jump"))
            {
                rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpStrength);
                if(jumpSoundEffect != null)   jumpSoundEffect.Play();
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0F)
        {
            state = MovementState.running;
            sprite.flipX = false;
            anim.SetBool("running", true);
        }
        else if (dirX < 0F)
        {
            state = MovementState.running;
            sprite.flipX = true;
            anim.SetBool("running", true);
        }
        else
        {
            state = MovementState.idle;
            anim.SetBool("running", false);
        }
        if (rigidbody.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        if(rigidbody.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        //Detects if the player's touching the ground with the bottom of it's box collider by creating a similar box offset by .1f
        //on the "jumpableGround" layer that we created in the Unity IDE
        if (limitJumps)
        {
            Debug.Log("testing to see if ground is jumpable");
            return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        }
        else return true;
    }
}
