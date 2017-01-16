﻿using System;
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
                        if (mappedAction.InputType == InputType.Keyboard)
                        {
                            var keys =
                                mappedAction.AssingedValue.Split(',')
                                    .Select(t => (Keys)Enum.Parse(typeof(Keys), t))
                                    .ToArray();

                            RegisterAxis(inputAction.Name, keys[0], keys[1], keys[2], keys[3]);
                        }
                    }
                    else
                    {
                        if (mappedAction.InputType == InputType.Keyboard)
                        {
                            RegisterAction(inputAction.Name, (Keys)Enum.Parse(typeof(Keys), mappedAction.AssingedValue));
                        }
                    }
                }
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
            var referenceList = GetOrCreateActionList(name);

            referenceList.Add(new KeyboardInputAction(KeyboardHandler, name , key));
        }

        public void RegisterAxis(string name, Keys left, Keys right, Keys up, Keys down)
        {
            var referenceList = GetOrCreateAxisList(name);

            referenceList.Add(new KeyboardAxisAction(KeyboardHandler, name, left, right, up, down));
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
