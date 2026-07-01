using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dylanng.Core
{
    public interface IService
    { 
        void Initialize(); 
    }

    public interface IManager : IService { }

    public interface ISystem : IService { }

    public interface IEvent { }

    public interface IDamageable
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
        void TakeDamage(float amount);
        void Heal(float amount);
        void Die();
    }
}