using System.Collections.Generic;

namespace Infrastructure
{
    public class TemplateToTextConverter
    {
        public string GenerateResultingText(List<string> headers, List<string> item, string template)
        {
            for (var i = 0; i < headers.Count; i++)
            {
                template = template.Replace($"%%{headers[i]}%%", item[i]);
            }

            return template;
        }
    }
}
