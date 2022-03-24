﻿using System;
using System.Collections.Generic;
using System.Linq;
using CardSystem;
using DG.Tweening;
using Game_Manager;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Utils
{
    public class CardDetailManager : Singleton<CardDetailManager>
    {
        private const string ActionMap = "CardDropSelection";
       
        [SerializeField] private CardDetailUI cardDetailUI;
        [SerializeField] private CircularCardListBank cardListBank;
        [SerializeField] private GameObject menu;
        [SerializeField] private CircularCardListBank listBank;

        private UIManager uiManager;

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
        }

        public void Start()
        {
            gameObject.SetActive(true);
            Init(new List<Card>(
                GameManagerAPI.instance.rewards.Select(o => o.GetComponent<Card>())));
            uiManager = menu.GetComponent<UIManager>();
        }

       

        private void Init(List<Card> cards)
        {
            ControlManager.instance.SwitchMap(ActionMap);
            cardListBank.Init(cards);
        }

        private Card RetrieveSelectedCard()
        {
            return cardListBank.selectedCard;
        }

        public void PassSelectedCard(InputAction.CallbackContext context)
        {
            if (context.phase is not InputActionPhase.Performed) return;
            GameManagerAPI.instance.selectCard(RetrieveSelectedCard().gameObject);
            uiManager.Continue();
            ControlManager.instance.SwitchToPlayer();
            gameObject.SetActive(false);
        }

        public void MoveSelection(InputAction.CallbackContext context)
        {
            listBank.MoveItem(context);
        }
    }
}