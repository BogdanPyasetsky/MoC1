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
            int cpIndex;                          //cyphertext index
            for (int i = 0; i < 20; i++)          // key index
            {
                for (int j = 0; j < 20; j++)      // opentext index
                {
                    cpIndex = EncrTable[i, j];
                    CProb[cpIndex] += MProb[j] * KProb[i];
                }
            }           
            return CProb;
        }

        static float[,] MCProbCalc(float[] MProb, float[] KProb, int[,] EncrTable)
        {
            float[,] MCProb = new float[20, 20];
            int cpIndex;                          //cyphertext index
            for (int i = 0; i < 20; i++)          // key index
            {
                for (int j = 0; j < 20; j++)      // opentext index
                {
                    cpIndex = EncrTable[i, j];
                    MCProb[cpIndex, j] += MProb[j] * KProb[i];
                }
            }
            return MCProb;
        }

        static float[,] MCCondProbCalc(float[] CProb, float[,] MCProb)
        {
            float[,] MCcondProb = new float[20, 20];
            for (int i = 0; i < 20; i++)         //cyphertext index
            {
                for (int j = 0; j < 20; j++)     //opentext index
                {
                    MCcondProb[i, j] = MCProb[i, j] / CProb[i];
                }
            }
            return MCcondProb;
        } 

        static int DDF(float[,] MCcondProb, int C)
        {
            int M;
            float[] MCandidatesProb = new float[20];
            for (int i = 0; i < 20; i++)
            {
                MCandidatesProb[i] = MCcondProb[C, i];
            }
            M = Array.IndexOf(MCandidatesProb, MCandidatesProb.Max());
            return M;
        }

        static int SDF(float[,] MCcondProb, int C)
        {
            int M;
            float[] MCandidatesProb = new float[20];
            for (int i = 0; i < 20; i++)
            {
                MCandidatesProb[i] = MCcondProb[C, i];
            }
            var maxProb = MCandidatesProb.Max();
            List<int> CandidateIdx = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                if (MCandidatesProb[i] == maxProb)
                    CandidateIdx.Add(i);
            }
            var NumberOfCandidates = CandidateIdx.Count;
            float candidateProb = 1 / (float)NumberOfCandidates;
            var random = new Random();
            var rd = random.Next(NumberOfCandidates);
            M = CandidateIdx[rd];
            return M;
        }
            

        static void Main(string[] args)
        {
            
            float[] MP = new float[20];
            float[] KP = new float[20];
            int[,] ET = new int[20, 20];
            string v = "06";
            ReadFiles(v, MP, KP, ET);

            
            var CP = CProbCalc(MP, KP, ET);
            var MCP = MCProbCalc(MP, KP, ET);
            var MCcP = MCCondProbCalc(CP, MCP);
            var mDDF = DDF(MCcP, 5);
            var mSDF = SDF(MCcP, 5);
            

            /*
            string name = "prob.txt";
            string temp = "";
            for (int i = 0; i < 20; i++)
            {
                temp += CP[i].ToString() + "  ";
            }
            File.WriteAllText(name, temp);
            */

            /*
            float t = 0;
            
            for (int i = 0; i < 20; i++)
            {
                t = 0;
                for (int j = 0; j < 20; j++)
                {
                    Console.Write(MCcP[i, j] + "  ");
                    t += MCcP[i,j];
                }
                Console.WriteLine(t);
                Console.WriteLine();
                //t += CP[i];
            }*/

            


            Console.WriteLine(mDDF);
            Console.WriteLine(mSDF);
            Console.ReadKey();
        }
    }
}
