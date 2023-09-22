using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Projeto_CRUD
{
    internal class Program
    {
        static Uri CriarArquivoLocal()
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var iconPath = Path.Combine(outPutDirectory, "ListaBandas.txt");
            return new Uri(iconPath);
        }

        static void AdicionarBanda(List<string> listaBandas)
        {
            Console.WriteLine("Digite a banda que deseja adicionar, digite 0 para sair:");

            while (true)
            {
                string bandaAdicionada = Console.ReadLine();

                if (bandaAdicionada == "0")
                {
                    break;
                }

                else
                {
                    listaBandas.Add(bandaAdicionada);
                }               
            }
        }

        static void ExibirListaBandas(List<string> listaBandas)
        {
            for (int i = 0; i < listaBandas.Count; i++)
            {
                Console.WriteLine($"{i+1}.{listaBandas[i]}");
            }
            Console.WriteLine();
        }

        static void EditarListaBandas(List<string> listaBandas)
        {
            
            Console.WriteLine("Digite o número da banda que deseja editar:");
            ExibirListaBandas(listaBandas);

            int numeroDigitado = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Digite a nova banda:");
            string novaBanda = Console.ReadLine();

            listaBandas[numeroDigitado-1] = novaBanda;
            Console.WriteLine();
        }

        static void RemoverBanda(List<string> listaBandas)
        {
            Console.WriteLine("Digite o número da banda que deseja remover:");
            ExibirListaBandas(listaBandas);

            int numeroDigitado = Convert.ToInt32(Console.ReadLine());
            listaBandas.RemoveAt(numeroDigitado - 1);
            Console.WriteLine();
        }

        static void ExibirMenu()
        {
            Console.WriteLine("Bem-Vindo ao JSONify");
            Console.WriteLine("\nSelecione uma opção:\n");

            Console.WriteLine("1.Adicionar bandas à lista");
            Console.WriteLine("2.Visualizar as bandas adicionadas");
            Console.WriteLine("3.Editar a lista");
            Console.WriteLine("4.Deletar banda");

            Console.WriteLine("\n5.Sair e salvar alterações");
        }

        static void Main(string[] args)
        {
            try
            {
                StreamWriter arquivo;

                string caminho = CriarArquivoLocal().LocalPath;

                if (!File.Exists(caminho))
                {
                    arquivo = File.CreateText(caminho);
                    arquivo.Close();
                }
                
                List<string> listaBandasArquivo = File.ReadLines(caminho).ToList();
                string jsonListaBandas = JsonSerializer.Serialize(listaBandasArquivo);
                List<string> listaBandas = JsonSerializer.Deserialize<List<string>>(jsonListaBandas);

                while (true)
                {
                    ExibirMenu();

                    int opcaoSelecionada = Convert.ToInt32(Console.ReadLine());

                    switch (opcaoSelecionada)
                    {
                        case 1:
                            Console.Clear();
                            AdicionarBanda(listaBandas);
                            break;

                        case 2:
                            Console.Clear();
                            ExibirListaBandas(listaBandas);
                            break;

                        case 3:
                            Console.Clear();
                            EditarListaBandas(listaBandas);
                            break;

                        case 4:
                            Console.Clear();
                            RemoverBanda(listaBandas);
                            break;

                        case 5:
                            jsonListaBandas = JsonSerializer.Serialize(listaBandas);
                            arquivo = File.CreateText(caminho);
                            foreach (string banda in listaBandas)
                            {
                                arquivo.WriteLine(banda);
                            }
                            arquivo.Close();
                            Environment.Exit(0);
                            break;

                        default:
                            break;
                    }

                }
                
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                Console.ReadLine();
            }
        }
    }
}