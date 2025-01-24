using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement : MonoBehaviour {
    private float speed = 5.0f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    public bool canMove;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canMove = true;
    }

    void Update() {
        rb.linearVelocity = moveInput * speed;
    }
    
    public void Interact(bool value) {
        canMove = value;
        if(canMove == false) {
            moveInput = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
    }

    public void Move(InputAction.CallbackContext context) {
        if(!canMove) return;
        animator.SetBool("isWalking", true);

        if(context.canceled) {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }
}
