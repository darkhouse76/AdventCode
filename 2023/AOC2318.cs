using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.Mathematics;
using UnityEngine;

namespace CodeTAF
{
    public class AOC2318 : MonoBehaviour {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;


        [SerializeField]
        private GameObject ground;
        [SerializeField]
        private Transform parentTarget;

        private List<List<List<GameObject>>> trench;

        private static Dictionary<char, Vector2Int> moveLookup;

        private Vector2Int negAdjustment; //for move the grid up when going negative


        private void Start() {
            trench = new List<List<List<GameObject>>>();
            moveLookup = new Dictionary<char, Vector2Int>()
            {
                {'U', Vector2Int.up },
                {'D', Vector2Int.down },
                {'L', Vector2Int.left },
                {'R', Vector2Int.right }
            };
            negAdjustment = Vector2Int.zero;

        }

        bool changeColor(GameObject obj, Color color) {
            Renderer[] renderer = obj.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderer.Length; i++) {
                if (renderer[i].material.color == color) { return false; }
                renderer[i].material.color = color;
            }
            return true;
        }

        void ClearObjects() {
            if (trench.Count > 0) {
                foreach (List<List<GameObject>> depth in trench) {
                    foreach (List<GameObject> row in depth) {
                        foreach (GameObject obj in row) {
                            Destroy(obj);
                        }
                    }
                }

                trench.Clear();
            }
        }

        Vector2Int GetBounds((char dirChar, int moveAmt, string hashColor)[] instructions) {

            Vector2Int neg = Vector2Int.zero;
            Vector2Int max = Vector2Int.zero;

            int MaxX = 0;
            int MaxY = 0;
            
            Vector2Int curPos = Vector2Int.zero;

            for (int line = 0; line < instructions.Length; line++) {
                for (int i = 0; i < instructions[line].moveAmt; i++) {
                    curPos += moveLookup[instructions[line].dirChar];                                        
                }              
                //check for lowest to adjust grid for neg
                neg.x = math.min(neg.x, curPos.x);
                neg.y = math.min(neg.y, curPos.y);
                //getting size of the grid
                max.x = math.max(max.x, curPos.x);
                max.y = math.max(max.y, curPos.y);               
            }
            negAdjustment.x = math.abs(neg.x);
            negAdjustment.y = math.abs(neg.y);
            
            return new Vector2Int((max.x + 1) + negAdjustment.x, (max.y + 1) + negAdjustment.y);
        }

        (char dirChar, int moveAmt, string hashColor)[] ParseInput(string input) {
            string[] eachLine = input.Split("\r\n");
            List<(char dirChar, int moveAmt, string hashColor)> output = new List<(char dirChar, int moveAmt, string hashColor)>();

            for (int i = 0; i < eachLine.Length; i++) {
                string[] tempSplit = eachLine[i].Split(" ");
                output.Add((eachLine[i][0], int.Parse(tempSplit[1]), tempSplit[2].Trim(new char[] { '(', ')' })));
                //print($"Move {output[i].dirChar} x{output[i].moveAmt} w/ Color = {output[i].hashColor}");
            }

            return output.ToArray();
        }

        private void CheckAddGround(Vector2Int curPos) {

            if (trench[0].Count < curPos.x + 1) {
                //add more x for each y
                for (int depth = 0; depth < 2; depth++) {
                    trench[depth].Add(new List<GameObject>());                    

                    for (int row = 0; row < trench[depth][0].Count; row++) {
                        trench[depth][^1].Add(Instantiate(ground, (new Vector3Int(curPos.x, curPos.y, depth)) + parentTarget.position, Quaternion.identity, parentTarget));
                    }
                }
            }
            else if (trench[0][0].Count < curPos.y + 1) {
                //add more y for each x;
                for (int depth = 0; depth < 2; depth++) {
                    for (int col = 0; col < trench[depth].Count; col++) {
                        trench[depth][col].Add(Instantiate(ground, (new Vector3Int(curPos.x, curPos.y, depth)) + parentTarget.position, Quaternion.identity, parentTarget));
                    }                    
                }
            }
        }

        int GetNumberForRowOLD(int[] rowXpos) {            
            Array.Sort(rowXpos);
            int totalCount = 0;
            if (rowXpos.Length % 2 == 0) {
                for (int i = 0; i < rowXpos.Length; i += 2) {
                    totalCount += (rowXpos[i + 1] - rowXpos[i]) + 1;
                }
            }
            else {
                for (int i = 0; i < rowXpos.Length; i ++ ) {
                    if (i == rowXpos.Length -1) {
                        totalCount += (rowXpos[i] - rowXpos[i-1]);
                        //last one
                        continue;
                    }
                    totalCount += (rowXpos[i + 1] - rowXpos[i]);
                }
            }
            return totalCount;
        }

        int GetNumberForRow(int[] rowXpos) {
            Array.Sort(rowXpos);
            int totalCount = 0;
            bool inside = true;


            for (int i = 0; i < rowXpos.Length-1; i++) {
                
                if (inside && rowXpos[i + 1] - rowXpos[i] != 1) {                    
                    totalCount += (rowXpos[i + 1] - rowXpos[i]) - 1;                                       
                }
                inside = !inside;
                //totalCount += (rowXpos[i + 1] - rowXpos[i]) + 1;
            }            
            return totalCount;
        }


        void part1() {

            ClearObjects();

            (char dirChar, int moveAmt, string hashColor)[] instructions = ParseInput(input);

            Vector2Int bounds = GetBounds(instructions);
            print(bounds.ToString());

            //bounds.x = 10;
            //bounds.y = 10;

            print("----------");

            const int preX = 1;
            const int preY = 1;
            const int preZ = 2;

            //Color testColor;

            //ColorUtility.TryParseHtmlString("#70c710", out testColor);

            for (int level = 0; level < preZ; level++) {
                if (trench.Count < (level + 1)) { trench.Add(new List<List<GameObject>>()); }
                for (int i = 0; i < bounds.x; i++) {
                    if (trench[level].Count < (i + 1)) { trench[level].Add(new List<GameObject>()); }
                    for (int j = 0; j < bounds.y; j++) {
                        trench[level][i].Add(Instantiate(ground, (new Vector3Int(i, j, level)) + parentTarget.position, Quaternion.identity, parentTarget));
                        //changeColor(trench[level][i][j],testColor);
                    }
                }
            }

            

            Vector2Int curPos = Vector2Int.zero;
            curPos += negAdjustment;
            int totalCount = 0;

            List<int>[] posOnY = new List<int>[bounds.y];
            for (int i = 0; i < bounds.y; i++) {                
                posOnY[i] = new List<int>();
            }

            for (int line  = 0; line < instructions.Length; line++) {
                for (int i = 0; i < instructions[line].moveAmt; i++) {

                    curPos += moveLookup[instructions[line].dirChar];                    
                    posOnY[curPos.y].Add(curPos.x);
                    totalCount++;
                    
                    ColorUtility.TryParseHtmlString(instructions[line].hashColor, out Color trimColor);
                    changeColor(trench[0][curPos.x][curPos.y], trimColor);
                }
            }

            int testNum = 2;

            print(GetNumberForRow(posOnY[testNum].ToArray()));            
            
            for (int i = 0; i < posOnY.Length; i++) {
                totalCount += GetNumberForRowOLD(posOnY[i].ToArray());
            }
            

            print($"Grand Total = {totalCount}");

        }

        

        void part2() {
            ClearObjects();
        }

        void Update() {
            if (run) {
                run = false;
                Debug.Log("========================================================================");

                if (useTestInput) { input = InputTest(); }
                else { input = Input(); }

                var startTime = System.DateTime.Now;
                if (partTwo) { part2(); }
                else { part1(); }
                print($"Took {System.DateTime.Now - startTime} to complete.");
            }
        }


        string InputTest() {
            return
@"R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)";
        }

        string Input() {
            return
@"L 4 (#906400)
D 13 (#01d963)
L 3 (#79a0e0)
U 10 (#98a913)
L 2 (#19c840)
U 3 (#47db23)
L 4 (#a84580)
U 4 (#5bac33)
L 6 (#58c070)
U 6 (#089513)
L 9 (#1dfa20)
U 2 (#3c5e23)
L 4 (#621d52)
U 6 (#bf9c73)
L 9 (#50eb22)
U 2 (#286a93)
L 3 (#72d9e2)
U 5 (#806033)
L 3 (#832ce0)
U 5 (#8e26b3)
L 2 (#30d5f2)
U 13 (#97a923)
L 3 (#5256f2)
U 4 (#154f63)
L 11 (#42ed72)
U 6 (#368651)
L 7 (#21fb22)
U 6 (#5ff031)
L 7 (#89bf92)
U 7 (#6a91c1)
L 7 (#89bf90)
U 6 (#a12501)
L 5 (#21fb20)
U 6 (#195221)
L 6 (#514db2)
U 6 (#a750f1)
L 8 (#3af182)
U 6 (#a750f3)
L 2 (#4dbe42)
U 4 (#4cd933)
L 6 (#ad8800)
U 4 (#390b13)
L 5 (#6d56f2)
U 3 (#279653)
L 3 (#94fa52)
U 5 (#31efe3)
L 4 (#45ce10)
U 10 (#4d3ab3)
L 7 (#7286e0)
U 9 (#280e23)
L 4 (#49fc50)
U 6 (#6debc3)
L 5 (#a72fe0)
U 3 (#36c9e3)
L 5 (#46f620)
U 4 (#448f13)
L 8 (#2ca942)
U 6 (#555aa3)
L 5 (#399532)
U 5 (#483083)
L 10 (#7cec72)
U 6 (#5ebe53)
L 3 (#686db2)
U 5 (#0040d3)
L 8 (#0e2ba2)
U 7 (#8803d3)
L 9 (#00bbc2)
U 6 (#8e6a03)
L 7 (#1120f2)
U 3 (#522df3)
R 6 (#512a82)
U 9 (#3f0631)
R 7 (#019a30)
U 3 (#b94321)
R 7 (#019a32)
U 3 (#6d6821)
R 3 (#a469d2)
U 5 (#4d6dc1)
R 7 (#824c22)
U 4 (#1f1d73)
R 3 (#2b5402)
U 3 (#9b1593)
R 10 (#28b092)
U 3 (#114f43)
R 8 (#595422)
U 7 (#506211)
R 8 (#014122)
U 2 (#aa8581)
R 2 (#7b5922)
U 9 (#0efde1)
R 3 (#39c632)
U 5 (#6e3a61)
L 4 (#7d8012)
U 4 (#159383)
R 4 (#15bc72)
U 6 (#6cb663)
R 7 (#444262)
U 6 (#52c351)
R 4 (#9800c2)
U 3 (#52c353)
L 4 (#012572)
U 7 (#1811b3)
L 6 (#5e8a32)
U 4 (#6f1fd3)
L 3 (#3df5c0)
U 11 (#6b3d23)
L 3 (#3295d0)
U 6 (#639293)
L 6 (#6208c0)
U 3 (#5a3581)
L 4 (#212042)
U 4 (#27a8c1)
R 3 (#212040)
U 2 (#4cf171)
R 9 (#0d87d0)
U 4 (#437a43)
R 7 (#719310)
U 5 (#2b2a33)
R 3 (#1bd8d2)
U 7 (#61c453)
R 2 (#1f4e80)
U 4 (#08b443)
L 9 (#9726c0)
U 6 (#2a7373)
L 6 (#57ba80)
U 10 (#653173)
L 6 (#13e8e0)
U 4 (#985921)
L 7 (#86f4d0)
U 5 (#85d8a3)
L 4 (#5bf682)
D 9 (#27b353)
L 4 (#09c1c0)
U 9 (#0ecf63)
L 6 (#a15350)
U 6 (#0ecf61)
R 7 (#2d3af0)
U 3 (#2673a3)
R 2 (#b2e500)
U 6 (#620613)
R 7 (#0f6bd0)
D 3 (#1c7543)
R 2 (#0a10c0)
D 6 (#59ae93)
R 3 (#9506a0)
D 5 (#37f853)
R 7 (#5e29f0)
U 5 (#597243)
R 5 (#a47a52)
U 3 (#140b03)
R 2 (#a47a50)
U 6 (#67b023)
R 10 (#0068b0)
D 6 (#0a4aa3)
R 9 (#3ab5c0)
D 8 (#79a163)
R 5 (#64c650)
D 6 (#44cad3)
R 6 (#51e700)
D 3 (#56c053)
L 9 (#09ce80)
D 2 (#1ea873)
L 2 (#73dff2)
D 9 (#a29b21)
R 8 (#2d4522)
D 5 (#a29b23)
L 6 (#4d00f2)
D 5 (#42ec53)
L 4 (#bc5c20)
D 8 (#21f071)
L 4 (#8a6f10)
D 5 (#5591f1)
R 3 (#0a3b82)
D 12 (#2e3e01)
R 4 (#9aa5c0)
D 8 (#8ea681)
R 7 (#9aa5c2)
D 5 (#602721)
L 8 (#0a3b80)
D 4 (#0f16d1)
R 2 (#a2deb0)
D 11 (#2d2ee1)
R 6 (#21a420)
U 6 (#5da871)
R 6 (#4f3d80)
U 7 (#c0da91)
R 3 (#4f3d82)
U 6 (#011b91)
R 3 (#41ff40)
U 2 (#4c1ec1)
R 7 (#af4f10)
U 2 (#06dcd1)
R 6 (#0ebc70)
U 4 (#0a9051)
R 7 (#653602)
U 3 (#6e9211)
R 4 (#606072)
U 5 (#39fc01)
L 10 (#c59670)
U 7 (#3ffb51)
R 9 (#063452)
U 3 (#61fc51)
R 4 (#6b83b2)
U 5 (#0b4801)
L 9 (#48e9b2)
U 3 (#a22491)
L 4 (#457ea2)
U 4 (#018051)
L 8 (#24bc42)
D 7 (#80bb31)
R 5 (#4c5392)
D 11 (#120631)
L 5 (#9f7850)
D 4 (#653a83)
L 9 (#c3ae60)
U 2 (#653a81)
L 3 (#0e0970)
U 4 (#67c3e1)
R 7 (#7395d0)
U 8 (#4c1161)
L 7 (#6cd100)
U 4 (#0d6c03)
R 7 (#388930)
U 8 (#220913)
R 4 (#415690)
U 7 (#5ea4a3)
R 8 (#5d8712)
U 8 (#9b9393)
R 5 (#5d8710)
U 3 (#002983)
L 4 (#7dc760)
U 8 (#1e52f3)
L 9 (#025a00)
D 8 (#1c1253)
L 3 (#b789a0)
U 3 (#1c1251)
L 4 (#1b78d0)
U 8 (#0bb5a3)
R 6 (#4d6930)
U 11 (#367da3)
R 2 (#c8f352)
U 3 (#44cc63)
R 8 (#c8f350)
D 3 (#5ac933)
L 5 (#207e60)
D 5 (#85afe3)
R 5 (#01d742)
D 6 (#902b33)
R 4 (#1e5032)
U 5 (#854b81)
R 4 (#6df0c2)
U 2 (#5e92e1)
R 7 (#64a4f2)
D 5 (#078fa1)
R 3 (#730962)
D 5 (#078fa3)
R 2 (#146262)
D 5 (#2b2f93)
R 11 (#1e8282)
D 2 (#b8aed3)
R 11 (#05b2d2)
D 2 (#902b31)
R 6 (#2500c2)
D 6 (#0af8a3)
R 4 (#778c32)
D 5 (#8ffe83)
R 6 (#4b59f0)
U 5 (#1bc403)
R 5 (#680210)
D 8 (#355103)
R 4 (#6f0f40)
D 3 (#4f8da3)
R 5 (#0193c0)
D 4 (#1236b3)
R 3 (#24a310)
D 7 (#7d9383)
R 3 (#a7aa90)
D 5 (#065c93)
R 3 (#59e070)
D 4 (#99e113)
R 6 (#28f8d0)
U 4 (#4c8283)
R 3 (#200c10)
U 5 (#2f3d13)
R 6 (#a0a790)
D 5 (#744603)
R 4 (#30a200)
D 8 (#82bdd3)
R 5 (#7b0d50)
U 5 (#15a7c1)
R 3 (#7410f2)
U 7 (#5308c1)
R 4 (#54de30)
D 9 (#74b971)
R 4 (#54de32)
D 3 (#355941)
R 3 (#7410f0)
D 6 (#1a9561)
R 4 (#5666b0)
D 3 (#7c2b23)
R 6 (#ac6ab0)
D 10 (#3c6181)
R 4 (#479482)
U 9 (#4b3c11)
R 7 (#5be122)
U 5 (#44f8a1)
L 6 (#9474a2)
U 3 (#7c8f11)
L 7 (#579ed0)
U 3 (#70fb11)
R 13 (#98b6f0)
U 4 (#16d6c1)
L 7 (#479480)
U 8 (#27cf11)
R 6 (#6a5de0)
D 2 (#a6cf63)
R 9 (#1c2240)
D 3 (#535b63)
R 4 (#ba5b40)
D 2 (#3a70a3)
R 5 (#1c07e2)
D 7 (#328f03)
L 9 (#90b982)
D 7 (#4e78e3)
R 3 (#29bc22)
D 3 (#4322e3)
L 2 (#409ea0)
D 3 (#19c8f3)
L 7 (#7b3e20)
D 4 (#925493)
L 5 (#2c7b40)
D 5 (#ac1d81)
L 7 (#081b90)
D 2 (#27a361)
L 4 (#1bf3d0)
D 5 (#295af3)
L 12 (#957dd0)
D 5 (#00e5a3)
R 4 (#432960)
D 2 (#a44e43)
R 8 (#0e2e60)
D 9 (#0711a1)
R 6 (#9e8d70)
D 6 (#68fac1)
R 3 (#9e8d72)
U 4 (#5e8271)
R 7 (#1c7e80)
D 4 (#27a363)
R 4 (#5a19c0)
D 7 (#069561)
L 6 (#334990)
D 3 (#069563)
L 5 (#67dd80)
D 9 (#7c2b21)
L 2 (#3a0ed0)
D 2 (#ad5d33)
L 7 (#1e84e0)
D 7 (#2edbc1)
L 6 (#74b590)
U 7 (#0e1491)
L 6 (#504080)
D 5 (#0e1493)
L 4 (#a9f120)
U 8 (#2edbc3)
L 8 (#251b40)
D 8 (#ad5d31)
L 3 (#5ed770)
D 5 (#3ae671)
L 7 (#061552)
U 4 (#009981)
L 5 (#0ca222)
U 9 (#23b031)
R 7 (#496292)
U 5 (#16ec01)
R 7 (#8ee312)
U 4 (#67a321)
R 4 (#24a272)
D 4 (#16c0f1)
R 4 (#1cc622)
U 7 (#7fcff1)
L 3 (#a7fd32)
U 5 (#789b91)
L 2 (#3358a0)
U 5 (#5322b1)
L 7 (#922a90)
D 10 (#835913)
L 3 (#603690)
U 4 (#835911)
L 7 (#3d4130)
U 3 (#078c81)
L 4 (#0ef3f0)
D 2 (#adc831)
L 3 (#6273f0)
D 11 (#39f881)
L 5 (#17c6a0)
D 4 (#b7d2a1)
L 6 (#17c6a2)
D 3 (#4a4ff1)
R 10 (#7d5902)
D 6 (#8aa763)
L 10 (#15f8e2)
D 6 (#0d8e93)
L 6 (#8b98e2)
D 8 (#428061)
L 5 (#2c9832)
D 5 (#579bb1)
R 5 (#453452)
D 4 (#5acad3)
R 5 (#3477b2)
U 3 (#3f5143)
R 7 (#5fb842)
U 3 (#6b4553)
R 9 (#3dcbb2)
D 7 (#247d53)
R 4 (#166552)
U 4 (#5e9e53)
R 3 (#765a32)
U 5 (#920223)
R 5 (#5c4fb2)
D 5 (#2609c3)
R 7 (#15bf50)
D 4 (#201523)
R 7 (#bcea90)
D 8 (#6309f3)
R 5 (#3e6cb2)
D 12 (#726c23)
R 5 (#536492)
D 5 (#1509d3)
R 5 (#808d92)
D 4 (#7fe721)
R 2 (#06eca0)
D 9 (#5caa21)
R 4 (#3105e0)
U 13 (#939171)
R 4 (#3105e2)
U 5 (#244431)
R 3 (#06eca2)
D 4 (#991be1)
R 9 (#8f3eb2)
D 8 (#3f4591)
R 6 (#4a8e22)
U 4 (#a8cd41)
R 2 (#430fb2)
U 7 (#33a231)
R 7 (#602992)
U 7 (#68bf81)
L 10 (#7ee6c2)
U 4 (#7cd521)
R 10 (#a42412)
U 5 (#1ee571)
R 2 (#039072)
U 12 (#516221)
R 4 (#60e3c2)
D 5 (#bb8851)
R 7 (#60e3c0)
D 3 (#81ab81)
R 5 (#8e80c2)
D 4 (#18e071)
R 5 (#943bc0)
U 9 (#6a5b41)
R 2 (#76ef70)
U 3 (#70c4c1)
R 5 (#7d7fb0)
U 5 (#a0cb51)
R 4 (#1ed780)
D 8 (#4f9261)
R 2 (#9bbcf0)
D 5 (#138771)
R 3 (#823170)
D 4 (#673221)
R 11 (#72fd30)
D 6 (#3354e3)
R 8 (#347150)
D 8 (#3354e1)
R 8 (#49ea60)
D 4 (#5baca1)
R 3 (#20a250)
D 3 (#55d6f1)
L 11 (#269a70)
D 6 (#514711)
R 7 (#641a30)
D 7 (#a25ba1)
R 5 (#641a32)
D 4 (#211b51)
L 8 (#8dbb20)
D 3 (#0efcf1)
L 4 (#02fe80)
D 4 (#4476b3)
R 2 (#296da0)
D 4 (#774ca3)
R 9 (#ad0dc0)
D 5 (#052723)
R 8 (#196740)
D 3 (#62d083)
R 6 (#4dc8c0)
D 4 (#9d8ee1)
R 3 (#4684a2)
D 11 (#749641)
L 3 (#778490)
D 8 (#170271)
L 6 (#6b1c40)
D 5 (#4bbdc1)
L 8 (#4d5bf0)
D 6 (#71d7b1)
R 4 (#363800)
D 3 (#3540b1)
R 4 (#6eed72)
D 9 (#6b9861)
R 2 (#2f1cc2)
D 2 (#0c9551)
R 5 (#a39e42)
U 5 (#0c9553)
L 4 (#248c52)
U 6 (#5cbe01)
R 4 (#4684a0)
U 3 (#49e411)
R 5 (#147d02)
D 8 (#22e8e1)
R 7 (#47c702)
D 5 (#6a7721)
R 3 (#6b0a62)
D 4 (#8d6003)
R 6 (#143e92)
D 10 (#3686f3)
R 7 (#594052)
U 6 (#7dcf03)
R 5 (#19e8d2)
U 7 (#2ac123)
R 4 (#7c8732)
U 4 (#59a753)
R 5 (#3e4872)
U 9 (#6e0623)
R 3 (#767cf2)
D 4 (#476623)
R 2 (#00e3b2)
D 5 (#799291)
R 8 (#4fb4f2)
D 2 (#7e14a1)
R 4 (#44e8b2)
D 3 (#6ff981)
L 10 (#03c9e0)
D 6 (#4c0a11)
L 4 (#03c9e2)
D 6 (#3a7fe1)
R 5 (#73a822)
D 7 (#782481)
R 6 (#1fced2)
D 8 (#5164e1)
L 7 (#18fa22)
D 7 (#00ac93)
L 5 (#0af5c0)
D 3 (#200ee3)
L 3 (#244172)
D 6 (#a5dc13)
R 10 (#244170)
D 4 (#129e13)
L 8 (#0af5c2)
D 4 (#266773)
L 2 (#64a1a2)
D 4 (#1bcf13)
L 7 (#5ade72)
U 3 (#74f681)
L 4 (#1c7b92)
U 6 (#95f3d1)
R 4 (#1c7b90)
U 5 (#1081c1)
L 2 (#600742)
U 4 (#a4da11)
L 8 (#56b352)
U 6 (#5dd011)
L 3 (#41b240)
D 6 (#142841)
L 6 (#298cc0)
D 6 (#1dee71)
R 8 (#2dfda0)
D 4 (#863f61)
R 2 (#7b0ee0)
D 11 (#2a7fd1)
R 4 (#70c8d2)
D 7 (#7ed441)
R 9 (#97faa2)
D 4 (#7ed443)
R 3 (#0b8812)
D 5 (#380fd1)
L 12 (#2bbaf2)
D 3 (#3797b1)
L 8 (#101d22)
U 3 (#bbd851)
L 9 (#101d20)
U 6 (#3f9731)
L 4 (#10fda0)
D 8 (#2bd171)
L 3 (#15fa90)
D 4 (#c30771)
L 4 (#15fa92)
U 8 (#971561)
L 3 (#10fda2)
U 4 (#09b3c1)
L 3 (#771bd2)
U 9 (#a74c31)
L 7 (#480c62)
U 7 (#31c003)
L 7 (#32b6b2)
U 7 (#4db183)
L 5 (#315702)
D 7 (#01ac13)
L 5 (#6d3812)
U 7 (#3c9253)
L 8 (#47c7a0)
U 3 (#396c93)
L 10 (#47c7a2)
U 3 (#589283)
L 3 (#25e122)
D 6 (#476671)
L 5 (#2520d2)
U 9 (#1658c1)
L 4 (#7905c2)
U 5 (#5ec5d1)
L 7 (#638e42)
U 8 (#9329f1)
L 2 (#367a42)
U 7 (#448cd3)
L 6 (#47c8e2)
U 7 (#53c891)
R 8 (#3e9d00)
U 7 (#757191)
R 5 (#3e9d02)
D 3 (#05c551)
R 2 (#386ca2)
D 10 (#7a87b3)
R 6 (#1c60d2)
D 6 (#5477c3)
R 5 (#5db422)
U 3 (#7cc273)
R 4 (#67ea02)
U 4 (#a53143)
R 7 (#204b80)
U 4 (#4a7073)
L 3 (#447c70)
U 3 (#72cad3)
L 8 (#7a4fa0)
U 5 (#72cad1)
R 6 (#3bcc70)
U 4 (#625903)
L 3 (#2cc5d0)
U 10 (#acc971)
L 3 (#1a8aa0)
U 3 (#2a9173)
L 3 (#a6cb80)
D 9 (#53f763)
L 3 (#a6cb82)
D 4 (#5fc703)
L 3 (#4338e0)
U 7 (#9e8b23)
L 7 (#4338e2)
U 5 (#8699f3)
L 10 (#159e12)
U 5 (#1b3de3)
L 8 (#1e5802)
U 6 (#06c193)
L 9 (#691102)
U 5 (#9e6e43)
L 4 (#32bb52)
U 7 (#8ce3a3)
L 6 (#0c5e72)
U 2 (#11c163)
L 3 (#20d7b2)
D 9 (#3aa213)
L 5 (#a25cd2)
D 4 (#397da3)
L 4 (#2b3e82)
D 3 (#2f1353)
L 8 (#503842)
D 5 (#6d25b1)
L 11 (#a55782)
D 5 (#6d25b3)
R 11 (#008022)
D 6 (#2e3033)
L 3 (#2574b2)
D 6 (#9d58f3)
L 6 (#841200)
D 4 (#296673)
L 2 (#c2b110)
D 5 (#5e07e3)
L 8 (#5b1262)
D 3 (#0c1b43)
L 8 (#27cf52)
D 4 (#0c1b41)
R 7 (#4b46b2)
D 5 (#4848b3)
R 6 (#2d7c52)
D 3 (#3818e3)
R 4 (#3c5792)
U 8 (#06a691)
R 7 (#18dc52)
D 8 (#c55b81)
L 3 (#2a8582)
D 5 (#2d58a3)
L 6 (#46f8a2)
D 9 (#9ea973)
L 4 (#375002)
D 3 (#56daf3)
L 4 (#7f15d2)
U 7 (#177923)
L 3 (#627e82)
U 5 (#713b83)
L 3 (#416152)
D 9 (#51edb3)
L 2 (#a3dfd0)
D 2 (#7b8423)
L 7 (#631ce2)
D 9 (#a54f53)
R 4 (#2be612)
D 3 (#282a71)
R 5 (#861152)
D 6 (#476751)
R 4 (#4683f2)
D 2 (#7d2b31)
R 8 (#1a7222)
D 3 (#51f383)
L 10 (#32f982)
D 3 (#6f40f3)
L 11 (#32f980)
D 4 (#2b8883)
L 9 (#54c132)
D 8 (#239bb3)
L 6 (#0ff5c2)
U 4 (#09a3f3)";
        }


    }
}
