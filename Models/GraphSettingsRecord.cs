using System.ComponentModel.DataAnnotations;
using Associativy.Models.Mind;
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


        public GraphSettingsRecord()
        {
            UseCache = true;
            InitialZoomLevel = 0;
            ZoomLevelCount = 10;
            MaxDistance = 3;
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
                MaxDistance = record.MaxDistance
            };
        }
    }
}