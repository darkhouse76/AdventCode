using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CodeTAF
{
    public class AoC11 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;


        class Monkey{

            private int TestDiv { get; }
            public int TrueMNum { get; }
            public int FalseMNum { get; }
            private Monkey ThrowToTrue { get; set; }
            private Monkey ThrowToFalse { get; set; }  
            public Func<int,int> Operation { get; set; }
            private List<int> Items { get; set; }

            public int Inspects { get; set; }


            public Monkey(int testDiv,int trueMNum,int falseMNum, List<int> items) {
                TestDiv = testDiv;
                Items = items;
                TrueMNum = trueMNum;
                FalseMNum = falseMNum;
                Inspects = 0;
            }            

            public void SetThrow(Monkey mTrue, Monkey mFalse) {
                ThrowToTrue = mTrue;
                ThrowToFalse = mFalse;
            }

            void Catch(int item) {
                Items.Add(item);
            }

            void ThrowTo(Monkey targetM, int item) {
                targetM.Catch(item);
            }

            Monkey TestItem(int item) {
                return (item %TestDiv == 0) ? ThrowToTrue : ThrowToFalse;
            }

            public void Turn() {
                //each item do opertation
                // check which to throw to.
                // throw to them
                for (int i = 0; i < Items.Count; i++) {
                    Inspects++;
                    Items[i] = Operation(Items[i]);
                    ThrowTo(TestItem(Items[i]), Items[i]);                    
                }
                //in theory all items are tossed
                Items.Clear();
            }
            public string GetItems() {
                string toPrint = "";
                for (int i = 0; i < Items.Count; i++) {
                    toPrint += Items[i].ToString();
                    if (i < Items.Count - 1) {
                        toPrint += ", ";
                    }
                }

                return toPrint;
            }
            

        }

        List<Monkey> InitExample() {
            //int the Monkeys aka manual parse the input
            List<Monkey> monkeys = new List<Monkey>();

            monkeys.Add(new Monkey(23, 2, 3, new List<int> { 79, 98 }));
            monkeys[^1].Operation = (int worry) => { return (worry * 19) / 3; };

            monkeys.Add(new Monkey(19, 2, 0, new List<int> { 54, 65, 75, 74 }));
            monkeys[^1].Operation = (int worry) => { return (worry + 6) / 3; };

            monkeys.Add(new Monkey(13, 1, 3, new List<int> { 79, 60, 97 }));
            monkeys[^1].Operation = (int worry) => { return (worry * worry) / 3; };

            monkeys.Add(new Monkey(17, 0, 1, new List<int> { 74 }));
            monkeys[^1].Operation = (int worry) => { return (worry + 3) / 3; };


            foreach (Monkey monkey in monkeys) {
                monkey.SetThrow(monkeys[monkey.TrueMNum], monkeys[monkey.FalseMNum]);
            }

            return monkeys;
        }

        List<Monkey> InitReal() {
            //int the Monkeys aka manual parse the input
            List<Monkey> monkeys = new List<Monkey>();
            //0
            monkeys.Add(new Monkey(19, 5, 3, new List<int> { 93, 98 }));
            monkeys[^1].Operation = (int worry) => { return (worry * 17) / 3; };
            //1
            monkeys.Add(new Monkey(13, 7, 6, new List<int> { 95, 72, 98, 82, 86 }));
            monkeys[^1].Operation = (int worry) => { return (worry + 5) / 3; };
            //2
            monkeys.Add(new Monkey(5, 3, 0, new List<int> { 85, 62, 82, 86, 70, 65, 83, 76 }));
            monkeys[^1].Operation = (int worry) => { return (worry + 8) / 3; };
            //3
            monkeys.Add(new Monkey(7, 4, 5, new List<int> { 86, 70, 71, 56 }));
            monkeys[^1].Operation = (int worry) => { return (worry + 1) / 3; };
            //4
            monkeys.Add(new Monkey(17, 1, 6, new List<int> { 77, 71, 86, 52, 81, 67 }));
            monkeys[^1].Operation = (int worry) => { return (worry + 4) / 3; };
            //5
            monkeys.Add(new Monkey(2, 1, 4, new List<int> { 89, 87, 60, 78, 54, 77, 98 }));
            monkeys[^1].Operation = (int worry) => { return (worry * 7) / 3; };
            //6
            monkeys.Add(new Monkey(3, 7, 2, new List<int> { 69, 65, 63 }));
            monkeys[^1].Operation = (int worry) => { return (worry + 6) / 3; };
            //7
            monkeys.Add(new Monkey(11, 0, 2, new List<int> { 89 }));
            monkeys[^1].Operation = (int worry) => { return (worry * worry) / 3; };

            foreach (Monkey monkey in monkeys) {
                monkey.SetThrow(monkeys[monkey.TrueMNum], monkeys[monkey.FalseMNum]);
            }

            return monkeys;
        }


        void part1() {

            //int the Monkeys aka manual parse the input
            List<Monkey> monkeys = InitReal();           

            const int TARGET_ROUNDS = 20;

            for (int i = 1; i <= TARGET_ROUNDS; i++ ) {
                for (int mNum = 0; mNum < monkeys.Count; mNum++ ) {
                    monkeys[mNum].Turn();
                }
            }
            //print all items after all rounds
            print($"All Current Items after {TARGET_ROUNDS} rounds:");
            for (int i = 0; i < monkeys.Count; i++ ) {                
                print($"Monkey {i}: {monkeys[i].GetItems()}");
            }

            int highestActivity = 0;
            int secondActivity = 0;

            print("All Activity numbers:");
            for (int i = 0; i < monkeys.Count; i++) {
                int curInspects = monkeys[i].Inspects;
                print($"Monkey {i}: {curInspects}");

                if (curInspects > highestActivity) {
                    secondActivity = highestActivity;
                    highestActivity = curInspects;
                }
                else if (curInspects > secondActivity) {
                    secondActivity = curInspects;
                } 
            }

            print($"Highest = {highestActivity}, 2ndHighest = {secondActivity}, Result = {highestActivity*secondActivity}");


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
@"replace";
        }

        string Input() {
            return
@"replace";
        }


    }
}
