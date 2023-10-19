
using System.Collections.Generic;
using UnityEngine;

public class CsvReader 
{
    public static Dictionary<string, string> ReadCSV(TextAsset file, int valueColumn, int valuetoskip)
    {
        Dictionary<string, string> disc = new Dictionary<string, string>();

        string[] filelines = file.text.Split('\n');
        string[] linesvalues;



        for(int i = valuetoskip; i < filelines.Length; i++) 
        {
            linesvalues = filelines[i].Split(';');

            if((linesvalues.Length > valueColumn ) && (string.IsNullOrEmpty(linesvalues[0])== false)) 
            {

                linesvalues[0] = linesvalues[0].Trim();
                if (disc.ContainsKey(linesvalues[0]) == true) 
                {
                    Debug.LogError(string.Format("Klucze w plikach sa zdublowane. Klucz: {0}. Plik {1}", linesvalues[0], file.text));
                }
                else 
                {
                    disc.Add(linesvalues[0], linesvalues[valueColumn]);
                }

            
            }

        
        }
        return disc;
    
   }
}
