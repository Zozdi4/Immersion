using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{
    private InitPlayerData _player_data;
    private GameObject _player_scripts;
    public UnityEvent<GameObject, GameObject> On_Use_Object;
    [SerializeField] private bool _pickupable = false;
    public bool Pickupable { get { return _pickupable; } }
    void Start()
    {
        _player_scripts = GameObject.Find("player_scripts");
        _player_scripts.GetComponent<PlayerRayCast>().On_Object_Found.AddListener((Object, _) =>
        {
            if (Object.GetComponent<InteractiveObject>() && _player_scripts.GetComponent<PlayerInteractive>().Is_Interacting) 
            {
                On_Use_Object?.Invoke(Object, _player_data.Item_in_hand);
            }
        });
    }
}
