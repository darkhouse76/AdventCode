using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEditor.Compilation;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace CodeTAF
{
    public class AoC7 : MonoBehaviour
    {
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;


        List<(bool isDir, string rootDir, string name, int size)> fileData = new List<(bool isDir, string rootDir, string name, int size)>();


        int GetSize(string dirName) {
            int totalSize = 0;
            for (int i = 0; i < fileData.Count; i++) {
                if (fileData[i].rootDir == dirName) {
                    if (fileData[i].isDir) {
                        totalSize += GetSize(fileData[i].name);
                    }
                    else {
                        totalSize += fileData[i].size;
                    }                    
                }
            }

            return totalSize;
        }

        string GetRootDirOf(string target) {
            for (int i = 0; i < fileData.Count; i++) {
                if (fileData[i].name == target) {
                    return fileData[i].rootDir;
                }
            }
            return "/";
        }       


        void Main() {

            
            string[] instructions = input.Split("\n");


            string curDir = " ";

            fileData.Add((true, "root", "/", 0));

            foreach (string instruction in instructions) {
                string[] instructionParts = instruction.Split(" ");                

                switch (instruction[0]) {
                    case '$':
                        //print("Command");
                        switch (instructionParts[1]) {
                            case "cd":
                                //print("Command: Change dir");
                                if (instructionParts[2].Trim() == "..") {                                    
                                    curDir = GetRootDirOf(curDir);                                    
                                }
                                else {
                                    curDir = instructionParts[2].Trim();
                                }
                                //print(curDir);
                                break;
                            case "ls":
                                //print("Command: List Stuff");
                                break;
                        }
                        break;
                    case 'd':
                        //print("Dir");
                        fileData.Add((true, curDir, instructionParts[1].Trim(), 0));
                        break;
                    default:
                        //print("file");
                        fileData.Add((false, curDir, instructionParts[1].Trim(), int.Parse(instructionParts[0])));
                        break;
                }
            }

            int resultTotal = 0;

            foreach (var data in fileData) {                
                if (!data.isDir) { continue; }

                int dirSize = GetSize(data.name);

                if (data.isDir && (dirSize <= 100000)) {
                    resultTotal += dirSize;                    
                }
            }

            print($"Result = {resultTotal}");


        }


        void Update() {
            if (run) {
                run = false;
                Debug.Log("========================================================================");

                if (useTestInput) { input = InputTest(); }
                else { input = Input(); }
                Main();
            }
        }


        string InputTest() {
            return
@"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";
        }

        string Input() {
            return
@"replace";
        }


    }
}
