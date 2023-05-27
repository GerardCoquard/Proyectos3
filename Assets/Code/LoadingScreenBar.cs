using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private float barImageSpeed = 3f;

    
    private void Update()
    {


        barImage.fillAmount = Mathf.MoveTowards(barImage.fillAmount, Loader.GetLoadingProgress(), 0.2f * Time.deltaTime);

    }
}
