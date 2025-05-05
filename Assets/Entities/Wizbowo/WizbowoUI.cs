using UnityEngine;
using UnityEngine.UI;

public class WizbowoUI : MonoBehaviour
{
    [SerializeField] WizbowoBrain wizbowo;

    [SerializeField] Image fillImage;
    float maxWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        maxWidth = fillImage.rectTransform.sizeDelta.x;
    }

    // Update is called once per frame
    void Update() {
        if (wizbowo == null) {
            UpdateWizbowoHealth(0.0f, 1.0f);
        } else {
            UpdateWizbowoHealth(wizbowo.health, wizbowo.maxHealth);
        }
    }

    void UpdateWizbowoHealth(float currentHealth, float maximumHealth) {
        fillImage.rectTransform.sizeDelta = new Vector2(maxWidth * (currentHealth / maximumHealth), 2);
    }
}
