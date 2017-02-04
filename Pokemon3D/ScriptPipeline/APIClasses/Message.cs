using System.Threading;
using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Scripting.Types;
using Pokemon3D.Common.ScriptPipeline;
using Pokemon3D.GameCore;

namespace Pokemon3D.ScriptPipeline.ApiClasses
{
    [ApiClass(ClassName = "Message")]
    internal class Message : ApiClass
    {
        // ReSharper disable InconsistentNaming
        public static SObject show(ScriptProcessor processor, SObject[] parameters)
        // ReSharper restore InconsistentNaming
        {
            object[] netObjects;
            if (EnsureTypeContract(parameters, new[] { typeof(string) }, out netObjects))
            {
                var autoResetEvent = new AutoResetEvent(false);

                var messengerService = GameProvider.GameInstance.GetService<MessengerService>();
                messengerService.ShowMessage(new MessageData
                {
                    Text = (string)netObjects[0],
                    OnFinished = () => autoResetEvent.WaitOne()
                });

                BlockThreadUntilCondition(() => autoResetEvent.WaitOne());
            }

            return ScriptInAdapter.GetUndefined(processor);
        }
    }
}
