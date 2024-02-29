using FMODUnity;
using MimicSpace;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class HealthEnemy : MonoBehaviour
{
    [SerializeField] float currentHealth;
    public float maxHealth;
    public Mimic myMimic;
    public Material mimicColor;
    public CameraSeesEnemy cameraScript;
    public int myIndex;
    public NavMeshAgent myAgent;
    public GameObject particlesSystem;
    public GameObject dyingEmitter;

    void Start()
    {
        currentHealth = maxHealth;
        myMimic = GetComponent<Mimic>();
    }

    public void TakeDamage(float damageAmount)
    {
        mimicColor.color = UnityEngine.Color.red;
        StartCoroutine(RemoveColorAfterSeconds(0.3f));
        currentHealth -= damageAmount;
        
        if(currentHealth <= 0) {
            mimicColor.color = UnityEngine.Color.black;
            cameraScript.ImDead(myIndex);

            myAgent.Stop();
            particlesSystem.SetActive(true);
            dyingEmitter.SetActive(true);
            foreach (Transform child in transform)
{
                child.gameObject.layer = LayerMask.NameToLayer("Default"); 
            }
            StartCoroutine(DieAfterSeconds(3.5f));
        }
    }
        
    IEnumerator RemoveColorAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        mimicColor.color = UnityEngine.Color.black;
    }
    IEnumerator DieAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        Destroy(gameObject);
    }
}
