using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Embeddings;


namespace AgentIA
{

    public class FilePlugin
    {
       
        public FilePlugin() { }

        [KernelFunction("getFilesInDirectory")]
        [Description("Gets the list of files in directory")]
        public async Task<IEnumerable<string>> GetFiles(string directorypath)
        {
            var files = new List<string>();
            foreach (var file in Directory.EnumerateFiles(directorypath))
            {
                files.Add(file);
            }
            return files;
        }

        [KernelFunction("getFilenumber")]
        [Description("Gets the number of files in specified directory")]
        public async Task<int> GetNumberOfFiles(string directorypath)
        {
            int i = 0;
            foreach (var file in Directory.EnumerateFiles(directorypath))
            {
                i++;
            }
            return i;
        }

        [KernelFunction("CopyFiles")]
        [Description("Copy files from directory1 to directory2")]
        public async Task CopyFiles(string directory1, string directory2)
        {
            foreach (var file in Directory.GetFiles(directory1))
            {
                string filename = Path.GetFileName(file);
                string filepath = Path.Combine(directory2, filename);
                File.Copy(file, filepath, overwrite: true);
            }
        }

        [KernelFunction("MoveFiles")]
        [Description("Move files from directory1 to directory2")]
        public async Task MoveFiles(string directory1, string directory2)
        {
            foreach (var file in Directory.EnumerateFiles(directory1))
            {
                string filename = Path.GetFileName(file);
                string filepath = Path.Combine(directory2, filename);
                File.Move(file, filepath, overwrite: true);
            }
        }

        [KernelFunction("GetFilesFromContent")]
        [Description("Given a text and a directory, search the files where its content matches the text")]

        public async Task<IEnumerable<string>> getFileFromContent(string text, string directory)
        {
            var files = new List<string>();
            string fileContent = "";
            foreach (var file in Directory.GetFiles(directory))
            {
                using (var reader = new StreamReader(file))
                {
                    fileContent = reader.ReadToEnd();
                }

                if (fileContent.Contains(text))
                {
                    files.Add(file);
                }
            }
            return files;
        }

        //Exemple de fonction annexe Contains
        public bool Contains(string Content, string part)
        {
            for (int i = 0; i < Content.Length - part.Length + 1; i++)
            {
                string contentPart = Content.Substring(i, part.Length);
                int length = contentPart.Length;
                if (contentPart.Equals(part))
                {
                    return true;
                }
            }
            return false;
        }

        [KernelFunction("SearchFiles")]
        [Description("Search for a document similar to the given query")]
        public Task<List<string>> SearchAsync(string query, string directory)
        {


        }
    }
}
