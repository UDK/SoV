using UnityEngine;
using UnityEditor;
using Assets.Scripts.GlobalControllers.Control;

namespace Assets.DOTSScripts.GlobalControllers.InputControl
{
    public static class ControlManager
    {
        private static Actions PlayerInpuit = Actions.EmptyInstance;
        public static void PlayerInputUpdate()
        {
            PlayerInpuit = GetPlayerActions();
        }

        public static Actions GetCurrentActions(WhoIs whoIs)
        {
            switch (whoIs)
            {
                case WhoIs.Player:
                    return PlayerInpuit;
                default:
                    return Actions.EmptyInstance;
            }
        }

        private static Actions GetPlayerActions()
        {
            Actions actions = Actions.EmptyInstance;
            if (Input.GetKey(KeyCode.W))
            {
                actions.Move |= Move.Up;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                actions.Move |= Move.Down;
            }

            if (Input.GetKey(KeyCode.D))
            {
                actions.Move |= Move.Rigth;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                actions.Move |= Move.Left;
            }
            return actions;
        }
    }
}