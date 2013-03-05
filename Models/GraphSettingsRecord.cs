using System.ComponentModel.DataAnnotations;
using Associativy.Models.Services;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models
{
    [OrchardFeature("Associativy.Administration")]
    public class GraphSettingsRecord
    {
        public virtual int Id { get; set; }
        [StringLength(1024)]
        public virtual string GraphName { get; set; }
        public virtual bool UseCache { get; set; }
        public virtual int InitialZoomLevel { get; set; }
        public virtual int ZoomLevelCount { get; set; }
        public virtual int MaxDistance { get; set; }
        public virtual int MaxNodeCount { get; set; }


        public GraphSettingsRecord()
        {
            var def = MindSettings.Default;

            UseCache = true;
            InitialZoomLevel = def.ZoomLevel;
            ZoomLevelCount = def.ZoomLevelCount;
            MaxDistance = def.MaxDistance;
            MaxNodeCount = def.MaxNodeCount;
        }
    }

    [OrchardFeature("Associativy.Administration")]
    public static class GraphSettingsExtensions
    {
        public static IMindSettings AsMindSettings(this GraphSettingsRecord record)
        {
            return new MindSettings
            {
                UseCache = record.UseCache,
                ZoomLevel = record.InitialZoomLevel,
                ZoomLevelCount = record.ZoomLevelCount,
                MaxDistance = record.MaxDistance,
                MaxNodeCount = record.MaxNodeCount
            };
        }
    }
}