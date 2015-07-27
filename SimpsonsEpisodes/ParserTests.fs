module EpisodeParsingTests

open System.IO
open NUnit.Framework
open SimpsonsEpisodes.Parser
open FSharp.Data

let testDataDir = Path.Combine(SimpsonsEpisodes.Parser.dataDirectory, "TestData")

[<TestFixture>]
type EpisodeParsingTests() = 

    [<Test>]
    member this.GetTheCorrectValueForTheSummary() = 
        let fakeFileFunc (thing1, thing2): string = 
            Path.Combine(testDataDir, "BasicParsingTest.html")
        let fakeSeasonNumber = 0
        let fakeEpisodeNumber = 0
        let fakeSummary = SimpsonsEpisodes.Parser.EpisodeSummaryInfo(fakeSeasonNumber, fakeEpisodeNumber, "", "")
        
        let result = SimpsonsEpisodes.Parser.parseEpisodeFile(fakeSummary, fakeFileFunc)
        Assert.That(result.summary, Is.EqualTo("This is a test to get the summary"))
    
    [<Test>]
    member this.GetTheCorrectValueForThePlotWithOnePTag() = 
        let fakeFileFunc (thing1, thing2): string = 
            Path.Combine(testDataDir, "BasicParsingTest.html")
        let fakeSeasonNumber = 0
        let fakeEpisodeNumber = 0
        let fakeSummary = SimpsonsEpisodes.Parser.EpisodeSummaryInfo(fakeSeasonNumber, fakeEpisodeNumber, "", "")
        
        let result = SimpsonsEpisodes.Parser.parseEpisodeFile(fakeSummary, fakeFileFunc)
        Assert.That(result.summary, Is.EqualTo("This is a test to get the plot with only one p tag"))


[<TestFixture>]
type SummaryParsingTests() =
    [<Test>]
    member this.ParseSummary() =
        let elements = @"
            <div id=""mw-content-text"" lang=""en"" dir=""ltr"" class=""mw-content-ltr"">
  <table class=""infobox vevent"" style=""width: 22em; text-align: left; font-size: 88%; line-height: 1.5em""></table>
  <p>
     ""<b>Simpsons Roasting on an Open Fire</b>"", also known as ""<b>The Simpsons Christmas Special</b>"", 
    In the episode, <a href=""/wiki/Homer_Simpson"" title=""Homer Simpson"">Homer Simpson</a> discovers that he will not be getting a Christmas bonus and thus his family has no money to buy Christmas presents after they had to waste money on getting his son <a href=""/wiki/Bart_Simpson"" title=""Bart Simpson"">Bart</a>'s tattoo removed.
  </p>
  <p>
     The episode was written by <a href=""/wiki/Mimi_Pond"" title=""Mimi Pond"">Mimi Pond</a> and directed by <a href=""/wiki/David_Silverman_(animator)"" title=""David Silverman (animator)"">David Silverman</a>. 
  </p><p />
  <div id=""toc"" class=""toc""></div><p />
  <h2>
    <span class=""mw-headline"" id=""Plot"">Plot</span>
    <span class=""mw-editsection"">
      <span class=""mw-editsection-bracket"">[</span><a href=""/w/index.php?title=Simpsons_Roasting_on_an_Open_Fire&action=edit&section=1"" title=""Edit section: Plot"">edit</a><span class=""mw-editsection-bracket"">]</span>
    </span>
  </h2>
  <p>
     After attending the <a href=""/wiki/Springfield_Elementary_School"" title=""Springfield Elementary School"" class=""mw-redirect"">Springfield Elementary School</a><a href=""/wiki/Christmas"" title=""Christmas"">Christmas</a> pageant, the <a href=""/wiki/Simpson_family"" title=""Simpson family"">Simpsons</a> prepare for the holiday season. <a href=""/wiki/Marge_Simpson"" title=""Marge Simpson"">Marge</a> asks <a href=""/wiki/Bart_Simpson"" title=""Bart Simpson"">Bart</a> and <a href=""/wiki/Lisa_Simpson"" title=""Lisa Simpson"">Lisa</a> for their letters to Santa.
  </p>
  <p>
     Homer, discovering there is no money for Christmas presents and not wanting to worry the family, takes a job as a shopping mall <a href=""/wiki/Santa_Claus"" title=""Santa Claus"">Santa Claus</a> at the suggestion of his friend <a href=""/wiki/Barney_Gumble"" title=""Barney Gumble"">Barney Gumble</a>.
  </p>
  <p>
     At Springfield Downs, Homer, inspired by an announcement about a last-minute entry named <a href=""/wiki/Santa%27s_Little_Helper"" title=""Santa's Little Helper"">Santa's Little Helper</a>, bets all his money on the 99-1 long shot. The <a href=""/wiki/Greyhound"" title=""Greyhound"">greyhound</a> finishes last.
  </p>
  <h2>
    <span class=""mw-headline"" id=""Development"">Development</span>
    <span class=""mw-editsection"">
      <span class=""mw-editsection-bracket"">[</span><a href=""/w/index.php?title=Simpsons_Roasting_on_an_Open_Fire&action=edit&section=2"" title=""Edit section: Development"">edit</a><span class=""mw-editsection-bracket"">]</span>
    </span>
  </h2>
  <h3>
    <span class=""mw-headline"" id=""Origin_of_The_Simpsons"">
       Origin of <i>The Simpsons</i>
    </span>
    <span class=""mw-editsection"">
      <span class=""mw-editsection-bracket"">[</span><a href=""/w/index.php?title=Simpsons_Roasting_on_an_Open_Fire&action=edit&section=3"" title=""Edit section: Origin of The Simpsons"">edit</a><span class=""mw-editsection-bracket"">]</span>
    </span>
  </h3>
  <div class=""hatnote relarticle mainarticle"">
     Main articles: <a href=""/wiki/History_of_The_Simpsons"" title=""History of The Simpsons"">History of The Simpsons</a> and <a href=""/wiki/The_Simpsons_shorts"" title=""The Simpsons shorts"">The Simpsons shorts</a>
  </div>
  <div class=""thumb tright"">  </div>
  <p>
    <i>The Simpsons</i> creator <a href=""/wiki/Matt_Groening"" title=""Matt Groening"">Matt Groening</a> conceived of the idea for the Simpsons in the lobby of <a href=""/wiki/James_L._Brooks"" title=""James L. Brooks"">James L. Brooks</a>'s office. Brooks, the producer of the sketch comedy program 
  </p>  
</div>
        "
        let node = HtmlNode.Parse(elements)
        let result = SimpsonsEpisodes.Parser.getSummaryFromContentText(node.[0])
        let expected = "This is a summary"
        Assert.That(result, Is.EqualTo(expected))