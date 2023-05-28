using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    
    private void Update()
    {
        barImage.fillAmount = Loader.instance.GetLoadingProgress();
    }
}
