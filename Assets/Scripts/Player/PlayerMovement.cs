using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : MonoBehaviour
{
    private PlayerData player_data;
    private CharacterController player;
    private Vector2 move_direction;
    private Vector2 camera_direction;
    private float sprint;
    private float jump;
    private bool is_crouching;
    private float interact;
    private float attack;
    private float _verticalVelocity;
    private GameObject player_gameobject;
    private Quaternion rotation;
    private Camera player_camera;
    private Animator animator;

    void Start()
    {
        player                     = gameObject.GetComponentInParent<CharacterController>();
        player_data                = gameObject.AddComponent<PlayerData>();
        player_data.speed          = 10f;
        player_data.health         = 100;
        player_data.damage         = 5;
        player_data.max_health     = 100;
        player_data.block_movement = false;
        player_gameobject          = gameObject.transform.parent.gameObject;
        rotation                   = player_gameobject.transform.localRotation;
        player_camera              = player_gameobject.GetComponentInChildren<Camera>();
        animator                   = player_gameobject.GetComponent<Animator>();
    }

    public void Move(InputAction.CallbackContext Action)
    {
        move_direction = Action.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext Action)
    {
        if (!player_data.block_movement)
            camera_direction = Action.ReadValue<Vector2>();
    }
    public void Sprint(InputAction.CallbackContext Action)
    {
        sprint = Action.ReadValue<float>();
    }
    public void Jump(InputAction.CallbackContext Action)
    {
        jump = Action.ReadValue<float>();
    }
    public void Attack(InputAction.CallbackContext Action)
    {
        attack = Action.ReadValue<float>();
    }
    public void Interact(InputAction.CallbackContext Action)
    {
        interact = Action.ReadValue<float>();
    }

    public void Crouch(InputAction.CallbackContext Action)
    {
        if (!player_data.block_movement)
            if (Action.started)
                is_crouching = true;
            else if (Action.canceled)
                is_crouching = false;
    }

    void Update()
    {
        // Crouch
        animator.SetBool("Crouch", is_crouching);
        animator.SetBool("Walk", move_direction != Vector2.zero);
        if (is_crouching)
        {
            player.center = Vector3.Lerp(player.center, new Vector3(0f, 0.09f, 0f), Time.deltaTime * 30f);
            player.height = Mathf.Lerp(player.height, 1f, Time.deltaTime * 10f);
            player_data.speed = 2f;
        }
        else
        { 
            player.center = Vector3.Lerp(player.center, new Vector3(0f, -0.19f, 0f), Time.deltaTime * 30f);
            player.height = Mathf.Lerp(player.height, 2.24f, Time.deltaTime * 10f);
            player_data.speed = 5f;
        }

        // Camera
        rotation.y += camera_direction.x * 3f * 5f * Time.deltaTime;
        rotation.x -= camera_direction.y * 1f * 5f * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -75f, 60f);

        if (!player_data.block_movement)
        {
            if (!float.IsNaN(transform.localRotation.z))
                player_gameobject.transform.localRotation = Quaternion.Euler(0f, rotation.y, 0f);
            if (!float.IsNaN(player_camera.transform.localRotation.z))
                player_camera.transform.localRotation = Quaternion.Euler(rotation.x, 0f, 0f);
        }

        // Jump
        if (player.isGrounded)
        {
            if (_verticalVelocity < 0)
                _verticalVelocity = -2f;

            if (jump != 0f)
                _verticalVelocity = Mathf.Sqrt(5f * 2f * Mathf.Abs(-25f));
        }
        else
        {
            _verticalVelocity += -25f * Time.deltaTime;

            if (!player.isGrounded && jump != 0f && Time.timeSinceLevelLoad > 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
                {

                    if (hit.distance > 10f && _verticalVelocity > -1f)
                        _verticalVelocity = Mathf.Min(_verticalVelocity, -20f);
                }
            }
        }

        // Movement
        Vector3 verticalMove = new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime;
        Vector3 finalMove;
        if (!player_data.block_movement)
        {
            Vector3 moveVector;
            if (sprint != 0f)
                moveVector = player_gameobject.transform.TransformDirection(new Vector3(move_direction.x, 0f, move_direction.y)) * player_data.speed * 2f * Time.deltaTime;
            else
                moveVector = player_gameobject.transform.TransformDirection(new Vector3(move_direction.x, 0f, move_direction.y)) * player_data.speed * Time.deltaTime;

            finalMove = moveVector + verticalMove;
        }
        else
        {
            finalMove = new Vector3(0f, 0f, 0f) + verticalMove;
        }


        player.Move(finalMove);
    }
}
