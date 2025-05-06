using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] PlayerScript player;

    [SerializeField] Image fillImage;
    float maxWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxWidth = fillImage.rectTransform.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            UpdatePlayerHealth(0.0f, 1.0f);
        } else {
            UpdatePlayerHealth(player.health, player.maxHealth);
        }
    }

    void UpdatePlayerHealth(float currentHealth, float maximumHealth) {
        fillImage.rectTransform.sizeDelta = new Vector2(maxWidth * (currentHealth / maximumHealth), 2);
    }
}
