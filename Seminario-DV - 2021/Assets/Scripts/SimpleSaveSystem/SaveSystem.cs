using System;
using Player;
using UnityEngine;

namespace SimpleSaveSystem
{
    public class SaveSystem : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("roomOne", 0);
            PlayerPrefs.SetInt("roomTwo", 0);
            PlayerPrefs.SetInt("roomThree", 0);
        }

        public static void MarkRoomAsCompleted(int roomNumber)
        {
            switch (roomNumber)
            {
                case 1:
                {
                    PlayerPrefs.SetInt("roomOne", 1);
                    break;
                }
                case 2:
                {
                    PlayerPrefs.SetInt("roomTwo", 1);
                    break;
                }
                case 3:
                {
                    PlayerPrefs.SetInt("roomThree", 1);
                    break;
                }
                default:
                {
                    throw new Exception("No existe el numero de room que se intenta guardar");
                }
            }
        }

        public static bool IsRoomCompleted(int roomNumber)
        {
            switch (roomNumber)
            {
                case 1:
                {
                    return PlayerPrefs.GetInt("roomOne") == 1;
                }
                case 2:
                {
                    return PlayerPrefs.GetInt("roomTwo") == 1;
                }
                case 3:
                {
                    return PlayerPrefs.GetInt("roomThree") == 1;
                }
                default:
                {
                    throw new Exception("No existe el numero de room que se intenta pedir");
                }
            }
        }
    }
}