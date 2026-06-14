using Radio_Search.Importer.Canada.Services.Interfaces.EnvironmentManagement;
using Spire.Pdf;
using System.Reflection;

namespace Radio_Search.Importer.Canada.Services.Implementations.EnvironmentManagement
{
    public class FontManagement : IFontManagement
    {
        private const string prefix = "Radio_Search.Importer.Canada.Services.Fonts.";

        // Spire's custom font folder is process-global. Guard so we copy + register only once,
        // even if InitializaFonts is called from multiple import invocations concurrently.
        private static readonly object _lock = new();
        private static string? _fontDir;

        public string InitializaFonts()
        {
            if (_fontDir is not null)
                return _fontDir;

            lock (_lock)
            {
                if (_fontDir is not null)
                    return _fontDir;

                // Use a guaranteed-writable folder. Avoids relying on system font install
                // or fontconfig, which are unavailable on a managed Azure Function.
                var fontDir = Path.Combine(Path.GetTempPath(), "fonts");
                Directory.CreateDirectory(fontDir);

                var asm = Assembly.GetExecutingAssembly();

                foreach (var resourceName in asm.GetManifestResourceNames())
                {
                    if (!resourceName.StartsWith(prefix, StringComparison.Ordinal))
                        continue;

                    var fileName = resourceName[prefix.Length..];
                    var dst = Path.Combine(fontDir, fileName);
                    if (File.Exists(dst))
                        continue;

                    using var stream = asm.GetManifestResourceStream(resourceName)!;
                    using var file = File.Create(dst);
                    stream.CopyTo(file);
                }

                // Point Spire.PDF at our font folder so it resolves fonts (e.g. Times New Roman)
                // without depending on a system font install or fontconfig.
                PdfDocument.LoadCustomFontFolder(fontDir);

                _fontDir = fontDir;
                return fontDir;
            }
        }
    }
}
