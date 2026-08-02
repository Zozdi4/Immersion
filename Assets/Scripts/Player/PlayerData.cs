using UnityEngine;

public interface IPlayerData
{
    int Health              { get; set; }
    int Max_health          { get; set; }
    int Damage              { get; set; }
    float Speed             { get; set; }
    GameObject Item_in_hand { get; set; }
    bool Block_movement     { get; set; }
    bool Is_Crouching       { get; set; }
}
