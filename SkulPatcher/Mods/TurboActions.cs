﻿using Characters.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkulPatcher
{
    public static class TurboActions
    {
        public static IEnumerator Coroutine()
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

        private static readonly List<Action.Type> attackActions = new() { Action.Type.BasicAttack, Action.Type.JumpAttack };

        private static void SetTurboAttack() => SetActionsValue(attackActions, ModConfig.turboAttackOn ? TryStartIsPressed : TryStartWasPressed);

        private static void SetTurboDash() => SetActionValue(Action.Type.Dash, ModConfig.turboDashOn ? TryStartIsPressed : TryStartWasPressed);

        private static void SetActionValue(Action.Type actionType, int value) => SetActionsValue(new List<Action.Type> { actionType }, value);

        private static void SetActionsValue(List<Action.Type> actionTypes, int value)
        {
            if (!ModConfig.IsInGame)
                return;

            List<Action> actions = ModConfig.Level.player.actions.Where(x => actionTypes.Contains(x.type)).ToList();

            foreach (Action action in actions)
                action._inputMethod = (Action.InputMethod)value;
        }
    }
}
