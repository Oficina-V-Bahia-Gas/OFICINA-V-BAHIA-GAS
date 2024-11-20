using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset = new Vector3(0, 2f, 0);

    void Update()
    {
        if (player != null)
            transform.position = player.position + offset;
    }
}