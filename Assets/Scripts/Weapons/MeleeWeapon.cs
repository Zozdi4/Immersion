using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IMeleeWeapon
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _couldown = 1f;
    private PlayerInteractive _player_interactive;
    private InitPlayerData _player_data;
    private GameObject _player;
    private Animator _player_animator;
    private float _timer = 0f;

    public int Damage { get { return _damage; } set { _damage = value; } }
    public float Couldown { get { return _couldown; } set { _couldown = value; } }
    void Start()
    {
        _player = GameObject.Find("Player");
        _player_data = _player.transform.GetComponentInChildren<InitPlayerData>();
        _player_animator = _player.GetComponent<Animator>();
        _player_interactive = _player.transform.GetComponentInChildren<PlayerInteractive>();
    }

    private bool Count_Couldown()
    {
        if (_timer == 0f)
        { 
            _timer = Time.deltaTime + _couldown;
            return true;
        }
        else
            return Time.deltaTime < _timer;
    }

    void Update()
    {
        bool condition = _player_interactive.Is_Attacking && !Count_Couldown() && !_player_data.Is_Crouching;
        _player_animator.SetBool("attack", condition);
    }
}
