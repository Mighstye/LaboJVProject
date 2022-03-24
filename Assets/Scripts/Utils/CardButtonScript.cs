using Game_Manager;
using PlayerInfosAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardButtonScript : MonoBehaviour
{
    public bool toggled;
    public GameObject associatedGameObject;
    public Color32 activatedColor;
    public Color32 deactivatedColor;
    private TextMeshProUGUI buttonText;
    private Image image;

    public void Start()
    {
        buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        image = gameObject.GetComponent<Image>();
        if (PlayerInfos.instance.SelectedActiveCard.Contains(associatedGameObject)) toggled = true;
        else if (PlayerInfos.instance.SelectedPassiveCard.Contains(associatedGameObject)) toggled = true;
        else toggled = false;
        if (toggled) image.color = activatedColor;
        else image.color = deactivatedColor;
        buttonText.text = associatedGameObject.name;
    }

    public void OnClick()
    {
        if (!toggled)
        {
            image.color = activatedColor;
            GameManagerAPI.instance.selectCard(associatedGameObject);
        }
        else
        {
            image.color = deactivatedColor;
            GameManagerAPI.instance.unSelectCard(associatedGameObject);
        }

        toggled = !toggled;
    }
}