using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    #region Singleton class: UIManager
    public static UIManager Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    #endregion

    [Header("Level Progress UI")]
    [SerializeField] private int sceneOffset;
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text nextLevelText;
    [SerializeField] private Image progressFillImage;

    [Space]
    [SerializeField] private TMP_Text levelCompletedText;
    
    [Space]
    [SerializeField] private Image fadePanel;
        
    void Start()
    {
        FadeAtStart();
        progressFillImage.fillAmount = 0f;
        SetLevelProgressText();
    }

    void SetLevelProgressText()
    {
        int level = SceneManager.GetActiveScene().buildIndex + sceneOffset;
        currentLevelText.text = level.ToString();
        nextLevelText.text = (level + 1).ToString();
    }

    public void UpdateLevelProgress()
    {
        float value = 1f - ((float)Level.Instance.objectsInScene / Level.Instance.totalObjects);
        progressFillImage.DOFillAmount(value, 0.4f);
    }

    public void ShowLevelCompletedUI()
    {
        levelCompletedText.DOFade(1f, 6f).From(0f);
    }
    
    public void FadeAtStart()
    {
        fadePanel.DOFade(0f, 1.3f).From(1f);
    }
}
