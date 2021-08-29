using IeCoreInterfaces;
using System;

namespace IeCore
{
	public class SceneManager : ISceneManager
	{
		public IScene Scene { get; private set; }

		public SceneManager(IScene scene)
		{
			Scene = scene;
		}

		public void OnLoad()
		{
			Scene.OnLoad();
		}

		public void OnRender()
		{
			//throw new NotImplementedException();
		}

		public void OnResized()
		{
			//throw new NotImplementedException();
		}

		public void OnUnload()
		{
			throw new NotImplementedException();
		}

		public void OnUpdated()
		{
			Scene.OnUpdated();
		}
	}
}
