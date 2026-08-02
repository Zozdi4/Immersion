using UnityEngine;

public class InitPlayerData : MonoBehaviour, IPlayerData
{
    private int _health;
    private int _max_health;
    private float _speed;
    private GameObject _item_in_hand;
    private int _damage;
    private bool _block_movement;
    private bool _is_crouching;
    public int Health              { get { return _health; } set { _health = value; } }
    public int Max_health          { get { return _max_health; } set { _max_health = value; } }
    public float Speed             { get { return _speed; } set { _speed = value; } }
    public GameObject Item_in_hand { get { return _item_in_hand; } set { _item_in_hand = value; } }
    public int Damage              { get { return _damage; } set { _damage = value; } }
    public bool Block_movement     { get { return _block_movement; } set { _block_movement = value; } }
    public bool Is_Crouching       { get { return _is_crouching; } set { _is_crouching = value; } }
}
