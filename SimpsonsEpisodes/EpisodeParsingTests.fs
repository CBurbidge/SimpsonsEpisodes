module EpisodeParsingTests

open System.IO
open NUnit.Framework
open SimpsonsEpisodes

let testDataDir = Path.Combine(SimpsonsEpisodes.dataDirectory, "TestData")

[<TestFixture>]
type EpisodeParsingTests() = 

    [<Test>]
    member this.GetTheCorrectValueForTheSummary() = 
        let fakeFileFunc (thing1, thing2): string = 
            Path.Combine(testDataDir, "BasicParsingTest.html")
        let fakeSeasonNumber = 0
        let fakeEpisodeNumber = 0
        let fakeSummary = SimpsonsEpisodes.EpisodeSummaryInfo(fakeSeasonNumber, fakeEpisodeNumber, "", "")
        
        let result = SimpsonsEpisodes.parseEpisodeFile(fakeSummary, fakeFileFunc)
        Assert.That(result.summary, Is.EqualTo("This is a test to get the summary"))
    
    [<Test>]
    member this.GetTheCorrectValueForThePlotWithOnePTag() = 
        let fakeFileFunc (thing1, thing2): string = 
            Path.Combine(testDataDir, "BasicParsingTest.html")
        let fakeSeasonNumber = 0
        let fakeEpisodeNumber = 0
        let fakeSummary = SimpsonsEpisodes.EpisodeSummaryInfo(fakeSeasonNumber, fakeEpisodeNumber, "", "")
        
        let result = SimpsonsEpisodes.parseEpisodeFile(fakeSummary, fakeFileFunc)
        Assert.That(result.summary, Is.EqualTo("This is a test to get the plot with only one p tag"))