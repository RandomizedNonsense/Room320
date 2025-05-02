using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            return;
        }

        Vector3 playerPos = player.transform.position;
        playerPos.z = -10;
        transform.position = playerPos;
    }
}
