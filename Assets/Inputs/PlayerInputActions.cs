// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player Controls"",
            ""id"": ""32db980b-2c50-46e9-a75f-92b5f24cfe93"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""105f49fa-7392-4d8a-be95-d5e7c989e43b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""587e5956-0c6f-40d4-9985-b287032683a6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Heal"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b333cd9a-4b62-40c9-abea-9658d8590325"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ce3a0cbf-a477-489e-bb74-9ffb60c9fa50"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""PassThrough"",
                    ""id"": ""fb998566-c859-4491-800a-909ddf62b02b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponChange"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2fc06477-dd40-4ef2-8993-b715f157d460"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""fe5ad147-c9ce-43f0-8d06-dc35dcd9a91e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""853dd9c2-4fc3-4517-ae18-bc228d57775f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d3a357d9-6213-4cd1-afe1-90938c6393b0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5096e468-e28c-4f61-9d13-c55297b2d78b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7b9b2a2e-c242-4042-95da-a49014973868"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""46b4dabc-3fc1-4c0b-ae24-aaca5d8034a2"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14f44bde-3189-4443-b729-b0f3ebcb51d9"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c5188773-d81e-4867-9035-d538c7ee2694"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ffb20fb-8300-4177-b9ad-88b2a84882f2"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press,Hold"",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8daf8d84-24fe-4236-84d9-b938bf9e49e2"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press,Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85ae4c85-4838-4d5f-9cf6-abb16b50da0e"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Heal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a568fea7-32cc-4d2b-9834-9a584556b2d1"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Heal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d9f7dd7b-1cb3-4306-90f0-ff6935ef255b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8171cd73-179f-44ce-b789-58d3087b0007"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c34b1a19-391e-4145-b4d1-8ced79d64a60"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""WeaponChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GameManagerControls"",
            ""id"": ""04855f39-fe2d-4e8c-8495-384da1534246"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0b117442-cfcd-4f9f-b991-d8e3632e9692"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""08cabcb2-c79a-426a-a701-aad3c5216425"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf691a03-6a28-4e3d-9be4-cf910b7d0d44"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player Controls
        m_PlayerControls = asset.FindActionMap("Player Controls", throwIfNotFound: true);
        m_PlayerControls_Move = m_PlayerControls.FindAction("Move", throwIfNotFound: true);
        m_PlayerControls_Look = m_PlayerControls.FindAction("Look", throwIfNotFound: true);
        m_PlayerControls_Heal = m_PlayerControls.FindAction("Heal", throwIfNotFound: true);
        m_PlayerControls_Fire = m_PlayerControls.FindAction("Fire", throwIfNotFound: true);
        m_PlayerControls_Dash = m_PlayerControls.FindAction("Dash", throwIfNotFound: true);
        m_PlayerControls_WeaponChange = m_PlayerControls.FindAction("WeaponChange", throwIfNotFound: true);
        // GameManagerControls
        m_GameManagerControls = asset.FindActionMap("GameManagerControls", throwIfNotFound: true);
        m_GameManagerControls_Pause = m_GameManagerControls.FindAction("Pause", throwIfNotFound: true);
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

    // Player Controls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Move;
    private readonly InputAction m_PlayerControls_Look;
    private readonly InputAction m_PlayerControls_Heal;
    private readonly InputAction m_PlayerControls_Fire;
    private readonly InputAction m_PlayerControls_Dash;
    private readonly InputAction m_PlayerControls_WeaponChange;
    public struct PlayerControlsActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerControlsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControls_Move;
        public InputAction @Look => m_Wrapper.m_PlayerControls_Look;
        public InputAction @Heal => m_Wrapper.m_PlayerControls_Heal;
        public InputAction @Fire => m_Wrapper.m_PlayerControls_Fire;
        public InputAction @Dash => m_Wrapper.m_PlayerControls_Dash;
        public InputAction @WeaponChange => m_Wrapper.m_PlayerControls_WeaponChange;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Heal.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnHeal;
                @Heal.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnHeal;
                @Heal.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnHeal;
                @Fire.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnFire;
                @Dash.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDash;
                @WeaponChange.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnWeaponChange;
                @WeaponChange.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnWeaponChange;
                @WeaponChange.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnWeaponChange;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Heal.started += instance.OnHeal;
                @Heal.performed += instance.OnHeal;
                @Heal.canceled += instance.OnHeal;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @WeaponChange.started += instance.OnWeaponChange;
                @WeaponChange.performed += instance.OnWeaponChange;
                @WeaponChange.canceled += instance.OnWeaponChange;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);

    // GameManagerControls
    private readonly InputActionMap m_GameManagerControls;
    private IGameManagerControlsActions m_GameManagerControlsActionsCallbackInterface;
    private readonly InputAction m_GameManagerControls_Pause;
    public struct GameManagerControlsActions
    {
        private @PlayerInputActions m_Wrapper;
        public GameManagerControlsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_GameManagerControls_Pause;
        public InputActionMap Get() { return m_Wrapper.m_GameManagerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameManagerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IGameManagerControlsActions instance)
        {
            if (m_Wrapper.m_GameManagerControlsActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_GameManagerControlsActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GameManagerControlsActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GameManagerControlsActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_GameManagerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public GameManagerControlsActions @GameManagerControls => new GameManagerControlsActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnHeal(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnWeaponChange(InputAction.CallbackContext context);
    }
    public interface IGameManagerControlsActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
}
