using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System;

class RXSolutionFixer : AssetPostprocessor 
{
	public static void OnGeneratedCSProjectFiles() //secret method called by unity after it generates the solution
	{
		Debug.Log (string.Format("[{0}]: RXSolutionFixer.OnGeneratedCSProjectFiles()", DateTime.Now.ToShortTimeString()));

		string currentDir = Directory.GetCurrentDirectory();
		string[] csprojFiles = Directory.GetFiles(currentDir, "*.csproj");
		
		foreach(var filePath in csprojFiles)
		{
			FixProject(filePath);
		}
	}
	
	static bool FixProject(string filePath)
	{
		string content = File.ReadAllText(filePath);
		
		string searchString = "<TargetFrameworkVersion>v3.5</TargetFrameworkVersion>";
		string replaceString = "<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>";
		
		if(content.IndexOf(searchString) != -1)
		{
			content = Regex.Replace(content,searchString,replaceString);
			File.WriteAllText(filePath,content);
			return true;
		}
		else 
		{
			return false;
		}
	}
	
}