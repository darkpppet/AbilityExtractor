
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        AbilityParser reputevalueParser = new() { AbilityKey = "reputevalue" };
        AbilityParser miraclecirculatorParser = new() { AbilityKey = "miraclecirculator" };
		
        Parallel.Invoke
        (
            () => { reputevalueParser.Parse(); },
            () => { miraclecirculatorParser.Parse(); }
        );
    }
}
