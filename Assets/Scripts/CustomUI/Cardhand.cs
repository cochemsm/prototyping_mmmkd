using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using UnityEngine.UIElements;

public class Cardhand : VisualElement {
    public new class UxmlFactory : UxmlFactory<Cardhand> {}

    public List<Card> cards = new List<Card>();
    private VisualTreeAsset cardTemplate;
    
    public Cardhand() {
        cardTemplate = Resources.Load<VisualTreeAsset>("");
    }

    private void SetCardsToPoints() {
        
    }
}