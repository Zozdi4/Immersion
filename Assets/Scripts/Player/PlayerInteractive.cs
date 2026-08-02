using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractive : MonoBehaviour
{
    private bool _interact;
    private bool _attack;
    public bool Is_Attacking   { get { return _attack; } }
    public bool Is_Interacting { get { return _interact; } }
    public void Attack(InputAction.CallbackContext Action)
    {
        _attack = System.Convert.ToBoolean(Action.ReadValue<float>());
    }
    public void Interact(InputAction.CallbackContext Action)
    {
        _interact = System.Convert.ToBoolean(Action.ReadValue<float>());
    }
}
