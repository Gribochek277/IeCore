using System;

namespace IeCoreInterfaces.Input
{
	/// <summary>
	/// Receives key press notifications from the window backend and
	/// forwards them to any subscriber in the engine (e.g. scenes, components).
	/// </summary>
	public interface IInputService
	{
		/// <summary>
		/// Raised whenever a key is pressed. The string matches the Silk.NET / OpenTK
		/// Key enum name (e.g. "Right", "Left", "Up", "Down", "Space", …).
		/// </summary>
		event Action<string> KeyPressed;

		/// <summary>
		/// Called by the window backend to forward a key press.
		/// </summary>
		void RaiseKeyPressed(string key);
	}
}
