using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class for check what player on look now
/// </summary>
public class PlayerRayCast : MonoBehaviour
{
    /// <summary>
    /// Return: GameObject what player was look rn, Bool - Have RigidBody/
    /// </summary>
    public UnityEvent<GameObject, bool> On_Object_Found;
    
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f))
        {
            GameObject viewedObject = hit.collider.gameObject;
            bool condition = viewedObject.GetComponent<Rigidbody>();
            On_Object_Found?.Invoke(viewedObject, condition);
        }
    }   
}
