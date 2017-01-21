using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.DataModel.GameCore;

namespace Pokemon3D.InputSystem
{
    public class InputSystem
    {
        private readonly Dictionary<string, List<InputAction>> _inputActionsByName = new Dictionary<string, List<InputAction>>();
        private readonly Dictionary<string, List<AxisAction>> _axisActionsByName = new Dictionary<string, List<AxisAction>>();

        public KeyboardHandler KeyboardHandler { get; } = new KeyboardHandler();
        public GamePadHandler GamePadHandler { get; } = new GamePadHandler();
        public MouseHandler MouseHandler { get; } = new MouseHandler();

        public void LoadFromConfiguration(InputActionModel[] inputActions)
        {
            foreach (var inputAction in inputActions)
            {
                foreach (var mappedAction in inputAction.ActionsModel)
                {
                    if (mappedAction.IsAxis)
                    {
                        MapAxisToDevice(inputAction.Name, mappedAction);
                    }
                    else
                    {
                        MapActionToDevice(inputAction.Name, mappedAction);
                    }
                }
            }
        }

        private void MapActionToDevice(string name, MappedActionModel mappedAction)
        {
            switch (mappedAction.InputType)
            {
                case InputType.Keyboard:
                    RegisterAction(name, (Keys)Enum.Parse(typeof(Keys), mappedAction.AssingedValue));
                    break;
                case InputType.GamePad:
                    RegisterAction(name, (Buttons)Enum.Parse(typeof(Buttons), mappedAction.AssingedValue));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MapAxisToDevice(string name, MappedActionModel mappedAction)
        {
            switch (mappedAction.InputType)
            {
                case InputType.Keyboard:
                    var keys = mappedAction.AssingedValue.Split(',').Select(t => (Keys)Enum.Parse(typeof(Keys), t)).ToArray();
                    RegisterAxis(name, keys[0], keys[1], keys[2], keys[3]);
                    break;
                case InputType.GamePad:
                    var axis = (GamePadAxis)Enum.Parse(typeof(GamePadAxis), mappedAction.AssingedValue);
                    RegisterAxis(name, axis);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Update(GameTime time)
        {
            KeyboardHandler.Update(time);
            GamePadHandler.Update(time);
            MouseHandler.Update();
        }

        public void RegisterAction(string name, Keys key)
        {
            GetOrCreateActionList(name).Add(KeyboardHandler.DefineAction(name, key));
        }

        public void RegisterAction(string name, Buttons key)
        {
            GetOrCreateActionList(name).Add(GamePadHandler.DefineAction(name, key));
        }

        public void RegisterAxis(string name, Keys left, Keys right, Keys up, Keys down)
        {
            GetOrCreateAxisList(name).Add(KeyboardHandler.DefineAxis(name, left, right, up, down));
        }

        public void RegisterAxis(string name, GamePadAxis axis)
        {
            GetOrCreateAxisList(name).Add(GamePadHandler.DefineAxis(name, axis));
        }

        private List<InputAction> GetOrCreateActionList(string actionName)
        {
            List<InputAction> referenceList;
            if (_inputActionsByName.TryGetValue(actionName, out referenceList)) return referenceList;

            referenceList = new List<InputAction>();
            _inputActionsByName.Add(actionName, referenceList);
            return referenceList;
        }

        private List<AxisAction> GetOrCreateAxisList(string axisName)
        {
            List<AxisAction> referenceList;
            if (_axisActionsByName.TryGetValue(axisName, out referenceList)) return referenceList;
            referenceList = new List<AxisAction>();
            _axisActionsByName.Add(axisName, referenceList);
            return referenceList;
        }

        public bool IsPressed(string actionName)
        {
            List<InputAction> referenceList;
            if (!_inputActionsByName.TryGetValue(actionName, out referenceList)) throw new ApplicationException($"There is no action '{actionName}' defined");

            return referenceList.Any(i => i.IsPressed());
        }

        public bool IsPressedOnce(string actionName)
        {
            List<InputAction> referenceList;
            if (!_inputActionsByName.TryGetValue(actionName, out referenceList)) throw new ApplicationException($"There is no action '{actionName}' defined");

            return referenceList.Any(i => i.IsPressedOnce());
        }

        public Vector2 GetAxis(string axisName)
        {
            List<AxisAction> referenceList;
            if (!_axisActionsByName.TryGetValue(axisName, out referenceList)) throw new ApplicationException($"There is no axis '{axisName}' defined");

            return referenceList.First().GetAxis();
        }
    }
}
