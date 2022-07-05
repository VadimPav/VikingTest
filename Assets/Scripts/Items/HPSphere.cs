using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSphere : MonoBehaviour
{
    public delegate void HPSphereHandler(HPSphere hpSphere);
    public event HPSphereHandler sphereDeactivate;

    [SerializeField] private float existenceTime = 15f;
    [SerializeField] private float healAmount = 1f;

    private void Awake()
    {
        GameManager.StageChanged += GameManager_StageChanged;
    }

    private void GameManager_StageChanged(GameStage changedStage, bool isGamePaused)
    {
        if(!isGamePaused || !gameObject.activeSelf)
        {
            return;
        }
        sphereDeactivate(this);
    }

    private void OnEnable()
    {
        StartCoroutine(WaitAndDestroy());
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(existenceTime);
        enabled = false;
        gameObject.SetActive(false);
        sphereDeactivate(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        other.GetComponent<PlayerControl>().Heal(healAmount);
        enabled = false;
        gameObject.SetActive(false);
        sphereDeactivate(this);
    }
}
