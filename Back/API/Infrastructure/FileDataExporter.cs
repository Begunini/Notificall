using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace API.Infrastructure
{
    public class FileDataExporter : IFileDataExporter
    {
        public async Task<List<KeyValuePair<string, List<string>>>> GetData(IFormFile file)
        {
            var data = new List<KeyValuePair<string, List<string>>>();

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream).ConfigureAwait(false);

                using (var package = new ExcelPackage(memoryStream))
                {
                    var worksheet = package.Workbook.Worksheets[1];

                    var empty = false;
                    var maxCols = 1;
                    var content = new List<string>();
                    while (!empty)
                    {
                        var value = worksheet.Cells[1, maxCols].Value as string;

                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            content.Add(value);
                            maxCols++;
                        }
                        else
                        {
                            maxCols--;
                            empty = true;
                        }
                    }

                    data.Add(new KeyValuePair<string, List<string>>("headers", content));

                    empty = false;
                    var row = 2;
                    while (!empty)
                    {
                        content = new List<string>();
                        for (var column = 1; column <= maxCols; column++)
                        {
                            var cellValue = worksheet.Cells[row, column].Value;

                            var value = string.Empty;

                            if (column == 1 && cellValue == null)
                            {
                                empty = true;
                            }
                            else if(cellValue != null)
                            {
                                value = cellValue.ToString();
                                if (!string.IsNullOrWhiteSpace(value))
                                {
                                    content.Add(value);
                                }
                                else if (column == 1 && string.IsNullOrWhiteSpace(value))
                                {
                                    empty = true;
                                }
                            }
                        }

                        if (!empty)
                        {
                            data.Add(new KeyValuePair<string, List<string>>("item", content));
                            row++;
                        }
                    }
                }
            }

            return data;
        }
    }
}
