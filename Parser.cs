
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

class AbilityParser
{
    private HtmlDocument htmlDocument = new();
    private HtmlWeb htmlWeb = new();
	
    public string AbilityKey { get; set; }
	
    private string Url { get => @"https://maplestory.nexon.com/Guide/OtherProbability/ability/" + AbilityKey; }
    private string JsonPath { get => @"extracted/" + AbilityKey; }
	
    private Dictionary<string, string> abilityKeyDic = new() { ["reputevalue"] = "명성치", ["miraclecirculator"] = "미라클 서큘레이터" };
	
    private string MakeHtmlStringNeat(string htmlString) => htmlString.Split("<br>").Select(x => x.Replace("\r\n", "").Replace("                                    ", " ").Trim()).Aggregate((partialPhrase, word) => $"{partialPhrase}, {word}");
	
    public void Parse()
    {
        Console.WriteLine($"{AbilityKey, 18}: Open Url: {Url}");
        htmlDocument = htmlWeb.Load(Url);

        Console.WriteLine($"{AbilityKey, 18}: Extract Ability Options");
        HtmlNodeCollection trNodes;

        trNodes = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='container']/div/div[1]/table[3]/tbody").SelectNodes("tr");

        AbilityOptionTable tableRare = new() { AbilityName = abilityKeyDic[AbilityKey], GradeName = "레어", Options = new() };
        AbilityOptionTable tableEpic = new() { AbilityName = abilityKeyDic[AbilityKey], GradeName = "에픽", Options = new() };
        AbilityOptionTable tableUnique = new() { AbilityName = abilityKeyDic[AbilityKey], GradeName = "유니크", Options = new() };
        AbilityOptionTable tableLegendary = new() { AbilityName = abilityKeyDic[AbilityKey], GradeName = "레전드리", Options = new() };

        foreach (var tr in trNodes)
        {
            HtmlNodeCollection tdNodes = tr.SelectNodes("td");

            tableRare.Options.Add(new AbilityOption() { Name = tdNodes[0].InnerHtml, Probability = tdNodes[1].InnerHtml });
            tableEpic.Options.Add(new AbilityOption() { Name = tdNodes[0].InnerHtml, Probability = tdNodes[2].InnerHtml });
            tableUnique.Options.Add(new AbilityOption() { Name = tdNodes[0].InnerHtml, Probability = tdNodes[3].InnerHtml });
            tableLegendary.Options.Add(new AbilityOption() { Name = tdNodes[0].InnerHtml, Probability = tdNodes[4].InnerHtml });
        }

        Console.WriteLine($"{AbilityKey, 18}: Serialize Ability Options Table");
        tableRare.SerializeThis(JsonPath + "/rare", "options.json");
        tableEpic.SerializeThis(JsonPath + "/epic", "options.json");
        tableUnique.SerializeThis(JsonPath + "/unique", "options.json");
        tableLegendary.SerializeThis(JsonPath + "/legendary", "options.json");

        Console.WriteLine($"{AbilityKey, 18}: Extract Ability Values");
        trNodes = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='container']/div/div[1]/table[4]/tbody").SelectNodes("tr");

        AbilityValueOptionTable valueTableRare = new() { AbilityName = abilityKeyDic[AbilityKey], GradeName = "레어", Options = new() };
        AbilityValueOptionTable valueTableEpic = new() { AbilityName = abilityKeyDic[AbilityKey], GradeName = "에픽", Options = new() };
        AbilityValueOptionTable valueTableUnique = new() { AbilityName = abilityKeyDic[AbilityKey], GradeName = "유니크", Options = new() };
        AbilityValueOptionTable valueTableLegendary = new() { AbilityName = abilityKeyDic[AbilityKey], GradeName = "레전드리", Options = new() };

        if (AbilityKey == "reputevalue")
        {
            AbilityValueOption? tempValueOptionRare = null;
            AbilityValueOption? tempValueOptionEpic = null;
            AbilityValueOption? tempValueOptionUnique = null;
            AbilityValueOption? tempValueOptionLegendary = null;

            foreach (var tr in trNodes)
            {
                HtmlNode thNode = tr.SelectSingleNode("th");
                HtmlNodeCollection tdNodes = tr.SelectNodes("td");

                if (thNode is not null)
                {
                    if (tempValueOptionRare is not null)
                        valueTableRare.Options.Add(tempValueOptionRare);
                    if (tempValueOptionEpic is not null)
                        valueTableEpic.Options.Add(tempValueOptionEpic);
                    if (tempValueOptionUnique is not null)
                        valueTableUnique.Options.Add(tempValueOptionUnique);
                    if (tempValueOptionLegendary is not null)
                        valueTableLegendary.Options.Add(tempValueOptionLegendary);

                    tempValueOptionRare = new() { Name = MakeHtmlStringNeat(thNode.InnerHtml), ValueOptions = new() };
                    tempValueOptionEpic = new() { Name = MakeHtmlStringNeat(thNode.InnerHtml), ValueOptions = new() };
                    tempValueOptionUnique = new() { Name = MakeHtmlStringNeat(thNode.InnerHtml), ValueOptions = new() };
                    tempValueOptionLegendary = new() { Name = MakeHtmlStringNeat(thNode.InnerHtml), ValueOptions = new() };
                }

                tempValueOptionRare?.ValueOptions.Add(new ValueOption() { Value = tdNodes[1].InnerHtml, Probability = tdNodes[0].InnerHtml });
                tempValueOptionEpic?.ValueOptions.Add(new ValueOption() { Value = tdNodes[2].InnerHtml, Probability = tdNodes[0].InnerHtml });
                tempValueOptionUnique?.ValueOptions.Add(new ValueOption() { Value = tdNodes[3].InnerHtml, Probability = tdNodes[0].InnerHtml });
                tempValueOptionLegendary?.ValueOptions.Add(new ValueOption() { Value = tdNodes[4].InnerHtml, Probability = tdNodes[0].InnerHtml });
            }
        }
        if (AbilityKey == "miraclecirculator")
        {
            foreach (var tr in trNodes)
            {
                HtmlNodeCollection tdNodes = tr.SelectNodes("td");

                AbilityValueOption tempValueOptionRare = new() { Name = MakeHtmlStringNeat(tdNodes[0].InnerHtml), ValueOptions = new() };
                AbilityValueOption tempValueOptionEpic = new() { Name = MakeHtmlStringNeat(tdNodes[0].InnerHtml), ValueOptions = new() };
                AbilityValueOption tempValueOptionUnique = new() { Name = MakeHtmlStringNeat(tdNodes[0].InnerHtml), ValueOptions = new() };
                AbilityValueOption tempValueOptionLegendary = new() { Name = MakeHtmlStringNeat(tdNodes[0].InnerHtml), ValueOptions = new() };

                tempValueOptionRare.ValueOptions.Add(new() { Value = tdNodes[2].InnerHtml, Probability = tdNodes[1].InnerHtml });
                tempValueOptionEpic.ValueOptions.Add(new() { Value = tdNodes[3].InnerHtml, Probability = tdNodes[1].InnerHtml });
                tempValueOptionUnique.ValueOptions.Add(new() { Value = tdNodes[4].InnerHtml, Probability = tdNodes[1].InnerHtml });
                tempValueOptionLegendary.ValueOptions.Add(new() { Value = tdNodes[5].InnerHtml, Probability = tdNodes[1].InnerHtml });

                valueTableRare.Options.Add(tempValueOptionRare);
                valueTableEpic.Options.Add(tempValueOptionEpic);
                valueTableUnique.Options.Add(tempValueOptionUnique);
                valueTableLegendary.Options.Add(tempValueOptionLegendary);
            }
        }

        Console.WriteLine($"{AbilityKey, 18}: Serialize Ability Values Table");
        valueTableRare.SerializeThis(JsonPath + "/rare", "optionvalues.json");
        valueTableEpic.SerializeThis(JsonPath + "/epic", "optionvalues.json");
        valueTableUnique.SerializeThis(JsonPath + "/unique", "optionvalues.json");
        valueTableLegendary.SerializeThis(JsonPath + "/legendary", "optionvalues.json");
    }
}
