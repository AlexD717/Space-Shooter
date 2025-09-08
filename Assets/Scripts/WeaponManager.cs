using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private Weapon weapon;

    private InputAction fire;

    private void OnEnable()
    {
        InputActionMap playerControls = inputActions.FindActionMap("Player");
        fire = playerControls.FindAction("Fire");
        fire.Enable();
        fire.performed += OnFire;
    }

    private void OnDisable()
    {
        fire.performed -= OnFire;
        fire.Disable();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        weapon.Fire();
    }
}
