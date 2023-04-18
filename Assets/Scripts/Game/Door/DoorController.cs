using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{

    public bool isOpen = false;
    private Ray ray;
    private RaycastHit hit;
    private float distance = 5.0f;
    public bool isDoorMoving = false;

    public AudioSource audioSource;
    public GameObject door;

    private void Update()
    {
        if (Input.GetKeyDown("e") && !isDoorMoving)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, distance))
            {
                Debug.Log(hit.collider.gameObject.name);
                Debug.Log(hit.collider.gameObject == door);
                if (hit.collider.gameObject.name.Equals(this.name) || hit.collider.gameObject == door)
                {
                    if (!isOpen)
                    {
                        audioSource.Stop();
                        door.transform.Rotate(0.0f, 90.0f, 0.0f);
                        isOpen = true;
                        Debug.Log("rotated");
                        audioSource.time = 0f;
                        StartCoroutine(DelayTime(3f));
                    }
                    else
                    {
                        audioSource.Stop();
                        door.transform.Rotate(0.0f, -90.0f, 0.0f);
                        isOpen = false;
                        Debug.Log("rotated");
                        audioSource.time = 5f;
                        audioSource.Play();
                    }
                }
            }
        }
    }

    IEnumerator DelayTime(float seconds)
    {
        audioSource.Play();
        yield return new WaitForSeconds(seconds);
        audioSource.Stop();
    }
}