using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//Assembly exports - globalne exporty
//uruchomienie CompileBinding na cały projekt:
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]


//dodanie czcionek/stylów do skompilowanego projektu
[assembly: ExportFont("CustomFont.ttf", Alias = "AC")]
[assembly: ExportFont("fa-brands-400.ttf", Alias = "FAR")]
[assembly: ExportFont("fa-regular-400.ttf", Alias = "FAS")]
[assembly: ExportFont("fa-solid-900.ttf", Alias = "FAB")]
[assembly: ExportFont("Solid-900.otf", Alias = "FS")]