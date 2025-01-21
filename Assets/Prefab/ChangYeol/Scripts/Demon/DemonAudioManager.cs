using BS.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sounds
{
    public string audioName;
    public AudioClip audioClip;
    public AudioUtility.AudioGroups group;
}
namespace BS.Audio
{
    public class DemonAudioManager : MonoBehaviour
    {
        #region Variables
        public List<Sounds> sounds;
        #endregion
    }
}