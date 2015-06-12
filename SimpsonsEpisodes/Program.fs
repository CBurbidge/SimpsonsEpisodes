open System
open System.IO
open FSharp.Configuration
open FSharp.Data

//type Settings = AppSettings<"app.config">

let dataDirectory = "F:\Repos\SimpsonsEpisodes\Data"
let seasonsDataDirectory = Path.Combine(dataDirectory, "Seasons")
let episodeUrlStart = "http://en.wikipedia.org/wiki/The_Simpsons_(season_"
let currentNumberOfSeries = 26

let getSeasonFileName = fun (seasonNumber: int) -> 
    Path.Combine( seasonsDataDirectory, "Season_" + seasonNumber.ToString() + ".html")

let downloadSeasonFilesToDisk =
    if Directory.Exists(dataDirectory) = false then
        Directory.CreateDirectory(dataDirectory) |> ignore

    if Directory.Exists(seasonsDataDirectory) = false then
        Directory.CreateDirectory(seasonsDataDirectory) |> ignore

    Console.WriteLine("Downloading Files if need be")
    for seriesNumber in 1 .. currentNumberOfSeries do
        let seasonFileName = getSeasonFileName(seriesNumber)
        if File.Exists(getSeasonFileName(seriesNumber)) then
            Console.WriteLine("File exists at:" + seasonFileName)
        else
            let seasonUrl = episodeUrlStart + seriesNumber.ToString() + ")"
            Console.WriteLine("Url for season is: " + seasonUrl)
            let seasonHtml = HtmlDocument.Load(seasonUrl)
            File.WriteAllLines(seasonFileName, [seasonHtml.ToString()])
            Console.WriteLine("Downloaded to file: " + seasonFileName)

[<EntryPoint>]
let main argv = 
    
    downloadSeasonFilesToDisk

    for seriesNumber in 1 .. currentNumberOfSeries do
        let seasonHtml = HtmlDocument.Load(getSeasonFileName(seriesNumber))
        let tables = seasonHtml.Descendants["table"]
        // Episodes are shown in a table with the classes wikitable plainrowheaders for some reason
        // Season 27 has two tables like this, but it isn't out yet so not going to worry about it 
        let episodeTableClass = "wikitable plainrowheaders"
        let episodeTables = 
            tables
            |> Seq.filter (fun (t: HtmlNode) -> 
                t.HasClass(episodeTableClass)        
            )
        if Seq.length episodeTables <> 1 then
            raise (new Exception("Can't find a episodes table in the season"))

        let episodeTable =  Seq.head episodeTables        
        
        Console.WriteLine("Loaded File")
        
    Console.ReadKey() |> ignore
    0 // return an integer exit code
