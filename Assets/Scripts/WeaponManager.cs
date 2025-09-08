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
    }

    private void OnDisable()
    {
        fire.Disable();
    }

    private void Update()
    {
        if (fire.IsPressed())
        {
            weapon.Fire();
        }
    }
}
