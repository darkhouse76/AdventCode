using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeTAF
{
    public class AOC2309 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;


        string printArray(int[] target) {
            string linePrint = "";

            for (int i = 0; i < target.Length; i++) {
                linePrint += target[i] + " ";
            }
            return linePrint;
        }

        int[] NextSeq(int[] curSeq) {

            int[] nextSeq = new int[curSeq.Length - 1];

            for (int i = 0; i < nextSeq.Length; i++) {
                nextSeq[i] = curSeq[i + 1] - curSeq[i];
            }
            return nextSeq;
        }

        bool reachedZero(int[] curSeq) {
            foreach (int i in curSeq) {
                if (i != 0) { return false; }
            }
            return true;
        }

        void part1() {

            string[] history = input.Split("\r\n");
            List<int[]> readings = new List<int[]>();

            int grandHistoryTotal = 0;

            foreach(string line in history) {
                readings.Clear();

                string[] tempRead = line.Split();
                readings.Add(Array.ConvertAll(tempRead, int.Parse));

                do {
                    readings.Add(NextSeq(readings[^1]));

                } while (!reachedZero(readings[^1]));

                
                // now figure out the next one in history
                int lastResult = 0;
                
                for (int i = (readings.Count - 2); i >= 0 ; i--) {
                    lastResult += readings[i][^1];                    
                }


                print($"Last number = {readings[0][^1]} Next= {lastResult}");
                
                for (int i= 0; i < readings.Count; i++) {                    
                    string printThis = printArray(readings[i]);
                    for (int j = 0; j < i; j++) {
                        printThis = " " + printThis;
                    }
                    
                    print("\t" + printThis);
                }             


                grandHistoryTotal += lastResult;                
            }

            print($"Grand Total = {grandHistoryTotal}");

        }

        void part2() {

            string[] history = input.Split("\r\n");
            List<int[]> readings = new List<int[]>();

            int grandHistoryTotal = 0;

            foreach (string line in history) {
                readings.Clear();

                string[] tempRead = line.Split();
                readings.Add(Array.ConvertAll(tempRead, int.Parse));

                do {
                    readings.Add(NextSeq(readings[^1]));
                } while (!reachedZero(readings[^1]));

                // now figure out the next one in history
                int lastResult = 0;

                for (int i = (readings.Count - 2); i >= 0; i--) {
                    lastResult = readings[i][0] - lastResult;
                }

                print($"Last number = {readings[0][^1]} Next= {lastResult}");

                for (int i = 0; i < readings.Count; i++) {
                    string printThis = printArray(readings[i]);
                    for (int j = 0; j < i; j++) {
                        printThis = " " + printThis;
                    }

                    print("\t" + printThis);
                }


                grandHistoryTotal += lastResult;
            }

            print($"Grand Total = {grandHistoryTotal}");
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
@"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45
-1 -2 -3 -4 -5 -6 -7 -8 -9 -10 -11 -12 -13 -14 -15 -16";
        }

        string Input() {
            return
@"-5 0 14 35 64 120 276 742 2049 5429 13539 31751 70366 148419 300461 589269 1130567 2141638 4035732 7600527 14324252
12 32 65 127 242 435 720 1091 1536 2105 3067 5179 10050 20500 40670 75413 128164 196022 260149 268767 108978
26 37 57 95 169 322 641 1277 2464 4535 7933 13215 21047 32188 47461 67709 93734 126217 165617 212047 265125
6 8 7 4 19 116 441 1279 3141 6897 13980 26708 48816 86359 149227 253598 425820 708770 1173501 1943668 3250965
13 31 64 119 218 420 865 1848 3927 8067 15834 29709 53749 95172 168120 302040 559053 1067647 2084384 4101467 8026448
7 15 20 20 8 -28 -91 -132 55 1071 4425 13588 36058 87224 197290 423196 868407 1714695 3271682 6052020 10882740
24 36 57 102 199 398 796 1593 3205 6489 13197 26895 54800 111373 225166 451497 895180 1749936 3364455 6349710 11748819
8 21 64 159 335 628 1081 1744 2674 3935 5598 7741 10449 13814 17935 22918 28876 35929 44204 53835 64963
4 3 2 1 0 -1 -2 -3 -4 -5 -6 -7 -8 -9 -10 -11 -12 -13 -14 -15 -16
-2 -8 -19 -37 -61 -72 3 359 1424 4024 9613 20579 40637 75320 132579 223503 363170 571640 875101 1307179 1910423
9 24 65 151 321 649 1267 2411 4515 8386 15496 28427 51515 91789 160444 275405 466149 783012 1314923 2222135 3794381
18 26 45 86 160 278 451 690 1006 1410 1913 2526 3260 4126 5135 6298 7626 9130 10821 12710 14808
20 31 41 42 20 -46 -185 -435 -844 -1471 -2387 -3676 -5436 -7780 -10837 -14753 -19692 -25837 -33391 -42578 -53644
4 10 32 73 141 263 513 1069 2331 5160 11337 24391 51006 103289 202264 383051 702294 1248518 2156222 3624653 5942355
12 8 5 20 86 269 711 1709 3850 8245 16941 33647 65017 122958 228914 422094 775687 1429185 2652620 4972369 9412019
7 5 13 36 72 111 136 135 145 362 1365 4517 12622 30934 68632 140894 271723 497699 872853 1474882 2412948
11 27 46 60 66 71 100 213 534 1292 2883 6014 12145 24804 53069 119817 279549 654137 1500260 3330312 7120068
9 13 11 5 11 65 228 589 1270 2455 4505 8305 16176 34114 77075 181039 429559 1009877 2326667 5223261 11395377
11 20 49 113 243 494 967 1857 3545 6762 12863 24255 45021 81768 144697 248843 415359 672616 1056757 1611173 2384159
3 20 62 148 313 620 1176 2153 3817 6583 11169 19073 33938 65033 135227 297667 668177 1484736 3212577 6726497 13627371
2 8 29 83 194 391 717 1271 2316 4507 9335 19968 42844 90713 188453 384059 768926 1514158 2933409 5587997 10457038
13 19 36 66 122 257 604 1422 3150 6497 12650 23775 44147 82550 157199 305643 604363 1206707 2414172 4805664 9462912
17 23 29 35 41 47 53 59 65 71 77 83 89 95 101 107 113 119 125 131 137
5 27 67 131 240 441 829 1607 3226 6668 13971 29160 59861 120060 234755 447670 833795 1519329 2712683 4751601 8173238
10 6 2 8 50 191 573 1489 3494 7564 15312 29270 53246 92765 155603 252423 397522 609698 913246 1339092 1926074
6 7 22 56 118 232 462 960 2055 4421 9393 19542 39673 78474 151119 283214 516572 917411 1587688 2680412 4419920
5 11 39 110 269 598 1227 2338 4156 6925 10881 16275 23605 34455 53818 95654 194915 431635 976287 2170888 4667817
15 31 72 154 310 609 1180 2241 4133 7359 12628 20904 33460 51937 78408 115447 166203 234479 324816 442582 594066
16 17 27 72 198 484 1060 2133 4024 7210 12351 20280 31981 48742 73062 111731 184254 343515 723722 1648703 3873148
9 22 39 76 158 321 617 1126 1988 3484 6238 11717 23423 49565 108650 240435 526147 1123930 2328257 4665706 9044212
14 34 67 110 153 178 157 45 -236 -826 -1973 -4100 -7897 -14442 -25355 -42989 -70662 -112934 -175933 -267734 -398795
21 43 76 133 239 438 811 1522 2936 5897 12331 26479 57314 123118 259874 536170 1078847 2115807 4045414 7545985 13744225
-2 -4 3 34 120 326 791 1800 3894 8030 15826 29971 54964 98536 174590 309677 557717 1033351 1985614 3955285 8099943
7 22 65 161 342 642 1101 1799 2967 5263 10366 22162 49048 108420 235556 501476 1049127 2166504 4432687 9004183 18163794
5 20 55 125 250 461 813 1405 2407 4094 6887 11401 18500 29359 45533 69033 102409 148840 212231 297317 409774
16 28 53 113 246 521 1065 2102 4004 7354 13021 22247 36746 58815 91457 138516 204824 296360 420421 585805 803006
13 21 37 65 106 166 277 543 1229 2917 6759 14863 30854 60658 113563 203617 351429 586445 949777 1497669 2305690
8 16 42 106 256 587 1277 2653 5299 10215 19031 34273 59669 100472 163764 258690 396554 590690 856000 1208028 1661414
21 34 56 87 140 270 622 1518 3621 8241 17890 37256 74856 145751 275868 508681 915261 1609020 2766852 4658821 7689068
7 16 50 137 323 675 1291 2340 4175 7582 14257 27670 54649 108438 214915 425582 844720 1685215 3380413 6801646 13668084
20 42 88 175 334 612 1070 1792 2941 4937 8914 17773 38432 86345 194111 427156 911246 1879268 3748712 7247171 13611718
7 17 47 115 249 489 901 1611 2868 5146 9296 16760 29860 52176 89028 148078 240069 379719 586789 887345 1315235
7 15 27 36 39 43 68 145 307 571 909 1206 1203 423 -1922 -7045 -16721 -33449 -60641 -102840 -165969
-2 -6 -13 -24 -45 -92 -197 -403 -729 -1072 -981 818 7343 25003 66332 154284 328882 658070 1257741 2334072 4280561
13 39 85 164 295 503 819 1280 1929 2815 3993 5524 7475 9919 12935 16608 21029 26295 32509 39780 48223
7 6 19 73 220 550 1219 2513 4979 9668 18553 35207 65852 120920 217301 381491 653895 1094586 1790871 2867069 4496964
21 42 80 139 225 348 524 777 1141 1662 2400 3431 4849 6768 9324 12677 17013 22546 29520 38211 48929
6 0 -4 14 91 284 672 1357 2464 4140 6552 9884 14333 20104 27404 36435 47386 60424 75684 93258 113183
6 19 44 91 175 318 559 978 1735 3118 5587 9808 16731 27970 47288 85280 172173 389512 943080 2321483 5625634
-2 7 35 95 202 373 627 985 1470 2107 2923 3947 5210 6745 8587 10773 13342 16335 19795 23767 28298
5 20 64 147 290 539 974 1709 2879 4610 6968 9883 13044 15761 16790 14117 4697 -15856 -53632 -116845 -216314
13 15 14 19 47 135 362 875 1919 3887 7436 13768 25265 46813 88376 169716 328633 634759 1211826 2272489 4171275
0 -10 -19 -10 48 210 569 1274 2567 4881 9080 16973 32297 62439 121254 233435 441002 812600 1456431 2537792 4302350
15 41 89 174 316 537 871 1408 2402 4499 9212 19924 43988 96996 211165 451373 945373 1940497 3910279 7755302 15180528
23 32 40 62 126 287 661 1485 3219 6728 13632 27019 52922 103321 202013 395578 772951 1498896 2869088 5397676 9952270
29 43 68 131 273 543 994 1685 2705 4272 7032 12800 26157 57554 128886 282896 598261 1212809 2358028 4408865 7953785
16 31 57 94 142 201 271 352 444 547 661 786 922 1069 1227 1396 1576 1767 1969 2182 2406
0 4 15 45 118 279 621 1353 2954 6496 14279 31020 66025 137161 278253 553126 1082427 2094247 4017002 7643130 14403771
10 30 58 87 116 155 225 353 562 856 1200 1495 1548 1037 -529 -3855 -9910 -19982 -35738 -59289 -93260
18 36 80 158 288 522 992 1991 4102 8388 16656 31808 58292 102666 174288 286145 455834 706708 1069200 1582338 2295464
19 27 43 73 121 189 277 383 503 631 759 877 973 1033 1041 979 827 563 163 -399 -1151
6 24 56 101 155 212 281 438 944 2477 6548 16198 37105 79267 159469 304789 557450 981382 1670920 2762131 4447335
-6 7 29 55 80 99 107 99 70 15 -71 -193 -356 -565 -825 -1141 -1518 -1961 -2475 -3065 -3736
-3 4 23 64 159 388 920 2070 4373 8676 16249 28916 49207 80532 127378 195530 292317 426884 610491 856840 1182431
26 48 90 162 290 535 1021 1980 3827 7299 13752 25841 49051 94952 187677 376037 754968 1501738 2934622 5604684 10435000
18 24 30 36 42 48 54 60 66 72 78 84 90 96 102 108 114 120 126 132 138
4 9 22 56 145 368 896 2070 4516 9301 18132 33598 59453 100936 165122 261296 401340 600121 875866 1250508 1749985
18 34 63 104 164 279 556 1255 2939 6737 14810 31210 63506 125845 244538 467816 882076 1637696 2988277 5347872 9371252
25 40 55 70 85 100 115 130 145 160 175 190 205 220 235 250 265 280 295 310 325
22 46 87 157 273 467 814 1492 2906 5947 12528 26653 56443 117776 240507 478630 926238 1741740 3183517 5661053 9806573
14 21 25 26 24 19 11 0 -14 -31 -51 -74 -100 -129 -161 -196 -234 -275 -319 -366 -416
21 31 41 51 61 71 81 91 101 111 121 131 141 151 161 171 181 191 201 211 221
11 34 67 101 131 171 280 607 1472 3514 7956 17061 34882 68443 129527 237291 421977 730042 1231089 2027045 3264101
9 16 33 69 142 282 548 1085 2261 4936 10932 23824 50317 102821 204588 400354 778763 1519962 2996861 5989949 12138698
11 22 24 23 48 177 577 1558 3641 7640 14758 26697 45782 75099 118647 181504 270007 391946 556772 775819 1062540
20 34 62 111 197 364 726 1544 3353 7161 14752 29137 55210 100679 177354 302884 503042 814660 1289314 1997851 3035835
13 34 70 137 269 530 1033 1975 3699 6786 12161 21179 35670 58019 91636 142844 226765 383278 716720 1488777 3325167
0 0 13 62 187 466 1060 2308 4911 10251 20894 41341 79170 146994 266465 477534 860538 1587494 3035571 6025204 12295261
3 6 21 68 179 398 781 1396 2323 3654 5493 7956 11171 15278 20429 26788 34531 43846 54933 68004 83283
21 37 71 149 308 595 1065 1778 2795 4173 5959 8183 10850 13931 17353 20988 24641 28037 30807 32473 32432
-4 -11 -14 -8 8 38 106 281 705 1615 3345 6288 10791 16948 24247 31017 33610 25241 -5604 -77296 -217846
14 20 33 62 117 201 300 388 486 851 2431 7813 23021 60698 145438 322329 670134 1320982 2488973 4510728 7901645
7 16 32 64 143 346 831 1892 4051 8202 15802 29057 50970 85009 134065 198460 273423 348527 416774 512551 821405
8 26 62 143 325 711 1483 2961 5716 10791 20140 37517 70314 133427 257468 506216 1013378 2058823 4223638 8699196 17891585
14 27 49 77 122 231 522 1234 2801 5975 12050 23284 43692 80505 146763 265706 477748 850643 1492559 2565451 4291234
21 37 51 68 108 210 435 868 1619 2823 4639 7248 10850 15660 21903 29808 39601 51497 65691 82348 101592
5 15 39 92 217 503 1113 2335 4690 9174 17793 34693 68416 136151 271305 536280 1043007 1984718 3684337 6669765 11798939
17 34 61 107 190 343 621 1116 2014 3780 7649 16760 38549 89513 204330 452810 970591 2013310 4049689 7917165 15074976
2 17 38 58 74 105 217 562 1451 3504 7961 17305 36449 74882 150362 294995 564856 1054699 1919776 3407348 5901132
10 16 32 55 80 100 106 87 30 -80 -260 -529 -908 -1420 -2090 -2945 -4014 -5328 -6920 -8825 -11080
14 25 42 70 129 257 508 948 1658 2759 4480 7296 12169 20931 36854 65458 115614 201005 342014 568114 920841
9 32 64 119 228 442 833 1493 2546 4223 7123 12923 26047 57209 130370 295567 651367 1382466 2820298 5537559 10491414
10 17 27 46 91 205 480 1100 2431 5200 10820 21933 43258 82846 153859 277005 483776 820651 1354441 2178968 3423285
6 15 44 108 222 401 660 1014 1478 2067 2796 3680 4734 5973 7412 9066 10950 13079 15468 18132 21086
-3 11 39 81 136 202 276 354 431 501 557 591 594 556 466 312 81 -241 -669 -1219 -1908
11 29 72 164 352 714 1373 2533 4568 8213 14933 27596 51675 97408 183787 346236 652053 1232555 2351257 4548991 8950416
4 23 69 156 309 575 1036 1819 3093 5038 7766 11169 14664 16800 14687 3202 -26078 -86271 -196973 -386494 -694617
20 39 74 137 255 497 1025 2188 4684 9821 19918 38908 73247 133309 235573 406103 686108 1140771 1873082 3045131 4910247
5 5 18 52 115 215 360 558 817 1145 1550 2040 2623 3307 4100 5010 6045 7213 8522 9980 11595
20 38 64 113 211 405 791 1575 3206 6672 14150 30379 65420 139916 294577 606325 1214132 2357614 4430068 8043461 14093725
3 10 41 116 276 591 1158 2091 3513 5580 8608 13443 22316 40563 79765 163074 333735 668086 1294611 2420926 4370884
22 32 59 121 236 414 643 871 994 874 447 76 1522 10343 41319 129811 354006 876032 2014243 4364911 8999496
20 37 65 107 162 225 287 335 352 317 205 -13 -370 -903 -1653 -2665 -3988 -5675 -7783 -10373 -13510
-1 10 33 66 119 231 492 1068 2219 4298 7734 13045 21009 33253 53713 91683 167519 322506 634943 1245164 2393003
25 38 51 64 77 90 103 116 129 142 155 168 181 194 207 220 233 246 259 272 285
13 17 14 -2 -34 -64 -15 323 1428 4276 10709 23993 49627 96473 178286 315732 538991 891051 1431808 2243096 3434780
2 12 25 45 80 148 297 650 1498 3485 7959 17602 37500 76871 151735 288885 531602 947650 1640189 2762355 4536376
16 25 43 80 145 257 468 907 1873 4050 8996 20181 45043 98870 211976 442991 903842 1807439 3559349 6933306 13402625
20 36 60 92 132 180 236 300 372 452 540 636 740 852 972 1100 1236 1380 1532 1692 1860
11 33 77 154 279 474 766 1175 1682 2154 2190 859 -3640 -14211 -34495 -66886 -107083 -132113 -77045 206712 1021701
5 4 9 27 73 183 438 1015 2288 5017 10694 22170 44774 88262 170109 320888 592775 1072586 1901199 3301749 5619615
16 24 22 2 -33 -47 57 484 1644 4358 10287 22816 48870 102613 212843 437405 890548 1793652 3567576 6998514 13532869
-1 14 37 69 113 172 246 331 430 607 1162 3090 9122 25843 67653 163694 369321 784260 1580283 3042051 5625743
-9 -19 -23 1 82 250 538 1008 1821 3383 6640 13698 29162 63021 136762 296101 637088 1358818 2867932 5981172 12307763
-4 2 22 70 170 356 672 1172 1920 2990 4466 6442 9022 12320 16460 21576 27812 35322 44270 54830 67186
17 39 77 145 273 515 966 1811 3445 6728 13484 27444 56020 113673 228394 454374 897213 1764889 3471779 6850771 13580935
8 14 29 57 106 188 319 519 812 1226 1793 2549 3534 4792 6371 8323 10704 13574 16997 21041 25778
8 9 19 49 117 258 535 1049 1951 3481 6122 11102 21778 47029 108910 258883 610636 1404061 3126719 6740619 14102189
-3 9 32 69 128 230 422 789 1452 2530 4028 5594 6093 3039 -7778 -31454 -69585 -107681 -85960 160699 1016414
-9 -18 -22 -9 36 135 332 729 1560 3332 7079 14797 30155 59609 114083 211423 379877 662906 1125688 1863739 3014142
1 0 -1 -5 -15 -31 -46 -26 163 943 3419 10114 26247 61901 135743 281637 560901 1085774 2065075 3893101 7320909
-1 2 4 10 42 153 442 1083 2408 5130 10869 23278 50306 108562 231480 484202 990025 1976198 3851182 7331662 13646182
3 8 12 29 85 218 478 927 1639 2700 4208 6273 9017 12574 17090 22723 29643 38032 48084 60005 74013
-7 -11 -14 -18 -16 18 147 510 1405 3456 7930 17323 36471 74803 151234 305170 621227 1283493 2691004 5695870 12080284
-2 0 2 4 6 8 10 12 14 16 18 20 22 24 26 28 30 32 34 36 38
-2 -2 8 35 90 185 334 570 989 1840 3710 7918 17345 38101 82678 175573 362800 727258 1412596 2659029 4855524
3 8 20 62 173 419 912 1849 3601 6903 13223 25428 48928 93577 176756 328270 597971 1067382 1867050 3201902 5387515
7 18 29 40 51 62 73 84 95 106 117 128 139 150 161 172 183 194 205 216 227
3 12 34 84 194 423 881 1792 3635 7415 15126 30466 59844 113699 208199 367659 628833 1050280 1734760 2883234 4921924
3 19 56 138 301 589 1056 1792 2993 5088 8929 16065 29202 53189 97463 182275 354171 723147 1548666 3431878 7730359
2 24 67 142 260 439 721 1207 2124 3952 7685 15423 31767 67041 144413 314942 688257 1493639 3200070 6750835 14020637
18 28 38 48 58 68 78 88 98 108 118 128 138 148 158 168 178 188 198 208 218
16 23 23 27 65 197 534 1275 2767 5594 10698 19530 34222 57762 94143 148444 226786 336089 483537 675637 916735
6 13 36 99 250 571 1185 2273 4126 7279 12829 23163 43567 85624 174031 359590 742802 1514893 3026438 5898271 11195364
5 4 3 2 1 0 -1 -2 -3 -4 -5 -6 -7 -8 -9 -10 -11 -12 -13 -14 -15
14 11 18 45 97 184 350 742 1766 4421 10981 26352 60745 134907 290262 608350 1247756 2514043 4992682 9804071 19098431
5 18 44 94 190 373 710 1300 2279 3824 6156 9542 14296 20779 29398 40604 54889 72782 94844 121662 153842
-2 5 32 94 223 486 1008 2015 3924 7523 14308 27078 50945 95035 175425 320422 580342 1045750 1881018 3384525 6093571
16 21 38 90 210 439 821 1405 2277 3670 6250 11772 24477 53914 120401 265199 568814 1182859 2383846 4661438 8859436
23 37 51 72 126 276 647 1460 3093 6209 12028 22897 43472 83132 160782 314079 616467 1207397 2343929 4485790 8431154
15 25 43 73 127 245 533 1232 2837 6291 13285 26701 51241 94291 167075 286160 475379 768245 1210935 1865929 2816395
11 19 36 76 163 344 717 1480 3016 6048 11933 23225 44751 85687 163673 313233 603333 1173929 2310574 4592178 9173599
-5 4 42 139 351 776 1589 3107 5901 10984 20134 36490 65751 118730 216872 404005 770757 1504158 2986933 5995198 12092915
10 3 -1 7 36 101 245 600 1529 3918 9726 22958 51307 108816 220024 426135 793692 1425885 2475713 4158369 6756878
-1 3 22 78 201 424 777 1274 1889 2538 3139 3939 6527 16379 48532 139239 368459 901101 2059468 4445828 9143069
-6 -9 -12 -15 -18 -21 -24 -27 -30 -33 -36 -39 -42 -45 -48 -51 -54 -57 -60 -63 -66
5 5 7 26 91 257 636 1464 3223 6841 14009 27695 53017 98779 180198 323681 574977 1012661 1769739 3067232 5264943
14 24 40 70 125 227 421 791 1480 2714 4830 8308 13807 22205 34643 52573 77810 112588 159620 222162 304081
8 22 37 57 90 153 286 574 1171 2312 4288 7345 11451 15855 18339 14038 -6326 -58982 -170641 -383279 -760472
8 22 57 143 330 692 1335 2412 4149 6890 11176 17883 28457 45300 72380 116160 186966 300942 482771 769375 1214844
0 10 46 117 244 482 962 1964 4026 8083 15614 28754 50302 83525 131622 196671 277836 368560 452414 497211 446928
12 22 34 65 162 421 1010 2196 4376 8112 14170 23563 37598 57927 86602 126134 179556 250490 343218 462757 614938
9 16 16 15 35 116 319 734 1501 2865 5311 9864 18693 36227 71074 139129 268361 505878 927978 1653997 2864855
11 33 70 132 247 478 948 1872 3607 6767 12527 23383 44891 89381 183557 383698 805683 1683657 3483044 7114162 14326831
4 12 27 49 88 172 355 725 1412 2596 4515 7473 11848 18100 26779 38533 54116 74396 100363 133137 173976
11 9 1 -11 -21 -11 63 291 852 2128 4982 11348 25342 55172 116229 235991 462114 877236 1629756 3003892 5584242
3 17 42 92 196 419 898 1893 3853 7497 13910 24654 41894 68539 108398 166351 248535 362545 517650 725024 997992
17 29 42 58 88 169 394 955 2199 4697 9326 17364 30598 51445 83086 129613 196189 289221 416546 587630 813780
7 17 41 90 184 371 751 1502 2918 5493 10118 18496 33917 62563 115522 211668 381497 671881 1151495 1916366 3094566
-8 0 30 90 193 379 765 1647 3703 8397 18784 41105 87910 184079 378231 763951 1518554 2972578 5732162 10890944 20391199
1 10 39 111 265 560 1081 1961 3449 6072 10959 20417 38873 74322 140449 259623 466993 815950 1385255 2288171 3683977
12 34 86 190 378 695 1213 2079 3635 6677 12980 26328 54482 112842 231082 464857 915914 1765700 3329926 6144498 11096566
4 5 26 81 184 349 590 921 1356 1909 2594 3425 4416 5581 6934 8489 10260 12261 14506 17009 19784
11 37 81 159 308 600 1162 2218 4178 7807 14513 26791 48839 87306 152020 256350 416549 648969 963393 1349847 1755086
14 32 57 86 126 208 406 861 1804 3563 6527 11025 17060 23817 28840 26746 7314 -47246 -166075 -394768 -802110
16 17 29 80 228 590 1394 3059 6302 12260 22592 39475 65294 101575 146175 186661 185776 51411 -421749 -1658266 -4480164
7 20 45 80 131 231 478 1110 2649 6166 13750 29313 59949 118255 226502 424744 787768 1461973 2746132 5265591 10338528
7 16 48 110 202 311 405 431 316 -40 -805 -2336 -5451 -11953 -25542 -53291 -107907 -211048 -398022 -724254 -1273972
6 21 63 142 274 506 954 1854 3636 7056 13474 25474 48233 92436 180214 356705 711606 1417755 2797677 5434538 10351544
6 11 16 21 26 31 36 41 46 51 56 61 66 71 76 81 86 91 96 101 106
11 12 9 2 -9 -24 -43 -66 -93 -124 -159 -198 -241 -288 -339 -394 -453 -516 -583 -654 -729
30 46 70 113 192 329 551 909 1558 2983 6524 15452 36995 85958 191043 405895 828714 1637708 3155858 5969127 11139822
12 10 11 28 95 291 787 1937 4451 9725 20472 41925 84121 166229 324771 629369 1213352 2334346 4496248 8697167 16933171
0 11 41 112 256 511 918 1520 2365 3530 5230 8196 14789 31954 78507 202197 518070 1291813 3115159 7263852 16406302
23 45 71 96 119 150 218 388 802 1771 3981 8966 20186 45380 101406 223606 482929 1015705 2073195 4100964 7861865
7 21 53 125 273 544 1003 1764 3068 5450 10072 19372 38353 77266 157471 324597 678130 1436742 3078495 6632375 14266380
3 10 22 36 59 115 253 558 1161 2250 4116 7343 13396 26132 55282 124004 284770 652293 1471173 3253601 7056192
5 -2 -5 0 11 17 0 -40 9 637 3353 12068 35855 94171 226411 509089 1086619 2226821 4422279 8578011 16362741
23 53 99 177 327 625 1203 2287 4270 7854 14326 26080 47572 87053 159836 294938 549601 1042191 2026453 4054395 8319773
8 23 47 94 186 356 666 1246 2358 4483 8420 15390 27192 46637 78950 135934 247226 488685 1050567 2395376 5612452
13 33 71 133 237 437 876 1887 4170 9086 19126 38634 74888 139672 251505 438730 743707 1228399 1981689 3128819 4843399
-3 7 30 66 118 208 416 946 2220 5009 10639 21370 41147 77074 142175 260290 474319 859483 1543828 2738866 4784036
23 44 75 116 167 228 299 380 471 572 683 804 935 1076 1227 1388 1559 1740 1931 2132 2343
7 4 -5 -13 -6 31 118 307 745 1786 4165 9247 19364 38253 71608 127759 218491 360016 574111 889435 1343038
20 37 71 129 213 318 429 522 587 724 1428 4298 13638 39912 107078 268041 636883 1455016 3224274 6969068 14742308
7 17 28 45 83 178 407 931 2089 4592 9908 21012 43820 89849 180950 357332 690487 1302955 2397000 4294007 7485485
20 34 59 119 266 597 1281 2616 5157 9980 19176 36707 69804 131138 242029 436937 769342 1318778 2198117 3559043 5591810
5 19 41 65 76 58 13 0 210 1106 3692 10055 24489 55831 122244 260826 546670 1130624 2314790 4705488 9526049
14 27 40 53 66 79 92 105 118 131 144 157 170 183 196 209 222 235 248 261 274
14 28 56 98 154 224 308 406 518 644 784 938 1106 1288 1484 1694 1918 2156 2408 2674 2954
12 23 52 123 275 573 1124 2107 3848 7009 13036 25159 50512 104447 219076 459982 958945 1978549 4040720 8183046 16467532
15 13 10 23 97 323 876 2084 4534 9220 17746 32628 57823 99806 169925 289603 501650 894300 1651019 3150994 6166178
9 8 9 12 17 24 33 44 57 72 89 108 129 152 177 204 233 264 297 332 369
0 -4 -12 -15 10 100 306 693 1340 2340 3800 5841 8598 12220 16870 22725 29976 38828 49500 62225 77250
16 31 71 162 342 661 1181 1976 3132 4747 6931 9806 13506 18177 23977 31076 39656 49911 62047 76282 92846
18 37 83 167 313 571 1032 1858 3347 6072 11192 21182 41548 84709 178364 382766 825363 1769310 3748656 7835808 16171298
17 24 45 100 215 419 741 1207 1837 2642 3621 4758 6019 7349 8669 9873 10825 11356 11261 10296 8175
29 49 71 102 173 347 729 1480 2836 5148 9006 15605 27673 51555 101562 208786 439040 930116 1970638 4168097 8804402
5 17 49 129 313 696 1422 2709 4917 8704 15353 27446 50269 94749 183486 362736 725269 1452179 2886345 5656799 10882305
-3 -10 -4 30 107 242 450 746 1145 1662 2312 3110 4071 5210 6542 8082 9845 11846 14100 16622 19427";
        }


    }
}
