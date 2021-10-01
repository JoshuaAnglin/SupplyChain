using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCG.Stats
{

    public class BaseStats : MonoBehaviour
    {
        [Range(1, 5)] [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
    }
}
