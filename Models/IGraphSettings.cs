﻿
namespace Associativy.Administration.Models
{
    public interface IGraphSettings
    {
        bool UseCache { get; set; }
        int InitialZoomLevel { get; set; }
        int ZoomLevelCount { get; set; }
        int MaxDistance { get; set; }
        int MaxConnectionCount { get; set; }
        string ImplicitlyCreatableContentType { get; set; }
    }
}
