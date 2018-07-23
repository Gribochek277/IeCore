﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using Irrational.Core.Entities;
using Irrational.Utils.Interfaces;

namespace Irrational.Utils
{
    /// <summary>
    /// Loads info about materials
    /// </summary>
    public class WavefrontMaterialLoader : IMaterialLoader
    {
        public Dictionary<string, Material> LoadFromFile(string filename)
        {
            Dictionary<string, Material> mats = new Dictionary<string, Material>();

            try
            {
                String currentmat = "";
                using (StreamReader reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    String currentLine;

                    while (!reader.EndOfStream)
                    {
                        currentLine = reader.ReadLine();

                        if (!currentLine.StartsWith("newmtl"))
                        {
                            if (currentmat.StartsWith("newmtl"))
                            {
                                currentmat += currentLine + "\n";
                            }
                        }
                        else
                        {
                            if (currentmat.Length > 0)
                            {
                                Material newMat = new Material();
                                String newMatName = "";

                                string relativeLocation = Directory.GetDirectoryRoot(filename);

                                newMat = LoadFromString(currentmat, out newMatName);

                                mats.Add(newMatName, newMat);
                            }

                            currentmat = currentLine + "\n";
                        }
                    }
                }

                // Add final material
                if (currentmat.Count((char c) => c == '\n') > 0)
                {
                    Material newMat = new Material();
                    String newMatName = "";

                    string relativeLocation = Directory.GetParent(filename).FullName;

                    newMat = LoadFromString(currentmat, out newMatName, relativeLocation);                   

                    mats.Add(newMatName, newMat);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found: {0}", filename);
            }
            catch (Exception)
            {
                Console.WriteLine("Error loading file: {0}", filename);
            }

            return mats;
        }

        public static Material LoadFromString(string mat, out string name, string relativeLocation = null)
        {
            Material output = new Material();
            name = "";

            List<string> lines = mat.Split('\n').ToList();

            // Skip until the material definition starts
            lines = lines.SkipWhile(s => !s.StartsWith("newmtl ")).ToList();

            // Make sure an actual material was included
            if (lines.Count != 0)
            {
                // Get name from first line                
                name = lines[0].Substring("newmtl ".Length);
                output.MaterialName = name;
            }

            // Remove leading whitespace
            lines = lines.Select((string s) => s.Trim()).ToList();

            // Read material properties
            foreach (string line in lines)
            {
                // Skip comments and blank lines
                if (line.Length < 3 || line.StartsWith("//") || line.StartsWith("#"))
                {
                    continue;
                }

                // Parse ambient color
                if (line.StartsWith("Ka"))
                {
                    string[] colorparts = line.Substring(3).Split(' ');

                    // Check that all vector fields are present
                    if (colorparts.Length < 3)
                    {
                        throw new ArgumentException("Invalid color data");
                    }

                    Vector3 vec = new Vector3();

                    // Attempt to parse each part of the color
                    bool success = float.TryParse(colorparts[0],NumberStyles.Any, CultureInfo.InvariantCulture, out vec.X);
                    success |= float.TryParse(colorparts[1],NumberStyles.Any, CultureInfo.InvariantCulture, out vec.Y);
                    success |= float.TryParse(colorparts[2],NumberStyles.Any, CultureInfo.InvariantCulture, out vec.Z);

                    output.AmbientColor = new Vector3(float.Parse(colorparts[0],CultureInfo.InvariantCulture.NumberFormat),
					                                  float.Parse(colorparts[1],CultureInfo.InvariantCulture.NumberFormat),
					                                  float.Parse(colorparts[2],CultureInfo.InvariantCulture.NumberFormat));

                    // If any of the parses failed, report the error
                    if (!success)
                    {
                        Console.WriteLine("Error parsing color: {0}", line);
                    }
                }

                // Parse diffuse color
                if (line.StartsWith("Kd"))
                {
                    String[] colorparts = line.Substring(3).Split(' ');

                    // Check that all vector fields are present
                    if (colorparts.Length < 3)
                    {
                        throw new ArgumentException("Invalid color data");
                    }

                    Vector3 vec = new Vector3();

                    // Attempt to parse each part of the color
                    bool success = float.TryParse(colorparts[0],NumberStyles.Any, CultureInfo.InvariantCulture, out vec.X);
                    success |= float.TryParse(colorparts[1], NumberStyles.Any, CultureInfo.InvariantCulture,out vec.Y);
                    success |= float.TryParse(colorparts[2],NumberStyles.Any, CultureInfo.InvariantCulture, out vec.Z);

                    output.DiffuseColor = new Vector3(float.Parse(colorparts[0],CultureInfo.InvariantCulture.NumberFormat),
					                                  float.Parse(colorparts[1],CultureInfo.InvariantCulture.NumberFormat),
					                                  float.Parse(colorparts[2],CultureInfo.InvariantCulture.NumberFormat));

                    // If any of the parses failed, report the error
                    if (!success)
                    {
                        Console.WriteLine("Error parsing color: {0}", line);
                    }
                }

                // Parse specular color
                if (line.StartsWith("Ks"))
                {
                    String[] colorparts = line.Substring(3).Split(' ');

                    // Check that all vector fields are present
                    if (colorparts.Length < 3)
                    {
                        throw new ArgumentException("Invalid color data");
                    }

                    Vector3 vec = new Vector3();

                    // Attempt to parse each part of the color
                    bool success = float.TryParse(colorparts[0], NumberStyles.Any, CultureInfo.InvariantCulture,out vec.X);
                    success |= float.TryParse(colorparts[1], NumberStyles.Any, CultureInfo.InvariantCulture,out vec.Y);
                    success |= float.TryParse(colorparts[2],NumberStyles.Any, CultureInfo.InvariantCulture, out vec.Z);

                    output.SpecularColor = new Vector3(float.Parse(colorparts[0],CultureInfo.InvariantCulture.NumberFormat), 
					                                   float.Parse(colorparts[1],CultureInfo.InvariantCulture.NumberFormat), 
					                                   float.Parse(colorparts[2],CultureInfo.InvariantCulture.NumberFormat));

                    // If any of the parses failed, report the error
                    if (!success)
                    {
                        Console.WriteLine("Error parsing color: {0}", line);
                    }
                }

                // Parse specular exponent
                if (line.StartsWith("Ns"))
                {
                    // Attempt to parse each part of the color
                    float exponent = 0.0f;
                    bool success = float.TryParse(line.Substring(3), NumberStyles.Any, CultureInfo.InvariantCulture,out exponent);

                    output.SpecularExponent = exponent;

                    // If any of the parses failed, report the error
                    if (!success)
                    {
                        Console.WriteLine("Error parsing specular exponent: {0}", line);
                    }
                }

                // Parse ambient map
                if (line.StartsWith("map_Ka"))
                {
                    // Check that file name is present
                    if (line.Length > "map_Ka".Length + 6)
                    {
                        output.AmbientMap = Path.Combine(relativeLocation, line.Substring("map_Ka".Length + 1));
                    }
                }

                // Parse diffuse map
                if (line.StartsWith("map_Kd"))
                {
                    // Check that file name is present
                    if (line.Length > "map_Kd".Length + 6)
                    {
                        output.DiffuseMap = Path.Combine(relativeLocation, line.Substring("map_Kd".Length + 1));
                    }
                }

                // Parse specular map
                if (line.StartsWith("map_Ks"))
                {
                    // Check that file name is present
                    if (line.Length > "map_Ks".Length + 6)
                    {
                        output.SpecularMap = Path.Combine(relativeLocation, line.Substring("map_Ks".Length + 1));
                    }
                }

                // Parse normal map
                if (line.StartsWith("map_normal"))
                {
                    // Check that file name is present
                    if (line.Length > "map_normal".Length + 6)
                    {
                        output.NormalMap = Path.Combine(relativeLocation, line.Substring("map_normal".Length + 1));
                    }
                }

                // Parse opacity map
                if (line.StartsWith("map_opacity"))
                {
                    // Check that file name is present
                    if (line.Length > "map_opacity".Length + 6)
                    {
                        output.OpacityMap = Path.Combine(relativeLocation, line.Substring("map_opacity".Length + 1));
                    }
                }

            }

            return output;
        }
    }
}