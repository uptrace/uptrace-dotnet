using OpenTelemetry;
using System.Collections.Generic;
using System.Diagnostics;

namespace Uptrace.OpenTelemetry
{
    /// <summary>
    /// Span processor that adds <see cref="Baggage"/> fields to every span
    /// </summary>
    public class BaggageSpanProcessor : BaseProcessor<Activity>
    {
        /// <inheritdoc />
        public override void OnStart(Activity activity)
        {
            foreach (KeyValuePair<string, string> entry in Baggage.Current)
            {
                activity.SetTag(entry.Key, entry.Value);
            }
        }
    }
}
