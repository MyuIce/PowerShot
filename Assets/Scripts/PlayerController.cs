using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	float accel;
	// Use this for initialization
	void Start () {
		accel = 0.2f;
	}
	
	void Update () {
		GetComponent<Rigidbody> ().AddForce (transform.right * Input.GetAxisRaw ("Horizontal") * accel, ForceMode.Impulse);
	}
}
