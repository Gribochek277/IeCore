using System;
using IeCoreInterfaces.Input;

namespace IeCore.DefaultImplementations.Input
{
	public class InputService : IInputService
	{
		public event Action<string> KeyPressed;

		public void RaiseKeyPressed(string key) => KeyPressed?.Invoke(key);
	}
}
