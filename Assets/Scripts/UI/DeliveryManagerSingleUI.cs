using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI _recipeNameText;
    [SerializeField] private Transform _iconContainer;
    [SerializeField] private Transform _iconTemplate;

    private void Awake()
    {
        _iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeSO(SO_Recipe recipeSO)
    {
        _recipeNameText.text = recipeSO.recipeName;

        foreach(Transform child in _iconContainer)
        {
            if(child == _iconTemplate)
                continue;
            Destroy(child.gameObject);
        }

        foreach(SO_KitchenObject kitchenObjectSO in recipeSO.kitchenObjectsSOList)
        {
            Transform iconTransform = Instantiate(_iconTemplate, _iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
            
        
    }

}
