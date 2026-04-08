using UnityEngine;
using UnityEngine.UI;

public class WordSplitUI : MonoBehaviour
{
    public WordSplitProgression wordSplitProgression;
    public LivesWordSplit livesWordSplit;
    
    public GameObject slicedText;
    public Image KnifeIcon1;
    public Image KnifeIcon2;
    public Image KnifeIcon3;

    public Sprite brokenKnife;

    string slicedAmount;
    int knifeAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        knifeAmount = livesWordSplit.lives;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSlicedCounter()
    {
        slicedAmount = wordSplitProgression.score.ToString();
        slicedText.GetComponent<TMPro.TextMeshProUGUI>().text = slicedAmount + " / 10";
    }

    public void BreakKnives()
    {
        knifeAmount = livesWordSplit.lives;
        switch (knifeAmount)
        {
            case 3:
                KnifeIcon1.sprite = brokenKnife;
                break;
            case 2:
                KnifeIcon2.sprite = brokenKnife;
                break;
            case 1:
                KnifeIcon3.sprite = brokenKnife;
                break;
        }
    }
}
