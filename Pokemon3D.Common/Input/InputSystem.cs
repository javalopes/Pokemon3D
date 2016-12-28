using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Input
{
    public class InputSystem
    {
        private readonly Dictionary<string, List<InputAction>> _inputActionsByName = new Dictionary<string, List<InputAction>>();
        private readonly Dictionary<string, List<AxisAction>> _axisActionsByName = new Dictionary<string, List<AxisAction>>();

        public KeyboardActionProvider KeyboardActionProvider { get; } = new KeyboardActionProvider();
        public GamePadActionProvider GamePadActionProvider { get; } = new GamePadActionProvider();

        public void RegisterAction(InputAction action)
        {
            List<InputAction> referenceList;
            if (!_inputActionsByName.TryGetValue(action.Name, out referenceList))
            {
                referenceList = new List<InputAction>();
                _inputActionsByName.Add(action.Name, referenceList);
            }

            referenceList.Add(action);
        }

        public void Update(GameTime time)
        {
            KeyboardActionProvider.Update(time);
            GamePadActionProvider.Update(time);
        }

        public void RegisterAxis(AxisAction axis)
        {
            List<AxisAction> referenceList;
            if (!_axisActionsByName.TryGetValue(axis.Name, out referenceList))
            {
                referenceList = new List<AxisAction>();
                _axisActionsByName.Add(axis.Name, referenceList);
            }

            referenceList.Add(axis);
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
