using NSwag;
using NSwag.CodeGeneration.CSharp;

//this code is used to generate the SmartFitnessClient.cs file from the OpenAPI specification of the Smart Fitness API.
//It uses the NSwag library to read the OpenAPI document and generate a C# client class that can be used to interact with the API.
var document = await OpenApiDocument.FromUrlAsync("http://localhost:5203/swagger/v1/swagger.json");

// Configure the C# client generator settings, including the class name and namespace for the generated code.
var settings = new CSharpClientGeneratorSettings
{
    ClassName = "SmartFitnessClient",
    CSharpGeneratorSettings =
    {
        Namespace = "Traqtiv.Mobile.Models"
    }
};

// Create a new C# client generator using the OpenAPI document and the specified settings, then generate the client code.
var generator = new CSharpClientGenerator(document, settings);
var code = generator.GenerateFile();

Directory.CreateDirectory("Models");
await File.WriteAllTextAsync("Models/SmartFitnessClient.cs", code);

Console.WriteLine("SmartFitnessClient.cs generated successfully!");
