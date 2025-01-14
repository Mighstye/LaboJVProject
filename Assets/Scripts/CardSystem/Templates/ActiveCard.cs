﻿using UnityEngine;
using UnityEngine.InputSystem;
using Control.ActiveCardControl.ControlTypes;
using Logic_System;
using System.Collections;

namespace CardSystem
{
    public abstract class ActiveCard: Card
    {
        public int grazeCostSegment = 1; //DEFAULT VALUE

        public float coolDownTime = 3; //DEFAULT VALUE

        private bool coolDown = false;
        
        

        protected bool UseCard()
        {
            if (coolDown) return false;
            if (!LogicSystemAPI.instance.graze.UseGraze(grazeCostSegment)) return false;
            coolDown = true;
            StartCoroutine(CoolDown());
            return true;
        }

        private IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(coolDownTime);
            coolDown = false;
        }
    }
}