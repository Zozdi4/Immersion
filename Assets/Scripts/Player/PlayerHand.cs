using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private GameObject _player_scripts;
    private InitPlayerData _player_data;
    private Transform _hand;
    void Start()
    {
        _hand = GameObject.Find("Player").transform.Find("body").Find("right_arm").Find("arm_03").Find("joint");
        _player_scripts = GameObject.Find("player_scripts");
        _player_data = _player_scripts.GetComponent<InitPlayerData>();
        _player_scripts.GetComponent<PlayerRayCast>().On_Object_Found.AddListener((Object, _) =>
        {
            Debug.Log(_player_data.Item_in_hand);
            if (Object.GetComponent<InteractiveObject>() && _player_data.Item_in_hand == null)
            {
                if (!Object.GetComponent<InteractiveObject>().Pickupable)
                    return;
                Object.transform.parent.position = _hand.position;
                Object.transform.parent.localRotation = Quaternion.Euler(-90, 90, 0);
                Object.transform.parent.SetParent(_hand);
            }
        });
    }
}
