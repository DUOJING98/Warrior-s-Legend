using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputSystem_Actions action;
    public Vector2 inputDirection;
    private void Awake()
    {
        action = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void Update()
    {
        inputDirection =action.Player.Move.ReadValue<Vector2>();
    }
}
