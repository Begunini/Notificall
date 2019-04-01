using System.IO;
using NAudio.Wave;

namespace Infrastructure
{
    public class AudioFileConverter
    {
        public byte[] Convert(byte[] inputFile)
        {
            var output = new byte[0];
            using (var memoryStream = new MemoryStream(inputFile))
            {
                using (var reader = new WaveFileReader(memoryStream))
                {
                    var newFormat = new WaveFormat(8000, 16, 1);
                    using (var conversionStream = new WaveFormatConversionStream(newFormat, reader))
                    {
                        using (var resultMemoryStream = new MemoryStream())
                        {
                            WaveFileWriter.WriteWavFileToStream(resultMemoryStream, conversionStream);
                            output = resultMemoryStream.ToArray();
                        }
                    }
                }
            }
            return output;
        }
    }
}
