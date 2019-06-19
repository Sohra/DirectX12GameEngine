﻿namespace DirectX12GameEngine.Core.Assets
{
    public abstract class AssetWithSource<T> : Asset<T>
    {
        public string Source { get; set; } = "";

        public override string? MainSource => Source;
    }
}
