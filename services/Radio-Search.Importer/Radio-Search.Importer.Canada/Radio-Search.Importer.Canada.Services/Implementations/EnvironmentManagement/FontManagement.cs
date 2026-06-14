using Radio_Search.Importer.Canada.Services.Interfaces.EnvironmentManagement;
using System.Reflection;

namespace Radio_Search.Importer.Canada.Services.Implementations.EnvironmentManagement
{
    public class FontManagement : IFontManagement
    {
        private const string prefix = "Radio_Search.Importer.Canada.Services.Fonts.";

        public string InitializaFonts()
        {
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

            return fontDir;
        }
    }
}
