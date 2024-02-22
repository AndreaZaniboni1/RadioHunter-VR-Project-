using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggers : MonoBehaviour
{
    public GameObject destroyText;
    public GameObject spawnText;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(destroyText);
            spawnText.SetActive(true);
            Destroy(gameObject);
        }
    }
}
