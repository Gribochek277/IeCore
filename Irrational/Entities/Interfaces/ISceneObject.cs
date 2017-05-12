using System;
using OpenGL;
namespace IrrationalSpace
{
	public interface ISceneObject
	{
		Vector3 position { get; set; }

		Vector3 scale { get; set; }

		Vector3 rotation { get; set;}

		Material mat { get; set;}

		Scene scene { get; set; }

		void SetMAterial();

		void ChangeTransform();
	}

}
