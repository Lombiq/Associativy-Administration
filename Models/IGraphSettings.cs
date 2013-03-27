﻿
namespace Associativy.Administration.Models
{
    public interface IGraphSettings
    {
        string GraphName { get; set; }
        bool UseCache { get; set; }
        int InitialZoomLevel { get; set; }
        int ZoomLevelCount { get; set; }
        int MaxDistance { get; set; }
        int MaxConnectionCount { get; set; }
    }
}
