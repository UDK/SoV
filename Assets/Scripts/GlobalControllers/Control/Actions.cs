using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.GlobalControllers.Control
{
    public enum Move
    {
        Nothing = 0b0000,
        Up = 0b0001,
        Down = 0b0010,
        Rigth = 0b0100,
        Left = 0b1000
    }

    public class Actions
    {
        public Move Move { get; set; } = Move.Nothing;

        public static Actions EmptyInstance =>
            new Actions();
    }
}