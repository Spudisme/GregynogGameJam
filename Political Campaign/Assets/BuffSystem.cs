using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    PMCharacter character;

    private void Start() {
        character = GetComponent<PMCharacter>();
    }
}
