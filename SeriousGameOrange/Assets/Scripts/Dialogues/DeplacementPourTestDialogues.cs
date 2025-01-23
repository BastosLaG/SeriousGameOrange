using UnityEngine;
using UnityEngine.InputSystem;

public class DeplacementPourTestDialogues : MonoBehaviour {
    public static DeplacementPourTestDialogues Instance { get; private set; }
    public bool canMove;
    
    public float speed = 5f;
    private InputSystemActions inputActions;
    private Vector2 movementInput;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
        inputActions = new InputSystemActions();
        canMove = true;
    }

    private void OnEnable() {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable() {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context) {
        movementInput = context.ReadValue<Vector2>();
    }

    private void Update() {
        if(canMove) {
            Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0f);
            transform.Translate(movement * speed * Time.deltaTime);
        }
    }
}
