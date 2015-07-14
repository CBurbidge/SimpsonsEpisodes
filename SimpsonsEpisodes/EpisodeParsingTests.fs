module EpisodeParsingTests

open System.IO
open NUnit.Framework
open SimpsonsEpisodes.Parser

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