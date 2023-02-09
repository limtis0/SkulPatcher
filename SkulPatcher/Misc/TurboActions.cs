﻿using Characters.Actions;
using HarmonyLib;
using Services;
using Singletons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkulPatcher.Misc
{
    public static class TurboActions
    {
        public static void Init()
        {
            Main.menu.GetComponent<Menu>().StartCoroutine(TurboCoroutine());
        }

        private static IEnumerator TurboCoroutine()
        {
            while (true)
            {
                SetTurboAttack();
                SetTurboDash();

                yield return new WaitForSeconds(1f);
            }
        }

        // Enum values
        private const int TryStartIsPressed = 0;
        private const int TryStartWasPressed = 1;

        private static readonly List<Action.Type> attackActions = new List<Action.Type>() { Action.Type.BasicAttack, Action.Type.JumpAttack };

        private static void SetTurboAttack() => SetActionsValue(attackActions, Config.turboAttackOn ? TryStartIsPressed : TryStartWasPressed);

        private static void SetTurboDash() => SetActionValue(Action.Type.Dash, Config.turboDashOn ? TryStartIsPressed : TryStartWasPressed);

        private static void SetActionValue(Action.Type actionType, int value) => SetActionsValue(new List<Action.Type> { actionType }, value);

        private static void SetActionsValue(List<Action.Type> actionTypes, int value)
        {
            List<Action> actions = Singleton<Service>.Instance.levelManager.player.actions.Where(x => actionTypes.Contains(x.type)).ToList();

            foreach (Action action in actions)
                Traverse.Create(action).Field("_inputMethod").SetValue(value);
        }
    }
}
