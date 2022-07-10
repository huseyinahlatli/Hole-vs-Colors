using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    #region Singleton class: Level

    public static Level Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    #endregion

    [SerializeField] private ParticleSystem winFx;

    [Space]
    [HideInInspector] public int objectsInScene;
    [HideInInspector] public int totalObjects;

    [SerializeField] private Transform objectsParent;
    
    void Start()
    {
        CountObjects();
    }

    void CountObjects()
    {
        totalObjects = objectsParent.childCount;
        objectsInScene = totalObjects;
    }

    public void PlayWinFx()
    {
        winFx.Play();
    }
    public void LoadNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex < 2)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
