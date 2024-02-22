using MimicSpace;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    [SerializeField] float currentHealth;
    public float maxHealth;
    public Mimic myMimic;
    public Material mimicColor;
    public CameraSeesEnemy cameraScript;
    public int myIndex;

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
            Destroy(gameObject);}
    }
        
    IEnumerator RemoveColorAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        mimicColor.color = UnityEngine.Color.black;
    }
}
