using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public Transform Player;
    // Start is called before the first frame update
    private void LateUpdate()
    {
        Vector3 newPosition = Player.position;
        newPosition.y = Player.transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, Player.eulerAngles.y, 0f);

    }
}
