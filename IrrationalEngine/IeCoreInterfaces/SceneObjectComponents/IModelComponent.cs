﻿using IeCoreEntites.Model;
using IeCoreInterfaces.Core;

namespace IeCoreInterfaces.SceneObjectComponents
{
    /// <summary>
    /// Represents scene object component which responsible for models.
    /// </summary>
    public interface IModelComponent: ISceneObjectComponent
    {
        /// <summary>
        /// Get model which stored in this component.
        /// </summary>
        Model Model { get; }

        /// <summary>
        /// Return Vertex Buffer Object data from model.
        /// </summary>
        /// <returns></returns>
        float[] GetVBODataOfModel();
        /// <summary>
        /// Return Indices's data from model.
        /// </summary>
        /// <returns></returns>
        uint[] GetIndicesOfModel();
    }
}
