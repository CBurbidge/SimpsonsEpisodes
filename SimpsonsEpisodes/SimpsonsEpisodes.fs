module SimpsonsEpisodes

open System
open System.IO
open System.Web
open FSharp.Configuration
open FSharp.Data

let debugDir = Environment.CurrentDirectory
let binDir = Directory.GetParent(debugDir)
let simpsonsEpisodesCodeDir = Directory.GetParent(binDir.FullName)
let simpsonsEpisodesRepoDir = Directory.GetParent(simpsonsEpisodesCodeDir.FullName)
let dataDirectory = Path.Combine(simpsonsEpisodesRepoDir.FullName, "Data")
let seasonsDataDirectory = Path.Combine(dataDirectory, "Seasons")
let episodesDataDirectory = Path.Combine(dataDirectory, "Episodes")

let wikiStart = "http://en.wikipedia.org"
let episodeUrlStart = wikiStart + "/wiki/The_Simpsons_(season_"
let currentNumberOfSeries = 26

let forceDownload = false

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
        if File.Exists(getSeasonFileName(seriesNumber)) = false || forceDownload then
            let seasonUrl = episodeUrlStart + seriesNumber.ToString() + ")"
            Console.WriteLine("Url for season is: " + seasonUrl)
            let seasonHtml = HtmlDocument.Load(seasonUrl)
            File.WriteAllLines(seasonFileName, [seasonHtml.ToString()])
            Console.WriteLine("Downloaded to file: " + seasonFileName)
        else
            Console.WriteLine("File exists at:" + seasonFileName)

let getEpisodeTableHtmlForSeason seasonNumber: HtmlNode =
    let seasonHtml = HtmlDocument.Load(getSeasonFileName(seasonNumber))
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
    Seq.head episodeTables

type EpisodeSummaryInfo(seasonNumber: int, episodeNumber:int, wikiUrlSuffix:string, description: string) =
    member this.seasonNumber = seasonNumber
    member this.episodeNumber = episodeNumber
    member this.wikiUrlSuffix = wikiUrlSuffix
    member this.description = description

let getAllEpisodes() :EpisodeSummaryInfo list = 
    let rec getSeasonEpisodesRec(seriesNumber: int, acc): EpisodeSummaryInfo list =
        let rec extractEpisodes(seasonNumber: int, numberOfEpisodes: int, infosAndDescriptionsList: List<HtmlNode>, acc: EpisodeSummaryInfo list): EpisodeSummaryInfo list =
            if numberOfEpisodes = 0 then
                acc
            else
                let infoElement = infosAndDescriptionsList.[numberOfEpisodes * 2 - 2]
                if infoElement.HasAttribute("class", "vevent") = false then
                    raise(Exception("This is not an info row when it should be!"))

                let episodeWikiUrlAnchor = 
                    infoElement.Elements().[2].Descendants["a"]
                    |> Seq.head
                let episodeWikiHref = episodeWikiUrlAnchor.AttributeValue("href")
            
                let descriptionRowElement = infosAndDescriptionsList.[numberOfEpisodes * 2 - 1]
                let descriptionElement = descriptionRowElement.Descendants("td") |> Seq.head
                if descriptionElement.HasAttribute("class", "description") = false then
                    raise(Exception("This is not a description row when it should be!"))
                let descText = descriptionElement.InnerText
                let descString = descriptionElement.ToString()
                        
                let oneLessThanInput = numberOfEpisodes - 1
                extractEpisodes(seasonNumber, oneLessThanInput, infosAndDescriptionsList, (EpisodeSummaryInfo(seasonNumber, numberOfEpisodes, episodeWikiHref, descString) :: acc))
        if seriesNumber = 0 then
            acc
        else 
            let episodeTableHtml = getEpisodeTableHtmlForSeason seriesNumber
            // Episode tables have a header then pairs of informations and descriptions.
            let trElements = episodeTableHtml.Descendants["tr"]
        
            let header = trElements |> Seq.head
            let infoAndDescriptions =  trElements |> Seq.filter (fun (a) -> a <> header )
            let infosAndDescriptionsList = infoAndDescriptions |> Seq.toList
            let numberOfInfosAndDecriptions = (List.length infosAndDescriptionsList)
            if numberOfInfosAndDecriptions % 2 <> 0 then
                raise (Exception("This should be an even number!"))
        
            let numberOfEpisodes = numberOfInfosAndDecriptions / 2
            let oneLessThanSeason = seriesNumber - 1
                
            getSeasonEpisodesRec(oneLessThanSeason, extractEpisodes(seriesNumber, numberOfEpisodes, infosAndDescriptionsList, acc))
                
    getSeasonEpisodesRec(currentNumberOfSeries, [])

let getEpisodeFileName (seriesNumber, episodeNumber) : string =
    Path.Combine(episodesDataDirectory, "S" + seriesNumber.ToString() + "_E" + episodeNumber.ToString() + ".html")

let ensureThatEpisodeFilesExist (allEpisodes: EpisodeSummaryInfo list) = 
    if Directory.Exists(episodesDataDirectory) = false then
        Directory.CreateDirectory(episodesDataDirectory) |> ignore

    for episode in allEpisodes do
        let decodedUrlSuffix: string = HttpUtility.UrlDecode(episode.wikiUrlSuffix)
        let downloadUrl = wikiStart + decodedUrlSuffix
        Console.Write(downloadUrl)
        let fileName:string = getEpisodeFileName(episode.seasonNumber, episode.episodeNumber)
        if File.Exists(fileName) = false || forceDownload then
            Console.WriteLine("File doesnt exist " + fileName)
            let html = HtmlDocument.Load(downloadUrl)
            File.WriteAllLines(fileName, [html.ToString()])
        else
            Console.WriteLine("File exists" + fileName)
        0 |> ignore
    
type Episode(summaryInfo: EpisodeSummaryInfo, summary: string, plot: string) =
    member this.summaryInfo = summaryInfo
    member this.summary = summary
    member this.plot = plot

let parseEpisodeFile (summary:EpisodeSummaryInfo, fileLocationGettingFunc): Episode =
    let fileLocation:string = fileLocationGettingFunc(summary.seasonNumber, summary.episodeNumber)
    
    Episode(summary, "", "")



    