open System
open System.IO
open FSharp.Configuration
open FSharp.Data

//type Settings = AppSettings<"app.config">

let dataDirectory = "F:\Repos\SimpsonsEpisodes\Data"
let episodeUrlStart = "http://en.wikipedia.org/wiki/The_Simpsons_(season_"
let currentNumberOfSeries = 27

let getSeasonFileName = fun (seasonNumber: int) -> 
    Path.Combine( dataDirectory, "Season_" + seasonNumber.ToString() + ".html")

[<EntryPoint>]
let main argv = 
    // Download files to disk
    for seriesNumber in 1 .. currentNumberOfSeries do
        if File.Exists(getSeasonFileName(seriesNumber)) then
            Console.WriteLine("File exists at:" + getSeasonFileName(seriesNumber))
        else
            let seasonUrl = episodeUrlStart + seriesNumber.ToString() + ")"
            Console.WriteLine("Url for season is: " + seasonUrl)
            let seasonHtml = HtmlDocument.Load(seasonUrl)
            File.WriteAllLines(getSeasonFileName(seriesNumber), [seasonHtml.ToString()])
            Console.WriteLine("Downloaded to file: " + getSeasonFileName(seriesNumber))
//    let episodes = HtmlDocument.Load(episodesUrl)
//    let trElements = episodes.Descendants["tr"]
//    // For some reason the episodes are in table rows with a 'vevent' class
//    let episodeRowClass = "vevent"
//    let episodeRows: seq<HtmlNode> = 
//        trElements
//        |> Seq.filter(fun (e: HtmlNode) -> 
//            e.HasAttribute("class", episodeRowClass) )

    Console.ReadKey() |> ignore
    0 // return an integer exit code
