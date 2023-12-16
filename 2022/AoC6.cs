using UnityEngine;

namespace CodeTAF
{
    public class AoC6 : MonoBehaviour
    {
        [SerializeField]
        private bool run = false;

        string signal;
        [SerializeField]
        int markerLength = 4;

        bool IsMarker(string signalSection) {
            int charCount;            


            foreach (char c in signalSection) {                
                charCount = 0;
                for (int i = 0; i < signalSection.Length; i++) {
                    if (signalSection[i] == c) {
                        charCount++;
                    }
                }
                if (charCount > 1) {
                    return false;
                }
            }

            return true;       
        }


        void Main() {
            signal = Input();


            for (int i = 0; i < signal.Length; i += 1) {
                if (IsMarker(signal.Substring(i, markerLength))) {
                    Debug.Log($"Marker Found at {i + markerLength} the marker string is {signal.Substring(i, markerLength)}");
                    return;
                }
            }
        
        }


        void Update() {
            if (run) {
                run = false;
                Debug.Log("========================================================================");

                Main();
            }
        }


        string InputTest() {
            return "replace";
        }

        string Input() {
            return "tnmmpfmfzmmnsmsjmjjbvvhnhzzfmmgpmgpgbgnnwffjhffzqqmzzbnbssrqqrnnhsnngsszsqzszhzfhzfzwzfzrrmhmghgwhhjjqwqttwhttjllrtrtzzcfzfgzznfznfzfnnbddvmvzmmfsmfsmfffhlfldlqqrnrznnhmmgqqzhhmjhmhppqbpbbngnlldvvdqvvrtrdrtrnttnppfllrbbrprpnpdplpmllhwwddqpdprddzzfccqpcqpcpcbbdhdjdjwjcwcctdcttzgzmmscmsmdmttwhwzhhnjhnhlhvhlvlglpgpmmjmgmrgrddmwddjfftfwflfslffqtfqttpftppflfmmhvhvcvbvhbhggpbgbppvdpvpvfppbwwsnnhphllbdbnbvbmvvzffvsffdldmlmtmccnlnbnjbnjnhhbfhhgzzlwlfflzffdccggdcgcjjhffjfgfgcczjccvwcvvqgvvqvllqzqmqllhjjqnqggttsdddjgdjgjzzrgrfrbrssrgrgdgrgbbssmdsdfddsndnsdnsdnnmqqsspqqmrqqpmmsjmmszzqvqrvrzznnjdndtntfnttgtctqtwwnwswrrthrttsdttlhlvvdzzgqgttnppjpljplpgpvgvqqvppzmmqggtjgtgstslltjltjjgcjcmjmsshvvtppgmmlslqqshqshsllbggfpgffdsdgssncchctcwwtllgqlqblqlqvvmsvmmwnnzppqllsttgmttftvfvjjrzzswzzjvzjzljjchcshcscbbrdbrbcrrnvvtctntvtvbvjvqjqggsrspsprrbgghdghhmwwldldzdttrvrnrfftqtftrrdsszlzvvbtbffftzzrzqrrhjhghhwbhhsjsfsttdjdjnjhjmjpmplplrrdjdcdjdbjblllbqlqdlqlpqptppdhhqmqfqhqhchwwqjqfjqfqhqshsmswsbbvssdspdpsdssstntltrrgnnmttgmmsjjrlrnlrnrnwrwfwlfltlzllcjcmjcjpjhphcpcwppmvmjjzbzvbbfnfcflfddntddbmmmhnnsrnrrvdvnvcvwvcwvwrrqwqccqmmswmmjrjmmwjmjfjhwrtbjzdvlgrjmvzfmhcqsncvlhzzncjlbvcwrdwjmqjcnptqslvfzpsvltgzsvjdsjrppdrmqrbqwhddfhnftfblspsrhtdtjwdnhbcbtlwlvccsfscvczzrrqmwbwbdmwgzqntvflppqvppwrhnvtlsbzqglhsfdgssqzdtjdpwrrhbnbtwhhnmnlwfwlqffjjrndbpwwsvdrhddbjnnqzmtpvvtwbcpndjzlhcfrrdvmljswjzvmfqcdsgqwclqshwrmblszdvsnrpdgnllmlchzdjlrrpndmmgddjqgjqrhwfbwddqdfbvptrmzhtsqfsfswpnvmtswqprjhbzvntgrlzthhnqbtpplqpvcfnpgdtbhqbhflltbbtmmhcwztslmpznttmssclhmnbsbrwlblrbsdfmnpqbwwmsncvzmpqwhzjgcgdrzvglgdtswmstdhrprdjfmqtjlmplbjtzcgnrwpdvpfjjfwjfnnpmdtwtqsgfndngsbmcwjtglqwtfrclbczfcmjtgcwszhzrbcphrhwmhcwghjznzthnwpljjltdlvqtffsrbmwcsvrdmqqggbznnlzbbqtgspqvnjpbdhtzmgttrcwwszwpgdrcnfqtgrgqdrctlzwtdwqppbhnwgldnqltznnfpbfqtgmmwpcqnndbgmrrtgtvnmlfcwsldchjnnqfrhpzwtclrzftsqllgvpqbgmfjdhqjttwcvbpvfqsvhbhhtwnqnbgndbtzhcvgglbhghbzrbrmdllmgfgttqmhtdnwrpwllhnghrjctrbzrcpnjnctvmrlpjhftnfbczrjrnnbqplplcrbngbhvmmvcffmgvbhjzbhcmtwmwgmjmwjvvlqfldswpntjnsjvmdlbzqqlgbwspwvmnwtwjbczmwplrhmjgsppnmtwmvsfwnsgddgwqcvpftcpzrhpldnwmcjgtjmljjbcmjcqdbwczndnjnjgrmtjrqnnjndzqdqpcgdqptdbrqftnwrgqmrzrvsfmmmbpltlncvtgrjfjmvtgwqphczwjhdrdwtfvgztbhrndvpcbgfjfvmrrljwrvcrtdmtjndfnwgcnfrzgsnjpztbwwsbvqfnpjctgrhsflhnzbbsfqbnmtnvrmjzsbjfndvttpvpfjhqntflgbfnzcclcwmhbsgqfjdcgsvrhtstspfzgvgglgddqmclsmzgzgtncdsfmwdvtcsgwvbzjvclwppqdjgfcrcbzcwbdhrnssjbmnmfmwthdrnmlfhqlddwqrdhsdvdcsmcgjsgcmpnhlbnqftpdjswtmpbznlcrhtswgnmwjcdfmljdngzfsmlzjjnzmfzshmztdbdmcqwmlvcrzgpmbjqcghclwvdbrhgvwqchnndftnrtptmctdlhmfjvpzrpccddfpcdwmzqfhnsqzrvwblzfhcjdcjfctczwqrcbjnrpdcbbnsgnlvqqmnsfgsqschjlbzhhsrbvdbfrhvsgrlzwncgwpdbvmblgzbwbcbgqfwmdmgcrbbjfcvmqgztqpptdhwmvmsdqwplpgcjzgqzdrftzhqbltvhrmlrfffcgfpqzwrrbbtlsjgmtbjvtnmhwdpjptjwfwgjgvbfqwmflrrqzlzdcmtlnptdrpcpdnswcfscnndnrfbgwvvncdjgsdpbwptdtvrqlmrhmvvcwblhhzbjdpsbszhrftfbcgwhwrgglnjzqdhcqnvlhgqjhnddvrslhntssptsbhmqwwqqnbvfmcbgpvgjbrttnvlljdbtfplgmbwtcbcdtqdpqqdvhbmpmtszwpzblcfrtznhhtcljtdlhjdbnlhvwgjsmgvrslrfwnmzwlstpgltvrgnpdqztvfnvdhdtwwqdfsmtpbpdclsbnwcgjzchjcsjmvhbjshmjjlpgdzcgbmmchwmcsddsvhsnpqtcpnhqnbvwgwqhtjbqncgwwftnrzsbsjtvqmjzqvvncmncwflcfpcjqgdtbsmjzzsdjfvhnqbgjhmfgjghwscthbfmbndltbqzwpqtmrswvprpmgwqnqpfnmffrpdlpfqmhrthppzvzwbrtjvwvjndsqdlqtbpqwfcttggnjmcqqnmjwfhfjgcvlnmtlgbdvmctzlwbfgnflwtsflgnfbnfbhhdgjctzvvmrhdsmvmmtnqwtszmqcpsbrqrgjfrzctcbzmtdlhwjtfdqbtthdnqcrpwrhcrvjstbhpltvgmvpmvfjstgzjsgzprzcqzqztvvdcnrrqwrhddcrhhncdrlwzwqlnbbzcfmqtnwgfdscmrbwnbldlfrqchzdnlnmwncgrzdclnvcvplgwjsbzmbnnsdrsfhrlssvncnwmcrjdjbjpdtrrvlnbjvspfqbwdpcnnpjzfnmbhcdhlmdgbpvbzmfltzstnznfctcdzhbfsvnfbsjqzmwfllhtrsfghlrpjgrgzgchlwrdmqzbrncsvnwhfqmwjbnvjctzphcsftqsbmwntgvjqhhvwndvmfmjhhhmfdvrlhpvzmmhrbhbddqbdmgqqsvddsswmzqcjmvhztfqpchzpwhdshzjlmbmnsgzqhbnmrshwvtmgmgndtddpfwsjrrjdhncdhtlczdvlbvqplttnzrblthlcffdtfsdtpwzdgbldvnsttvpzmbgnqddrszftcpwrgmfzhjjvghpntmzcttcsnrjnfpqzqqqljhzlrpgwngllqjwnwfcsphqplgbzmfqfgbfsqpsrntszqbcqnhctsnbfshmlbwfflrwwsjwqwfqlgnftdwmctmclwjhjhbsspqldlshbmpbgrftpnbpsqldhrrbdqwfwvfhclrlfdjfmzgmptdjdcsplcspznfjrfhtsjndwpslrdgnllllwqjgznrhswfssdlvdpmwwgmstqbhfmdhtzvzzvhwzbrrvvsl";
        }


    }
}
