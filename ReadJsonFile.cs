using System.Text.Json;

/*
 * Este archivo se encarga de leer un fichero .json que contenga API_KEY
 * Si no se encuentra el atributo en el fichero, devuelve un string vacío
 * 
 */

namespace JsonReader
{
  class ReadJsonFile
  {
      public static string getApiKey()
      {
        string actualDirectory = Directory.GetCurrentDirectory();
        string file = Path.Combine(actualDirectory, "envFile.json");
        string jsonToString = File.ReadAllText(file);
        var json = JsonSerializer.Deserialize<Items>(jsonToString);

        return json.API_KEY ?? "";
      }

      // En esta sobrecarga tendra que eleguir el archivo que contendra el API_KEY
      public static string getApiKey(string envFile)
      {
        string actualDirectory = Directory.GetCurrentDirectory();
        string file = Path.Combine(actualDirectory, envFile);
        string jsonToString = File.ReadAllText(file);
        var json = JsonSerializer.Deserialize<Items>(jsonToString);

        return json.API_KEY ?? "";
      }

    class Items
    {
      public string API_KEY {get; set;}
    }
  }
}
