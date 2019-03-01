using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    [SerializeField] GameObject target;
    [SerializeField] GameObject player;
    [SerializeField] GameObject arrow;

    void Start() {
    }


    void Update () {
        transform.position = player.transform.position + player.transform.forward * 0.7f - Vector3.up * 0.3f;
        //Vector2 vec2 = new Vector2(target.transform.position.x - player.transform.position.x, target.transform.position.z - player.transform.position.z);

        /*
        float r = Mathf.Atan2(vec2.y, vec2.x);
        float angle = Mathf.Floor(r * 360 / (2 * Mathf.PI));

        arrow.transform.rotation = Quaternion.Euler(0, 90 - angle, 0);
        */

        arrow.transform.LookAt(target.transform);
	}
}
