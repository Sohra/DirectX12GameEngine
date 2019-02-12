﻿using DirectX12GameEngine.Rendering.Shaders;

namespace DirectX12GameEngine.Rendering.Lights
{
    [ConstantBufferResource]
    public class DirectionalLightGroup : DirectLightGroup
    {
#nullable disable
        [ShaderResource] public DirectionalLightData[] Lights;
#nullable enable

        /// <summary>
        /// Compute the light color/direction for the specified index within this group.
        /// </summary>
        [ShaderMethod]
        protected override void PrepareDirectLightCore(int lightIndex)
        {
            LightStream.LightColor = Lights[lightIndex].Color;
            LightStream.LightDirection = -Lights[lightIndex].Direction;
        }
    }
}