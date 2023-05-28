using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class SelectManager : MonoBehaviour
{
    public Text displayText;
    public Button recommendButton;
    public float animationDuration = 3f;
    public float animationSpeed = 1f;
    public Sprite defaultButtonImage;
    public Sprite completedButtonImage;
    public AudioClip recommendationSound;

    private List<string> foodList;
    private Coroutine animationCoroutine;
    private Image recommendButtonImage;
    private AudioSource audioSource;

    private void Start()
    {
        LoadFoodList();
        recommendButton.onClick.AddListener(StartRecommendationAnimation);

        recommendButtonImage = recommendButton.GetComponent<Image>();
        recommendButtonImage.sprite = defaultButtonImage;

        audioSource = GetComponent<AudioSource>();
    }

    private void LoadFoodList()
    {
        foodList = new List<string>();

        // CSV ���� �ε�
        TextAsset csvFile = Resources.Load<TextAsset>("foods");
        StringReader reader = new StringReader(csvFile.text);

        // CSV ������ ������ ����Ʈ�� �߰�
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] fields = line.Split(',');
            foreach (string field in fields)
            {
                foodList.Add(field);
            }
        }
    }

    public void StartRecommendationAnimation()
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(RecommendationAnimation());
    }

    private IEnumerator RecommendationAnimation()
    {
        float elapsed = 0f;
        float normalizedTime = 0f;
        string initialText = displayText.text;
        string lastRecommendedFood = "";

        while (elapsed < animationDuration)
        {
            // ����Ʈ���� �������� ���� ����
            int randomIndex = Random.Range(0, foodList.Count);
            string recommendedFood = foodList[randomIndex];

            // ���õ� ������ �ؽ�Ʈ UI�� ǥ��
            displayText.text = recommendedFood;

            lastRecommendedFood = recommendedFood;

            elapsed += Time.deltaTime * animationSpeed;
            yield return null;
        }

        // �ִϸ��̼� ���� �� ���� ���õ� ���� ǥ��
        displayText.text = lastRecommendedFood;

        // ��ư �̹��� ����
        recommendButtonImage.sprite = completedButtonImage;

        // ȿ���� ���
        PlayRecommendationSound();
    }

    private void PlayRecommendationSound()
    {
        audioSource.PlayOneShot(recommendationSound);
    }
}
