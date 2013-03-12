using System.ComponentModel.DataAnnotations;
using Associativy.Models.Services;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models
{
    public class GraphSettingsRecord
    {
        public virtual int Id { get; set; }
        [StringLength(1024)]
        public virtual string GraphName { get; set; }
        public virtual bool UseCache { get; set; }
        public virtual int InitialZoomLevel { get; set; }
        public virtual int ZoomLevelCount { get; set; }
        public virtual int MaxDistance { get; set; }
        public virtual int MaxConnectionCount { get; set; }


        public GraphSettingsRecord()
        {
            var def = MindSettings.Default;

            UseCache = false;
            InitialZoomLevel = 0;
            ZoomLevelCount = 10;
            MaxDistance = def.MaxDistance;
            MaxConnectionCount = 50;
        }
    }
}