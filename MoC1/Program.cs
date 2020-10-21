using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace MoC1
{
    class Program
    {
        static void ReadFiles(string variant, float[] MProb, float[] KProb, int[,] EncrTable)
        {
            // Read prob file
            string path = "prob_" + variant + ".csv";
            string[] lines = File.ReadAllLines(path);
            string[] MProbStr = lines[0].Split(',');
            string[] KProbStr = lines[1].Split(',');
            for (int i=0; i < 20; i++)
            {
                MProb[i] = float.Parse(MProbStr[i], CultureInfo.InvariantCulture.NumberFormat);
                KProb[i] = float.Parse(KProbStr[i], CultureInfo.InvariantCulture.NumberFormat);
            }

            // Read table file
            path = "table_" + variant + ".csv";
            lines = File.ReadAllLines(path);
            string[] columns = new string[20];
            for(int i = 0; i < lines.Length; i++)
            {
                columns = lines[i].Split(',');
                for (int j = 0; j < columns.Length; j++)
                {
                    EncrTable[i, j] = int.Parse(columns[j]);
                }
            }
        }

        static float[] CProbCalc(float[] MProb, float[]KProb, int[,] EncrTable)
        {
            float[] CProb = new float[20];
            int cpIndex;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    cpIndex = EncrTable[i, j];
                    CProb[cpIndex] += MProb[i] * KProb[j];
                }
            }           
            return CProb;
        }

        static float[,] MCProbCalc(float[] MProb, float[] KProb, int[,] EncrTable)
        {
            float[,] CMProb = new float[20, 20];
            int cpIndex;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    cpIndex = EncrTable[i, j];
                    CMProb[i, cpIndex] += MProb[i] * KProb[j];
                }
            }


            return CMProb;
        }

        static float[,] MCCondProbCalc(float[] CProb, float[,] MCProb)
        {
            float[,] MCcondProb = new float[20, 20];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    MCcondProb[i, j] = MCProb[i, j] / CProb[j];
                }
            }
            return MCcondProb;
        }


        static void Main(string[] args)
        {
            float[] MP = new float[20];
            float[] KP = new float[20];
            int[,] ET = new int[20, 20];
            string v = "06";
            ReadFiles(v, MP, KP, ET);
            var MCP = MCProbCalc(MP, KP, ET);
            float t = 0;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Console.Write(MCP[i, j] + "  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine(t);
            Console.ReadKey();
        }
    }
}
