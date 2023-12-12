using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeTAF
{
    public class AOC2312 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;

        string[] nums;

        void part1() {
            Regex regex = new(@"\W+");
            Regex tester = new(@"\s+");
            string[] bob = regex.Split(input);

            string[] house = input.Split("\r\n");
            string[] parts;
            

            for (int i = 0; i < house.Length; i++) {               

                string[] amountsString = regex.Split(house[i]);
                string springs = house[i].Split()[0];

                int[] amounts = new int[amountsString.Length-1];
                print(amountsString.Length);
                for (i = 1; i < amountsString.Length; i++) {
                    print(i);
                    print(amountsString[i]);
                    amounts[i-1] = int.Parse(amountsString[i]);
                }
                
                //string springs = regex.Split(house[i])[0];

                print(springs);               
                print(amounts[0]);

            }



            for (int i = 0; i < house.Length; i++) {
                parts = house[i].Split(" ");
                for (int j = 0; j < parts.Length; j++) {
                    nums = parts[j].Split(",");
                }  
                for (int j = 0; j < nums.Length; j++) {
                    //print(nums[j]);
                }
            }
            for (int i = 0; i < bob.Length; i++) {
                //print(bob[i]);
                
            }
        }

        void part2() {

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
@"???.### 167,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1";
        }

        string Input() {
            return
@".?#??.??#???.?? 2,6,2
?#??.?#.???#. 3,1,3
.#??#?#?.?##?#. 6,5
.#???##??.# 7,1
..#..??????#?#.#.#? 1,9,1,2
????#???????#????? 1,8,1,1,1
??#??#???#????????? 10,2,1,1
?#?.?##.?? 2,2,1
?..#.????.?#???#??? 1,1,1,1,8
????#??.#???? 4,1,1
????#.#.????? 5,1,2,1
?.??#????##????#.#? 1,1,4,3,1
.???..???####?? 2,7
??????????#.#???#?? 1,4,2,1,1,4
?????.??????? 4,3
?.?#?#??#?#????#??.. 9,4
..##??????#????##?? 3,5,4
?????#?##????#?.. 10,1
?##????#???#?.? 9,2
.??#.???????#?.???.? 1,7,3
????????###?#? 1,6
?#????#?.???? 1,1,2,4
?##?..?#.??.??#?# 2,2,1,1,3
?#?#?????. 7,1
.##?????#?????? 2,5,1,1
?...??????..?#? 1,1,1,1,3
?.#.?..?#?.? 1,1,1,1
??????###.???.. 1,3
?.?.?????###?.??#? 1,1,8,1,1
?#?????#??.??#??. 1,1,3,2,1
#?.?.??.????????.#. 1,2,6,1
?.?#????..?.?.?###. 3,2,1,3
#??#?.?#?#?.?#?????. 1,3,1,2,6
??.??#?#??## 1,7
??#??????#??????#?? 4,4,5,1
???#??##???. 4,5
????????.???##???#. 1,1,1,2,9
..#??..????????#.?#. 3,2,3,1,1
?#?????#?? 2,5
#??#?.???#.? 1,2,1,1
??#??.?.??? 4,1,2
.#.#.#??.???#?? 1,1,3,3
???#??#??.????? 8,4
?.??#?#?.??.#.? 5,1
???.?#?????##?#????? 1,2,7,1
??..???..??. 2,2,1
.????#??#?? 6,2
?.#.?...?#???#? 1,1,6
.?.#.????#?#??.?#.? 1,6,2
??#..#?????#??#???? 1,1,1,9,1
?????###??##??? 7,4
??????.??????#??#? 1,1,1,2,7
??#??????..? 1,4,1,1
??????.?#. 2,2
???#?.#?.? 4,2,1
#???#?#?????? 5,2,1
???#?#??.???..#.?## 8,1,1,1,3
??.??##???.? 2,5
?##??#?.#?. 6,2
??.?.?#####??#??? 1,1,5,5
?????##?##????.?# 1,7,1,2
?????##?#??##???#?. 2,9,1,1
???.????????#.. 1,1,1,2,1
.?????#???? 7,1
?????.#??#?#?#? 1,9
###????..#???#?? 5,1,2
???#?...??? 4,2
???.?.????#. 1,1,3
??#???...??????. 1,4
??#????..? 3,1
#.???#?#####??.???? 1,9,1,1
?#?#?????#???? 1,4,1,3
#?.#.?#???#?.??????? 1,1,4,1,3,2
????#?????##?#?#? 6,7
???#???.?#?##????? 2,6,1
.?#?????.??.????? 4,1,1,1
#..?#####????. 1,7,1
.?#.?#??.??#?? 1,1,1,3
??????#?.#?#?#?#?? 1,1,7,1
??#???.???. 5,1,1
.??#??????#???...#?? 11,3
??#?.?.?????##?... 2,7
?#?????###.?#???? 2,6,4
#..#.#??.. 1,1,3
.??.?????.? 1,4
??#?#????????????#? 4,1,5,1,3
?##?#??##???.? 5,2,1
?#????#?#???? 4,1,1,1
?.?#?#?.??#?? 5,4
.#.??#?##.? 1,6,1
?#.?.??????.#?? 1,1,1,1,2
?#??#????##..#.????? 1,7,1,1,1
?.??????#??# 1,4,1,1
.????##???#???? 1,8
??#?##?#?. 1,5
???#????????##.????? 3,2,3,2,1
?#???.???#????#???? 1,1,1,2,1,5
??#???????????#? 7,1,3
?.???#??#?.? 2,4
?#????#???#????? 3,9,1
???????.??##???. 1,4
???.????.???.?? 1,1,1,1,1
????#????.??#?#? 5,1,1,1,2
..??????.??????.?. 4,1,3
.#???##????#???#?? 1,1,3,1,3,2
.??..##?.???? 1,2,3
##?????????????#???# 4,2,1,1,1,1
###.??#??#???? 3,2,6
??#???.??????.# 1,1,1,2,1
????#?##??.??#??? 1,4,4
?##?#?#?????#??.#?. 9,3,2
???#?.??..?? 1,1,2,1
?#?.?.???#?.?????? 1,1,3,1,1,2
???####???#????.#?? 1,8,1,1,1
??????#?#??#.????.? 1,1,7,2,1
??????????..??.? 2,5,1,1
???#?#?.#??#????. 1,3,2,1,1
.??#???#?.???#???? 7,5,1
?????...?##?. 2,2
??#.##??????#???#?? 2,3,1,1,3,3
##?#?????#?.? 4,1,1
?#??.#??#?? 1,1,2
?.?#?..#???#. 2,1,1
.???.#?#???#???#..? 3,11
???#?.#?.?##? 4,1,3
?????.??.? 1,1,1
.????.???.??????? 1,1,6
.???????.?#?#??#???# 3,2,11
??#???#??.#???? 8,1,2
????#?#??.?.? 1,4,1
?#?#??#???.?????# 2,5,1,1,1
??#???.?#.. 4,2
.?.#???#?.??? 2,2,2
?.??..???#?? 1,3
???.????###?###????# 1,1,12
??##???????...#??.. 2,5,3
#?#?#?.???????????#. 1,3,2,8
???????#???.???. 1,6,1,1
?##???????.??????. 8,1,2
???##??.#? 5,2
?????###??????.##??? 1,9,1,3,1
..#.#?#??##.???..? 1,1,1,3,3,1
???###???#???????. 7,5,1,1
#?.??????##??#?. 2,10
?????#?.?? 4,1
??.??#?.????? 3,3
??#?.???????? 2,5
?##..??#?#??#?##?.?. 3,1,9,1
?.?????.???? 4,3
.?#??????.?#??? 3,4
?###?????????????##? 6,9
???#????#???. 1,5,4
#??#?.??.?.? 5,1,1,1
?.??#..??.#?? 1,1,3
##?##?##.? 2,5
?#??#??#?. 5,2
?#?..?#.#??#???##?? 2,2,9
???..#??..?? 1,1,1,1
?...?#.??.??? 2,2
???##????.???? 1,4,2
??.??..??.. 2,1,1
???#?????.? 5,1
.??????????#??.??? 1,1,1,5,1
????#?#????????#?## 1,2,2,9
.????#.?.? 1,1
???..?#?#???? 2,4,3
???#?????.#? 1,6,2
.????.???#?#.?# 1,1,4,1
?##???#???.?##?# 6,5
.??#.?????#?#?? 1,9
???.????##?#.???.?? 1,2,5,1,1
?..?#??#?#?####? 7,4
??##???#???.???.#??# 4,6,1,1,1,1
????.?#?#???#? 2,5
???...?#??? 1,3
??#?#?##??????##?#? 15,1
.?#??#??#???#???. 9,1,1
??????.#??#???.#.??? 1,1,1,5,1,1
??.?.???#?? 1,1,3
?#.??.?#??????? 1,1,2,2
????##?#??. 6,1,1
????.???????.?.??? 1,1
.??.??#??????#?? 1,10,1
?#?..?#???#??? 2,8
..???#??#?#?? 1,4,2
??????.#???#. 1,3,1,3
??##??.??. 5,1
????##???#.??#?#.?? 6,1,1,3,1
.#??#?##????? 1,8
..?????.??# 1,1
?#????.?#?#? 4,1,3
????????#???#?#??.?? 1,12
??#???.?.?????#???? 2,1,1,3,2,2
????.##??#????? 2,6,1
??????#?.????? 6,2
?.???#?#?#??#???#. 4,3,1,2
.????????.?#??????.? 3,1,7,1
??###?##?????? 10,1
..?.?#??#??. 1,6
??#?#??.?##?# 1,3,2,1
###???###???#..?##?? 10,1,3
???????#?? 3,3
??###??#??...?#..#? 6,3,2,2
??#?????#?????#.? 8,1,1
.?#???.???????#? 5,2,1,2
..?????.??.## 1,1,1,2
?#????????#? 3,3
#???#?.??.??? 1,1,1,2
?#.??.#?????.???? 1,1,4,1,1
?????????#??? 2,3
?..??####? 1,5
??#??#?##..##? 6,2,2
???#.?.??.???? 1,1
??????.?????##?#? 1,1,1,1,8
???#????.??? 1,1,2,3
??#?#??##?#?.?#. 10,2
##???#?.???? 6,1
..?.#????#?#?. 1,6,2
???????.?..?????? 2,4
#??#.#???##... 4,6
??.?.?????? 1,1,3
??.?.??.?#?#?####??? 1,8
..#?#??.?#?#???. 4,6
???.???#?#?#??? 1,8
.???.?.????.??.?? 2,1,4,1,1
?.?.?#?.?###? 1,1,4
#.??#?.##? 1,3,3
???#?????###???#?#?? 1,1,1,10
???.????????#?# 1,4,3
..???????#? 5,1
?????#.?????????. 2,3
?#????##.?.?.????. 7,1,1,1,1
?.#??#.??? 1,4,1
#?.??#???????##??# 1,4,1,1,3,1
???.?##???#??##????? 1,7,3,3
#????#??#?#??????? 2,11
???#?#??##? 1,8
??.???????#??# 1,6,1,1
??????????#.##????? 1,2,1,2,2,1
.?????????.???#??.? 4,1
..???#..??? 2,2
.?#????????#?# 1,1,5
?#?#??????##????? 7,3,2
??#??##?.???..??#... 5,2,3
?.?.????????.??? 1,1,5,2
?????#?##??.#??? 3,2,4,3
???..?#.?###??## 3,2,5,2
.???.??#??. 2,5
#???#.?#????#? 2,1,3,4
##.?????#?.?????? 2,1,3,1,2
?##????.??? 2,1
#.?#?.?????????# 1,1,1,7
??.?#??#?.?? 2,1,1,1
#???##?#?????.??? 3,8,1
?#?????#????#?.?? 5,2,1,1
?.?????????#? 1,1,1,3
.???????#?.?.? 1,1,3,1
?.???????#?#?#??? 1,2,6,3
.?.????????. 1,1,1,1
????#?.????.?? 5,1,1
?.??????#?..??# 1,1,2,1,1
??#??.??.???# 4,1,4
???.?#?#?#???.#?.??# 2,8,1,1,1
????.??#?????.? 4,3,2,1
????#?????? 3,1
?.?##?#???#??.???? 1,10,1,1
.?#?????????####?? 3,3,8
#?????.??.?#??? 6,3
.?#.?????????#?####. 1,4,8
???..?#???#???#???. 2,1,2,4,1
???.#?#?#???????.?# 7,2
?#????????? 1,1,2
#??#???#?.???.?.? 5,2,2,1
?????.?.?. 4,1
?.#?...?##? 1,1,4
??#?.??#??????#?#?#? 1,1,15
??#?..?.?? 2,1
#?????..?? 1,1,2
#??##??????#?.???. 8,1,1,3
?...????#?#?.?#??. 6,1
.???????.? 1,2
?#??????.??.??.#? 8,1,1
???.??..?#?#?. 2,1,4
????#?#?.#?????? 1,1,3,1,5
.???.???.? 1,1,1
?????#?????.?.#?? 3,2,1,3
????????.# 7,1
???#??.?.#?? 2,1
.?##???.??? 3,2
?.?##.??#????. 2,7
#??.?#????????#??#?? 1,1,11,1,1
???.?.?.?.?#..? 1,2
.????#?????#??.??? 12,1
?????#?..????##?# 2,1,6,1
?????..#?.?# 3,2,2
??.??????#.#?????? 1,1,2,1,1,2
???.#?..?..?????? 2,1
??##?#?????.? 6,2
?.?????#?#?. 2,4
?#???#???#???.????? 1,3,3,1,1,1
.#????#?.?.?#????#? 4,2,1,1,2
??#??#??#..??.?.??. 7,1,1
#??.??#???.##. 1,4,1,2
????.??????###..???? 1,2,1,7,1,1
?????##??#???????.?. 8,1
???.?#.?##??????# 3,1,7,2
??..??????.?? 1,1,2,1
????????.##????#? 3,3,2,1,1
??.?#????##?#? 2,6
.#?#???????? 3,1,1
???.???#?? 1,3
??????#???.???#????? 1,7,5,1
#?#.???.???# 3,2,1,2
?.???#??.??.?..?#??? 2,1,2
???.????#????##? 2,2,3,4
???#?????.?? 1,1,2,2
?#.#??..#?#? 2,3,4
???#?????#?#.????#?. 2,7,4
?????#??.????. 3,1
??????????????#?#. 1,1,7,1,1
??#.#??#????. 1,2,1,1
?.###?.?#.? 1,4,1
????##?#?.?????#?.## 2,2,2,2,3,2
????.#..?#???#? 3,1,4,1
??##??##??.??.?#??#? 8,2,5
??#??#???? 1,1,2
#??#..?.???????. 1,1,1,4,1
#??#?.??????#???#?? 1,2,10,1
#?##?#??#???##??#?. 1,2,5,3,1
?#??###..? 2,4,1
????????.?.?? 6,1
.?#??????? 5,1
.?#?????.. 2,1
?.????..?????##? 1,1,1,1,4
.##?.?????? 2,2
?????.##??##?#??? 2,2,6
#???.??#?????#????# 2,1,12
???.??#?????.? 1,1,4,1
????###???##?? 1,9
#.???.??#???# 1,1,4,1
..???#?????#??#. 1,2,1,4
#???##???#?#?????# 1,13,1
??#?#?.#???.##??.??? 1,4,3,4,3
?..??#????? 1,1,3
?.??#.???#.. 1,2,1,1
??????##???.???#. 1,6,3
????.#????? 2,3
.#?..#?#?#.. 1,1,1,1
.???#????##?????#? 1,2,10
.#.?#??#????? 1,3,2,3
?.???###?. 2,4
????????.?????? 1,1,3,1,1
????????##?#??? 2,7
?.????#.??#?# 1,2,2,3
.??##??????#???#???? 6,8
?#??#?#??.??#?#?? 1,4,6
#???????.??. 1,1,1,1
????.????..??? 3,2,3
?.???#??#.?.???? 1,1,1,2,3
?.#??#??.#???#?#???? 1,1,1,1,1,8
?###..#?????.?#? 4,1,3,1
??????#????????? 1,4,1,1,3
???????#???#??#?? 1,6,5,1
..??##?#?.???.? 5,3
#???.??#?.??#? 1,1,3,3
#?????????. 1,1,3
???????#???? 3,1
?????#??#???#??.? 1,8,1,1,1
??.???..??#?? 3,3
...????#??#?.??#?# 2,4,5
..#?#.?..? 3,1
#???.?.#??.??? 4,1,1,2
.?#.?.??.. 2,1,1
???????.??.??????? 4,1,1,1,3
??.??.????. 2,1,1
?.???????# 1,5
?????.????.. 5,2
??#?????#?.?? 2,2
#.?.?.??#? 1,1,2
.??#????.#.????#? 2,1,1,6
??#??#.????##? 1,1,3,3
??#??.?##???.??..??? 2,4,1,1,3
??????..?# 2,1,2
.???#??.##.? 3,2
?????#????.?.#?.# 1,7,1,1
??##??#????.# 5,3,1
?#?.?????#?# 1,2,3
.?#?.????#??#?#???# 3,6,1,1
..??####??#??..? 1,4,4
#.?#.??#.?? 1,1,3
???????#???????.? 4,2,1,1,1
??????.?.? 2,1
??????..#??? 4,1,2,1
?.?.?????##??? 1,1,6
?????#?#?#??? 1,1,7
?#????????? 1,1,2
??????#.?.??..? 1,2,1,1,1
.#?.?.#???.#?. 1,1,1,2
?..?##???.#.??#??# 1,4,1,1,6
.??#??##?????.?#?? 7,1,3
?.#???????.?.? 1,1,3
.???.?.#??.????? 1,1,3,1,1
.???????#?.??.#??.?. 6,1
#??.???#??.?.???#?. 1,5,5
??.???..?.??????? 1,2,1,1,5
#???#??#????.#. 1,1,1,5,1
????.?#???? 2,3
???##?#?#?? 2,3,1
??#?.?.#??? 3,1,1
?.??.???.??????##. 1,1,1,8
.?.#?????#?#???#?..? 1,1,7,1,1
#???????????#? 1,2,1,4
.???#??##??????.#?#? 10,3,1,2
???..??????#??? 2,6,1
.???????#????. 1,5
??????#.?#?##.. 5,2,2
??.???????#? 1,2,6
??#???#??#??????#. 12,1,1
#???#.#.????#?#??# 1,3,1,5,1
????.##??#?#?#? 2,3,5
.?###?.????? 5,1,1
???.????.?..????. 2,4
?.#?..???? 1,2
?#????#??##?#????#. 6,9
?.??#???.?.??. 1,4,1,1
.#????#??#??##??#? 1,10,3
.???#.??????# 1,1,1,3
?.?.?##?.?.???#? 1,2,4
??#????#??#???.?#??. 13,4
?#??.?????##?.??? 3,5
?#????#?#???.???#??? 2,1,5,7
??.??###????#?? 4,1
?#???#????.? 2,4,1,1
???.?##?###?. 3,7
#??##???#.????#? 9,3
???.#??#????#?. 1,1,1,1,3
#???#?##?.? 2,2,2
???##?????.?##.? 7,2
???###??#?? 5,2,1
??..???#..??#?????.. 4,7
???##??###.?##? 8,3
???.???#.?.?????. 1,4,2
???.????#?????? 1,1,1,1,1
?#?.?????####?.? 2,2,5
#??#?#?#???..##?? 1,6,2,2,1
##????.#?? 3,3
????.??#.?#.?#???.?? 1,1,1,2,5,1
?.#?????#?##??? 1,1,1,5,2
????#?.??..??.?.? 4,2
??#?##.??#??????.# 6,1,1,1,1,1
??.???#?.#.# 1,4,1,1
.?..#?##?#???. 1,1,2,3
???????.?????#?#. 1,3,2,4
?????..???.????. 3,3,2
#?###???.? 5,1,1
????.#.?..#????? 2,1,1,4,1
????#?#??#?? 1,1,2,3
????#.?.???? 1,1,1,2
???.#??#???????? 2,4,2,3
?..???????#???????.. 1,1,5,2,2
.?.???#?##?###?# 10,1
#??.?????##?#????. 2,12
????.#?.??.???#?#? 1,1,2,1,7
.??#????#?? 1,1
#??.?#???.??#???# 1,2,1,2,2
??#?????#???????. 1,8
???#???.????? 5,1,1
#???.???##???#. 4,1,2,1,1
?.????#.#???##? 1,1,1,7
??#?????##??.? 1,7
.?#.?.?#.? 1,1
.?###?##?#?#? 3,6
?????.??##.? 1,1,1,2
??????????. 1,1
?.?????#?#?#?..#? 11,1
??#???.##???. 3,2,2,1
???#?????????.?. 3,2
?#?#?#???..#????#?## 7,9
????.??.??? 3,1,1
#.?.#.?.?#????#???? 1,1,1,11
?##??##????#.?? 3,6,1,1
#?.#???.?#??????? 2,4,2,1
###.???#.. 3,1,1
.##???#..#? 4,1,1
???????.#?. 3,2
#??#???.?????????? 7,1,1,2,1
?#??#???????????..?? 10,3,2
#???.?.???#?????? 3,1,8
?????.?.?????##.?#? 1,1,1,5,2
#???.?????#????? 1,2,1,7
??.??.???.?. 1,1,1,1
.?.???????????. 1,8
????.#?#???.??? 1,3,1,3
???.????.????###???? 1,1,6,2
?.???????. 1,7
?????#??????.??.?? 1,1,3,1,2,1
.??????##.?? 8,1
?#???#?#???#? 1,1,5
#??#?#..?? 4,1,2
?????.??.? 4,1
.?????#???. 1,6
??#?##??#??##??#??#? 6,11
??#??##.????#?????#? 3,3,1,1,1,2
???.????????.?#?# 1,3,2,4
?###??.???? 5,2
?##???????.#??.#???# 10,1,2,1
.??.??????#??# 1,3,1
#???#?????#???# 1,6,1,3
?????#.?#?#????## 1,1,6,2
.?#?#??#??#?#? 2,2,7
??????##?????#?#?#?? 10,3,1,1
??.???.??#???? 1,2,4,2
????#.??..?#????.??# 1,1,1,1,6,1
??.#???#??#?#??. 1,2,1,4,1
?#?##??#.##. 4,1,2
.###?.?????.?? 3,2,1,1
??????.?.#??.???#? 2,1,4
??#??#?.#.#?????##.? 1,1,2,1,8,1
.#?#???.??# 4,3
?..##??..??.. 1,3,1
#???????.?? 4,1,1
?#??.???#??.? 4,2
????#?????#..?. 1,2,4,1
#??#??#??#.??##??# 10,3,2
?.???.?.#?? 2,1
#??.??#???????.?#? 1,1,3,1,1,1
?.??#?##???##.. 1,3,2,2
?#????#??? 2,4
??#???.?#?##??##. 1,2,1,6
.?.?.???????##?#???? 1,2,2,9
??#.???????? 1,5
???#??.?#. 4,2
.??.#??..##?.? 1,3,2,1
???.?..?#.? 3,1
#?.?#???#?. 1,1,2
????#.??##?????#?# 3,5,1,3
?????.?.??#? 1,1,1
??????????.? 6,1
?????#????.??##?? 1,6,1,5
.???#.???????##?#?#? 2,12
????#?###??#?.?##?? 1,6,1,5
??.?..??.# 1,1,1
????????#?#?..???# 2,1,5,1,1
.??..?##????? 2,3,1
??##??..?.?. 4,1
#..#.#??#?#?#??# 1,1,2,8
??#.#????#?? 2,2,1,1
#????##???#..?#????. 11,1,1
?#?.#?????????# 2,1,4,3
?#????.??..#? 2,1,2,2
.???#????? 1,1,2
?.???#????.#??# 1,5,2,1,1
#??##?#?????#?? 1,2,5,1,1
#???????.#?.#. 2,2,1,1
??.??.#?.????..#.? 2,1,1,2,1,1
???.????## 3,1,2
????#?#.???? 7,1
#?????#??##?### 1,1,10
#?.?#???????..#? 1,9,2
#??????#??????# 1,1,1,5,1
.???#??????? 6,1
?.?.????#?#####?# 1,1,2,7,1
?..???.?????# 1,1,3
?###?????????..# 7,1,1,1
??#.?#.??. 1,1,2
#?#..#??.??? 3,1,1,2
?.?.??.?.#?? 1,2,1,2
??#??.#?#??#..?.?? 4,1,1,1,1,1
??????##.??????? 7,2,2
???#?.???.#??#.?.?.# 4,1,1,2,1,1
?#??#?#?????.#???#. 9,1,1,2
?????#??#???.? 1,9,1
#?#?#??#??????. 1,8,3
?????.#???? 3,5
?.??.??..#? 2,1,1
#.?.?#???.? 1,3
?.?#?#??.##. 4,2
??.#..??#. 2,1,1
???????.#.?? 1,1,1,1
?.????#???##????#..? 7,7
???.?##?##?????? 1,2,3,3
???#?##??#?.???.???? 1,5,2,1,1,1
.?#?????#?????? 2,6
?????#.???. 3,1
???##?#??.#???#? 1,7,3,1
.??#?#??.???.#???? 2,1,1,2,1,2
?##??.?#????# 4,1,2
.#???#???.#???#??.? 8,1,4
?.?##?##??#????#?. 10,2
.?????##.???.?? 2,2,2,1
#????..?#?.#??? 5,1,2
.#?.#.???? 1,1,3
??#??????? 1,5
??????.???. 3,1
?#?##?.??? 4,1
#.???#??#??#??#?? 1,7,1,2,1
.?????.??# 3,1
?.??????.?.##???#??? 3,1,2,3
????#???.?.?.?.?#? 1,4,1,1,1,3
##????##??..#.??. 2,5,1,1,2
?#???????? 6,2
?#?##.?????##??? 5,2,1,4
.??##?.??.? 2,1,1
#?????????##?? 2,1,1,4
????.??##???.????? 4,3,1,3
?..??##??.? 1,4
.##?.??.?? 3,1,1
??#??#?#?.?? 2,3
?????#??.??#?.????? 2,4,1,1,1,1
??.???#??#?? 1,5
?#???.???# 4,1,1
?###?????.?#... 5,1,1
.???##??.?#? 5,3
???????.?...??.##??# 3,1,1,1,5
.???????##??##? 2,2,7
?#..#??#?.#?# 1,2,1,3
??????????.????#?# 2,1,1,1,2,4
??#??????.. 5,2
?#????#??#?#??.?.#. 3,1,3,1,1
#?#..?####???#?? 1,1,9,1
#??#??.?????? 1,3,5
??????????.???.??? 1,1,1,1,2,1
??.????#??#?? 1,1,1,3
??????.#?? 1,1,3
?.#????#???#????#?? 2,2,5,2
?#???.??.?. 5,1,1
???.??.?#???. 1,4
?????????.?? 3,1,2
.??#?..??#?? 4,3
??#.??#?.??????..? 2,1,1,1,2,1
??#???.??#??#.?#?#?# 3,1,6,2,3
.???##.?.???? 2,2,1
???#???##?#??#? 3,8
.???????????#?#? 1,1,1,1,6
?.?.?????? 1,1,1
?.#?.??????????#? 1,1,4,1,3
??#..???.???#?? 1,1,2,3
????????????? 1,3,4
???.?#?#.. 1,3
?#???.??.?. 1,1,1
.??????#???????. 1,6,3
.?#.??#????.#?#????# 2,4,8
???????.#?#. 2,1,1,1
??????.#??? 1,1,2
?.??##????#.?#? 9,2
?.??????#????.????.. 7,4
#?##?????#?? 1,3,3,1
?#??#??###??.#?? 5,3,1,1
????.#..??#???? 3,1,1,1
#??#?.#?#??#?..# 1,3,4,2,1
?????#???###.? 2,7
?????.?????????.#. 2,1,8,1
.#.????.?#??? 1,1,1,2
????.?????? 1,2,3
?#???#?##????#???.?? 1,9,4,2
????#???#?#??????? 12,2
.?#?????.#? 2,3,1
?..????.?.#??????#? 1,1,1,1,7
??????...?#??? 2,1,3
?????.#??#?.?#?# 1,1,1,1,3
#????#???#???#?.. 2,2,6
?.?.??.??.? 1,1,2
??.#????????#?????? 1,2,12
.##??.#????..###?. 3,3,4
??#?#????#???#?? 9,1,1
?#??.?.?#???#?#? 1,1,9
??##?#?#????? 1,4,3,1
.?????#?.? 1,1,1
.??????#????.?##???# 2,4,2,3,1
?.???#?????? 1,5
?..??.#.?????????.? 1,5
?#?.#??.???#???# 1,1,2,1,2
#??#???..????. 6,3
???#????????. 7,1
???????#??.?#??.# 1,7,1,1,1
??????#??. 4,2
??.??##????#??.#???? 1,5,3,1,1,1
.##?#????????##?.?? 4,3,3,1
.?..??.??.?.??? 1,1,2
.??????##.????#?#.? 1,5,1,5
..#??.##??.??? 2,4,1
.?#??##???????????. 9,4
#??.??###.??.???#? 2,4,1,3
?????????????? 1,1,1,5
.??????#?#????##? 4,3,3
.??..??##??.# 1,1,3,1
??..##??????#?#?.? 5,5,1
????###??#?..#?.?? 1,4,2,1,1
???.#?????? 1,5
.??#?????????.??.?? 1,9,1
?..#???.????? 1,4
???????.?? 2,1,1
#?..???#???#??? 2,5,4
?????.??????#??#??.? 2,1,2,2,2,3
??.?????.?. 1,3
??.??#???#??.#? 1,1,1,5,2
#.??#??.??#?.?.??? 1,3,1,2,1,1
?..???.?#???.#? 1,1,3,1
?.?.##.#??#??# 1,2,7
?#.??.??#?#??. 1,1,5
.?#?????.?##?#?#.??? 5,6,3
???#.??.?#??. 1,1,1,3
##?##?.??##????? 6,5,1
?##????###??#?? 4,7
#?#?##??#????????#? 3,2,11
..?????#?## 2,5
.??#?..?##????#. 2,2,3
????????#???????? 4,1,7,1
?#??#.?.?.? 1,2,1
#?.?#.?#?.?#?????? 2,1,2,4,1
?.??#.##??##??? 2,9
????#????..??#? 3,3,1,1,2
#????#??????## 1,3,6
?..????????.? 2,4
??#.?????.#?. 3,1,1,2
?????.??#????? 3,3,1
??..?#???# 1,1,2
???.?#?#?#??? 2,8
??????#?.#? 2,4,1
##.????#???????.?# 2,1,1,1,6,1
???##?????????## 2,5,2
?#?#?????????????# 6,4,1,1
#??..?##?###??.? 2,9
?.?????#???.. 1,2,1,1
???.??#.?????. 2,1,2
???????.?? 1,1,1
???#?.??#???.??#.? 5,6,1
.?????#?#??.#.?? 1,1,5,1
???????????.. 4,1,2
????#?.?????###????? 1,4,1,5,3
?????#????????..?? 1,7,1,1
#?##?.???. 5,2
??#???#???#???????# 1,9,1,1
???#??#???###??##? 5,7
.??????#?. 1,3
??.?????#??????.?#?? 2,8,1,1,1
?.#???#??#?#??.???? 12,1
????#.???#.#?????#?. 2,1,3,3,3
?#.???.??.#??#?.? 2,1,1,5,1
??..?#??.????#?##?? 3,9
#???#???????#?..? 9,3,1
.??#?????.?? 3,4
?????????#???.? 4,1
?.?.??.????#??. 1,1,2,4,1
.##?????????.???#? 3,2,1,4
..#????.##???? 1,1,5
??##?.#.?. 3,1
.?.???#?#?..?.????? 3,3
??????????#?#?????? 13,1
??#????????.# 2,2,1,1
.??..#??#.?#. 1,4,1
.????##??#??? 1,1,7
?#???#??????#? 1,3,3,2
.?????????????? 2,1,7
??#???????.? 3,2
#??#?.?????##?#? 2,2,2,2,2
#?##.??.?##?. 1,2,1,2
.??????#??#?.???? 1,6,1,1
.???#???##.?.?##..? 9,2
#???#..???### 5,4
.?#?.?#??.??#.?? 3,2,2
..#?#..???##? 3,1,3
##??##???##.??? 2,4,2,1,1
??##?.#????..????. 4,3,3
.??#?????.?.???# 3,2,1,1
.??#?????????????##? 3,12
??.???#.???..#?# 1,2,2,3
????.???#.? 1,1,3
?#.????#?? 2,5
?###???#????????? 4,3,1,3
???.????##?????#??? 1,1,7,1,2
???.?.????? 2,3
???.???#?# 1,4
????.#?????#??? 1,3,2,2
????#??#??#?#.?..? 1,1,1,7,1,1
.?.#?##?.?????#??. 1,4,1,4
?.?#??.?.?. 1,3,1
?.?#.????#??#??##?#? 2,13
.?#??#??#? 3,4
????#??#???. 3,1,4
?.???#???.? 4,1
???????#?#? 1,2,5
?????#??#???..?? 3,5,1,1
###????.??????? 4,1,1,1,1
.??????#?#??#????#?? 7,6
..?.??????? 1,1,2
.???.?.#??#???#???.# 1,1,5,4,1
???.#?#??#???????? 1,4,1,1,2
????#?#?#?##???#.? 14,1
##?#??????????#?#. 4,1,9
?????#??#. 1,1,1
.??????#???.?? 6,2
?????.??.#.#???????? 3,1,1,1,7
??.#??.###? 1,3
?#?#?#??#? 5,2
?.???????? 2,3
???.#?.????? 1,2,1,1
.????..?#? 2,2
????.?#????? 2,5
.??#?.?????????#? 2,3,6
??????.????##?#? 1,6
#??#??#.?.???????? 1,1,1,1,8
???.#???????? 3,3,1,1
???##???#??#? 6,1,1
.????#???#.? 1,6
????...#??#?????#?.? 3,10
.????#?????????.?? 8,1,2,1
??????.????#?. 2,3
...?#??????#??? 2,6
?#????????###??????? 6,10,1
#?#??#??.?????#??#? 8,9
?.#??...#?? 1,2
????##.?#.# 6,1,1
?.#??..?## 2,3
???.????.????? 3,2
???????#???#??#???#? 1,2,1,1,1,6
???#??????.?. 4,1
#??##??#####??#??? 13,3
??.?.#?????#.????#? 2,4,2,3,2
#?????##?????? 1,1,4,2
#?????#?#???#?.###. 3,6,1,3
.?#???????. 1,1,2
????#??#?#..##?..? 7,3
.???##..?##??? 5,2
???#?##??#?#? 1,2,7
#####????.? 5,2,1
##??#?.???#? 2,2,1,2
?##?#.#????.??#? 4,4,1
???????#?.? 1,1
#????????#?#? 6,4
.?.??.?????#? 1,1,1,5
.?#?##.?????#? 5,4,1
..???.??#.. 2,3
##?#?????.?.???? 4,1,1,2
???#????#??? 5,1
?##??.?.???.#?. 4,1,1,1
.?#?.???????#?##. 1,4,5
####??#???????? 5,2,1,1,1
.###?.?##???. 4,5
??#?##????.??#?.? 10,1,1,1
?#????##.???.#??#.?? 8,1,1,1,2
#??.?.??#??#??..??? 2,1,1,4,1,1
??.?#?#?????. 5,2
????##..#?.?????.? 1,4,1,2,1
?#???????#??#??. 5,5,2
??????????#???????? 1,3,2,1,1,2
.?#??.???..? 1,1,1,1
??????###?.??.?# 1,1,3,1,1
??????.??#???? 3,1
?##..#..?? 2,1,1
.??..?..???? 1,1,1,1
????????????????# 8,2,1,1
?#?.????#????? 3,9
?????.?#??.??#?? 5,3,3
???#.?#???#??#???? 3,10
.?..???#?? 1,1,1
?#?..??.#?. 3,1,1
???.??#????.?????? 1,1,5,1,1,1
??#..??.#????????.? 1,1,1,5,1,1
?????????#??##???? 6,8
..???.?#????????? 1,1,6,1
?..?#??#??????. 1,7,2
???.#????#??????.# 3,1,1,1,4,1
??.????.?#?.??#????? 1,1,2,8
.?.??####?#???.. 5,3
??????.?#? 1,1,2
.??..????? 1,1,1
.???##??#?#.???? 9,2
??#?????#???# 6,5
?.????#???????##. 1,10
????#?.#???? 4,3
????????##? 2,3,2
#??#???????#? 1,3,2,2
?.???##??#?.??##?? 5,1,5
????????????##? 3,2,1,3
??????????##..??? 5,5,2
.##?#??????#???# 5,7
?????.##???##??? 3,10
??#.???#?????? 1,5,1
???????#?????.?# 4,2,2,2
???.????#?#?#?.?? 1,1,1,5,1
#??#?????.#?.?? 2,2,1,2,1
?.##??..???#??# 1,2,1,4,1
?##?.?..??..?? 3,1,1,1
?.#??????#??#? 2,1,6
?.#???.?.?.#??? 1,4,1,1
???.?????#? 2,1,2
?##??#????? 5,3
#?.?????.????. 1,3,3
?.???????.#.? 1,2,2,1
??.?#?????# 1,3,3
...?#??????????? 2,1,3
?#??.#???#????.?.? 2,5,3
.????#??#?#????# 1,6,2
.#???##????????.???# 11,1,1,1
???????????.??? 1,1,2,2,3
??#??.??????? 2,4
???#??.?#??????? 1,1,1,3,5
#?#?#???###?????.?? 1,10,1,1
?????##.##??? 1,3,2,1
???????#?#???#. 2,7
???#.?????. 1,1,2
??.?#.????#?????#?? 2,1,10
?#.???.???.##? 1,2,2,2
??#????????#??? 5,6,1
????#??.??? 4,2
??#?#?????##? 1,1,1,3
?#?.?.?#???#??????? 1,6,1,1,1
???##????#?.??? 9,1
??.???.???.???? 2,1,3,3
??#???#.???? 7,1,1
??.????#.?. 1,2
??????#??##??? 2,2,3,1
?#??.?#?.?.??????.?? 2,1,2,1,3,1
?????##?.#??? 1,1,3,1
?.??#???.#? 3,1
#?.???.?###??####.?. 2,3,10,1
?.#????#.?#????? 1,4,1,2,1
.?????.???#..#.?? 1,1,4,1,1
???#??#??.#???. 7,4
??##??????????? 6,5
??#??.??#??#????# 2,6,1
.?#???.?..??#. 3,1,1,1
?#??????#.?#?.?.?# 2,2,1,1,1,1
?.#?.??.????## 2,1,4
??.??###???????????? 1,13,1
?#?????#?.?##? 2,1,2,2
????#?.?.?.##?? 4,1,1,2
#.?.??#??? 1,1,2
??????.???#?? 3,3
???.???##? 2,1,3
..?????.?.?#?? 2,3
??.?#??##?##??#.??# 2,1,6,2,2
??#?????.?#??#??? 4,1,4,1
##???????????.??? 2,8,2
?.????..???.?? 2,1
????.????#??? 1,1,2,1
??????#?#?????#?? 6,1
????#?.??????? 3,1
.?#??##?.#???? 7,2,2
???#?#.??#???????. 1,1,7,1
???#?????#. 2,2,1
#??????#.?????#??.? 3,1,1,1,4,1
?#??????.????? 2,2,3
????????#???????.? 2,6,4
#?.???.??.??#??? 1,1,1,1,4
?????????#.?# 1,5,1
????#??#?????# 2,1,6,1
.?????#?????#????? 1,3,1,1,1,1
#??#??.??#????? 6,2,3
??.??????.??. 2,6,1
?#??#???###...#?? 5,3,2
?#?#..???????????? 2,1,4,1,1,1
????##????.??#????. 4,5
??#?.???.?#??? 1,1,1,4
?????.??.????#.# 3,2,1,2,1
?.????????? 1,6,1
#??##????#???? 1,8,1
?#??.??##? 1,1,3
.????#???????.?#?# 1,3,1,2,1,1
???###????#?? 8,1,1
??#????.???# 3,3,3
.??#?????.?.??.??? 3,2,1,2,1
.#???#?##???.??#.?? 1,1,4,1,1,1
##?????#????? 8,3
????.??.?? 1,1,1
?.???.???##?#??#? 2,9
??..????#???? 1,2,3,1
.???#????#?.?????..? 8,1
.?????.?#?. 1,1
#????##????? 7,1
??????#.#?#?#?? 5,5
.#???.??.#?????#??. 4,2,8
??#?#?#?..???#??? 1,6,5,1
?#?.#?.????. 1,1,3
#?????#??#?##??.?#?. 14,3
??..??##??. 2,5
..#????.#?.????? 4,2,2
##?#??#.?# 7,1
?#?.?##?#? 1,6
.?#???.?#???#? 4,7
?..?????#####??.??? 1,2,7,3
?????#????#?????.?? 8,1,1,1,1
???????##?.??? 7,2
??.???#?##?#?.??? 1,8,2
?????##??? 2,3,1
?#.?.??##???????#?? 2,5,3
?#??????.??#?? 8,3
?#?#??#????.????.? 4,4,1
???##????# 6,1
??????????? 2,5
??????.???.#????##? 3,1,7
?#..?.??#??#??? 1,1,3,2
.?.???#..?#. 1,1,2,2
??.#??.###? 1,1,4
?##?##???? 7,1
?#??#.????..?##? 1,2,3,4
..?##???#????#..#? 3,4,2,1
?.#.??????.#?.#???? 1,1,3,2,3
#?#??????? 3,4";
        }


    }
}
