using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private float _health;
    private float _maxHealth;
    private float _stamina;
    private float _maxStamina;
    private float _mana;
    private float _maxMana;

    public string Name;
    public float Health { 
        get { return _health; } 
        set { _health = value; } 
    }
    public float MaxHealth { 
        get { return _maxHealth; } 
        set { _maxHealth = value; } 
    }
    public float Stamina { 
        get { return _stamina; } 
        set { _stamina = value; }
    }
    public float MaxStamina {
        get { return _maxStamina; }
        set { _maxStamina = value; }
    }
    public float Mana { 
        get { return _mana; } 
        set { _mana = value; } 
    }
    public float MaxMana
    {
        get { return _maxMana; }
        set { _maxMana = value; }
    }

    public int Level;
    public int Experience;
    public int ExperienceToNextLevel;

    public int Intelligence;
    public int Strength;
    public int Dexterity;
    public int Constitution;
    public int Wisdom;
}
