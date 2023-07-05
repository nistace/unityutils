// GENERATED AUTOMATICALLY FROM 'Assets/Prefabs/Utils/Debug/DebugControls.inputactions'

#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace NiUtils.Debugging
{
    public class @DebugControls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @DebugControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""DebugControls"",
    ""maps"": [
        {
            ""name"": ""Common"",
            ""id"": ""442003ab-5a36-42ff-99aa-6b0f4e16c357"",
            ""actions"": [
                {
                    ""name"": ""Previous"",
                    ""type"": ""Button"",
                    ""id"": ""74b11d06-9378-48a0-857e-d0460c13e084"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Next"",
                    ""type"": ""Button"",
                    ""id"": ""62881bab-1b57-4fff-8466-e7e9a489bf66"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c23cb279-0fa4-40de-9eb1-5ab310ec0a4c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Previous"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20f07365-dfd1-4c53-a459-ee1b74623e7f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Common
            m_Common = asset.FindActionMap("Common", throwIfNotFound: true);
            m_Common_Previous = m_Common.FindAction("Previous", throwIfNotFound: true);
            m_Common_Next = m_Common.FindAction("Next", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Common
        private readonly InputActionMap m_Common;
        private ICommonActions m_CommonActionsCallbackInterface;
        private readonly InputAction m_Common_Previous;
        private readonly InputAction m_Common_Next;
        public struct CommonActions
        {
            private @DebugControls m_Wrapper;
            public CommonActions(@DebugControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Previous => m_Wrapper.m_Common_Previous;
            public InputAction @Next => m_Wrapper.m_Common_Next;
            public InputActionMap Get() { return m_Wrapper.m_Common; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CommonActions set) { return set.Get(); }
            public void SetCallbacks(ICommonActions instance)
            {
                if (m_Wrapper.m_CommonActionsCallbackInterface != null)
                {
                    @Previous.started -= m_Wrapper.m_CommonActionsCallbackInterface.OnPrevious;
                    @Previous.performed -= m_Wrapper.m_CommonActionsCallbackInterface.OnPrevious;
                    @Previous.canceled -= m_Wrapper.m_CommonActionsCallbackInterface.OnPrevious;
                    @Next.started -= m_Wrapper.m_CommonActionsCallbackInterface.OnNext;
                    @Next.performed -= m_Wrapper.m_CommonActionsCallbackInterface.OnNext;
                    @Next.canceled -= m_Wrapper.m_CommonActionsCallbackInterface.OnNext;
                }
                m_Wrapper.m_CommonActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Previous.started += instance.OnPrevious;
                    @Previous.performed += instance.OnPrevious;
                    @Previous.canceled += instance.OnPrevious;
                    @Next.started += instance.OnNext;
                    @Next.performed += instance.OnNext;
                    @Next.canceled += instance.OnNext;
                }
            }
        }
        public CommonActions @Common => new CommonActions(this);
        public interface ICommonActions
        {
            void OnPrevious(InputAction.CallbackContext context);
            void OnNext(InputAction.CallbackContext context);
        }
    }
}
#endif