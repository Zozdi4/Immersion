using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InitPlayerData _player_data;
    private CharacterController _player;
    private Vector2 _move_direction;
    private Vector2 _camera_direction;
    private bool _sprint;
    private bool _jump;
    private float _vertical_velocity;
    private GameObject _player_gameobject;
    private Quaternion _rotation;
    private Camera _player_camera;
    private Animator _animator;
    void Start()
    {
        _player_data = gameObject.GetComponent<InitPlayerData>();
        _player = gameObject.GetComponentInParent<CharacterController>();
        _player_gameobject = gameObject.transform.parent.gameObject;
        _rotation = _player_gameobject.transform.localRotation;
        _player_camera = _player_gameobject.GetComponentInChildren<Camera>();
        _animator = _player_gameobject.GetComponent<Animator>();
    }

    public void Move(InputAction.CallbackContext Action)
    {
        _move_direction = Action.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext Action)
    {
        if (!_player_data.Block_movement)
            _camera_direction = Action.ReadValue<Vector2>();
    }
    public void Sprint(InputAction.CallbackContext Action)
    {
        _sprint = System.Convert.ToBoolean(Action.ReadValue<float>());
    }
    public void Jump(InputAction.CallbackContext Action)
    {
        _jump = System.Convert.ToBoolean(Action.ReadValue<float>());
    }

    public void Crouch(InputAction.CallbackContext Action)
    {
        if (!_player_data.Block_movement)
            if (Action.started)
                _player_data.Is_Crouching = true;
            else if (Action.canceled)
                _player_data.Is_Crouching = false;
    }

    void Update()
    {
        // Crouch
        _animator.SetBool("Crouch", _player_data.Is_Crouching);
        _animator.SetBool("Walk", _move_direction != Vector2.zero);

        Vector3 _crouch_center_smooth;
        float _crouch_height_smooth;
        float _crouch_speed;
        if (_player_data.Is_Crouching)
        {
            _crouch_center_smooth = new Vector3(0f, -0.09f, 0f);
            _crouch_height_smooth = 1f;
            _crouch_speed         = 2f;
        }
        else
        {
            _crouch_center_smooth = new Vector3(0f, -0.19f, 0f);
            _crouch_height_smooth = 2.24f;
            _crouch_speed         = 5f;
            
        }
        _player.center     = Vector3.Lerp(_player.center, _crouch_center_smooth, Time.deltaTime * 30f);
        _player.height     = Mathf.Lerp(_player.height, _crouch_height_smooth, Time.deltaTime * 10f);
        _player_data.Speed = _crouch_speed;

        // Camera
        _rotation.y += _camera_direction.x * 3f * 5f * Time.deltaTime;
        _rotation.x -= _camera_direction.y * 1f * 5f * Time.deltaTime;
        _rotation.x = Mathf.Clamp(_rotation.x, -75f, 60f);

        if (!_player_data.Block_movement)
        {
            if (!float.IsNaN(transform.localRotation.z))
                _player_gameobject.transform.localRotation = Quaternion.Euler(0f, _rotation.y, 0f);
            if (!float.IsNaN(_player_camera.transform.localRotation.z))
                _player_camera.transform.localRotation = Quaternion.Euler(_rotation.x, 0f, 0f);
        }

        // Jump
        if (_player.isGrounded)
        {
            if (_vertical_velocity < 0)
                _vertical_velocity = -2f;

            if (_jump)
                _vertical_velocity = Mathf.Sqrt(5f * 2f * Mathf.Abs(-25f));
        }
        else
        {
            _vertical_velocity += -25f * Time.deltaTime;

            if (!_player.isGrounded && _jump && Time.timeSinceLevelLoad > 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
                {

                    if (hit.distance > 10f && _vertical_velocity > -1f)
                        _vertical_velocity = Mathf.Min(_vertical_velocity, -20f);
                }
            }
        }

        // Movement
        Vector3 verticalMove = new Vector3(0f, _vertical_velocity, 0f) * Time.deltaTime;
        Vector3 finalMove;
        if (!_player_data.Block_movement)
        {
            Vector3 moveVector;
            if (_sprint)
                moveVector = _player_gameobject.transform.TransformDirection(new Vector3(_move_direction.x, 0f, _move_direction.y)) * _player_data.Speed * 2f * Time.deltaTime;
            else
                moveVector = _player_gameobject.transform.TransformDirection(new Vector3(_move_direction.x, 0f, _move_direction.y)) * _player_data.Speed * Time.deltaTime;

            finalMove = moveVector + verticalMove;
        }
        else
        {
            finalMove = new Vector3(0f, 0f, 0f) + verticalMove;
        }

        _player.Move(finalMove);
    }
}
