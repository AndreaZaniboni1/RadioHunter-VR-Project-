using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerHP : MonoBehaviour

{
    [SerializeField] float currentHealth;
    public float maxHealth;
    public GameObject tunneling;
    public GameObject youWinText;
    public GameObject diedText;
    [SerializeField] private EventReference damageSound; 
    [SerializeField] private EventReference deathSound; 
    GameObject sceneManagerGO;
    public SceneTransitionManager sceneTransitionManager;
    public int radioOff = 0;
    void Start()
    {
        currentHealth = maxHealth;
        sceneManagerGO = GameObject.FindGameObjectWithTag("SceneManager");
        sceneTransitionManager = sceneManagerGO.GetComponent<SceneTransitionManager>();


    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        tunneling.SetActive(true);

        StartCoroutine(RemoveAfterSeconds(0.7f, tunneling));

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlayOneShot(deathSound, this.transform.position);
            diedText.gameObject.SetActive(true);
            sceneTransitionManager.GoToScene(0);

            Debug.Log("Player Dead");
        }
        else
        {
            AudioManager.instance.PlayOneShot(damageSound, this.transform.position);
        }
    }


    IEnumerator RemoveAfterSeconds(float seconds, GameObject obj)
    {
        yield return new WaitForSeconds(seconds);
        if(currentHealth > 0)
        {
            obj.SetActive(false);
        }
    }

    
    public void RadioOff()
    {
        radioOff += 1;
        if (radioOff == 3)
        {
            youWinText.SetActive(true);
            sceneTransitionManager.WinGame();
        }
    }
}