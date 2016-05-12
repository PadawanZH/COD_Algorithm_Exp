using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NormalizeData
{
    class NormalizedData
    {
        static void Main(string[] args)
        {
            NormalizedData nd = new NormalizedData();
            nd.GenerateData(System.Environment.CurrentDirectory + "\\newData.txt", 2, 100000);
        }

        public void NormalizedExistingData(string sourceFile, string targetFile, int dimension)
        {
            string strTem = "";
            char[] _delimiter = { ' ' };
            double[] maxValues = new double[dimension];
            double[] minValues = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                maxValues[i] = double.MinValue;
                minValues[i] = double.MaxValue;
            }
            using (StreamReader fir_rd = new StreamReader(sourceFile, Encoding.Default))
            {
                while (!fir_rd.EndOfStream)
                {
                    strTem = fir_rd.ReadLine();
                    string[] data = strTem.Trim().Split(_delimiter, StringSplitOptions.RemoveEmptyEntries);
                    if (data.Length != dimension)
                    {
                        throw new Exception("Error with dimension during split operation");
                    }
                    else
                    {
                        for (int i = 0; i < dimension; i++)
                        {
                            double vi = Convert.ToDouble(data[i]);
                            if (maxValues[i] < vi)
                            {
                                maxValues[i] = vi;
                            }
                            if (minValues[i] > vi)
                            {
                                minValues[i] = vi;
                            }
                        }
                    }
                }
                using (StreamReader sec_rd = new StreamReader(sourceFile, Encoding.Default))
                {
                    StreamWriter sw = new StreamWriter(targetFile, false);
                    while (!sec_rd.EndOfStream)
                    {
                        strTem = sec_rd.ReadLine();
                        string[] data = strTem.Trim().Split(_delimiter, StringSplitOptions.RemoveEmptyEntries);

                        string targetLine = "";
                        for (int i = 0; i < dimension; i++)
                        {
                            double vi = Convert.ToDouble(data[i]);
                            double newVi = (vi - minValues[i]) / (maxValues[i] - minValues[i]);
                            targetLine += (" " + vi.ToString("#0.000000"));
                        }
                        sw.WriteLine(targetLine);

                    }
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        public void GenerateData(string targetFile, int dimension, int numberOfSet)
        {
            Random rd = new Random();
            using (StreamWriter sw = new StreamWriter(targetFile))
            {
                for (int j = 0; j < numberOfSet; j++)
                {
                    string lineToWrite = "";
                    for (int i = 0; i < dimension; i++)
                    {
                        
                        lineToWrite += " " + rd.NextDouble().ToString("#0.00000");
                    }
                    sw.WriteLine(lineToWrite.TrimStart());
                }
            }  
        }
    }
}
