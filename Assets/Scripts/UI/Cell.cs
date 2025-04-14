/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Cell : MonoBehaviour
{
    [SerializeField] Animator _animator = default;
    public Animator Animator => _animator;

    public AnimationScrollRect ScrollRect { get; private set; }
    public float CurrentPosition { get; private set; } = 0;

    [NonSerialized] public int Index;

    public virtual void Initialize(AnimationScrollRect scrollRect)
    {
        ScrollRect = scrollRect;
    }

    public virtual void Enable(ICellData content)
    {
        gameObject.SetActive(true);
    }

    public virtual void Disable() 
    {
        gameObject.SetActive(false);
    }

    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }

    public virtual void UpdatePosition(float position)
    {
        CurrentPosition = position;

        if (_animator.isActiveAndEnabled)
        {
            _animator.Play(AnimatorHash.Scroll, -1, position);
        }

        _animator.speed = 0;
    }


}

