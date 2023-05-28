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

        // CSV 파일 로드
        TextAsset csvFile = Resources.Load<TextAsset>("foods");
        StringReader reader = new StringReader(csvFile.text);

        // CSV 파일의 내용을 리스트에 추가
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
            // 리스트에서 랜덤으로 음식 선택
            int randomIndex = Random.Range(0, foodList.Count);
            string recommendedFood = foodList[randomIndex];

            // 선택된 음식을 텍스트 UI에 표시
            displayText.text = recommendedFood;

            lastRecommendedFood = recommendedFood;

            elapsed += Time.deltaTime * animationSpeed;
            yield return null;
        }

        // 애니메이션 종료 후 최종 선택된 음식 표시
        displayText.text = lastRecommendedFood;

        // 버튼 이미지 변경
        recommendButtonImage.sprite = completedButtonImage;

        // 효과음 재생
        PlayRecommendationSound();
    }

    private void PlayRecommendationSound()
    {
        audioSource.PlayOneShot(recommendationSound);
    }
}
