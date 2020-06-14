using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Inputs;
using PBFramework.Allocation.Recyclers;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public abstract class BaseBeatsInput<T> : IBeatsInput<T>, IRecyclable
        where T : class, IInput
    {
        public event Action OnRelease;

        private T input;


        public T Input
        {
            get => input;
            set
            {
                if (input != null)
                {
                    input.State.OnValueChanged -= OnStateChange;
                    input = null;
                }
                if (value != null)
                {
                    input = value;
                    input.State.OnValueChanged += OnStateChange;
                }
            }
        }

        public bool IsActive { get; private set; }


        public virtual void OnRecycleNew()
        {
            IsActive = true;
        }

        public virtual void OnRecycleDestroy()
        {
            IsActive = false;
            Input = null;
        }

        /// <summary>
        /// Event called on input state change.
        /// </summary>
        private void OnStateChange(InputState state, InputState prevState)
        {
            if (state == InputState.Release)
                OnRelease?.Invoke();
        }
    }
}