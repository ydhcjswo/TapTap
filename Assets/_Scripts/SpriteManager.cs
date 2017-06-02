using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    static SpriteManager m_Instance;

    public static SpriteManager Instance
    {
        get
        {
            return m_Instance;
        }
    }


    public Sprite[] m_AnimalSprites = new Sprite[10];



    private void Awake()
    {
        m_Instance = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    public Sprite GetAnimalSprite(int index)
    {
        return m_AnimalSprites[index];
    }

}
