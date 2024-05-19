using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour {

    // Create a Damage Popup
    private static int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    [SerializeField]
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    public void Setup(int damageAmount, bool isCriticalHit) {
        textMesh.SetText(damageAmount.ToString());

        Color color;
        if (!isCriticalHit) {
            // Normal hit
            textMesh.fontSize = 1;
            ColorUtility.TryParseHtmlString("#FFC500", out color);
            textColor = color;
        } else {
            // Critical hit
            textMesh.fontSize = 3;
            ColorUtility.TryParseHtmlString("#FF2B00", out color);
            textColor = color;
        }
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(.7f, 2);
    }

    private void Update() {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 1f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f) {
            // First half of the popup lifetime
            float increaseScaleAmount = 3f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        } else {
            // Second half of the popup lifetime
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0) {
            // Start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }
    }

}
