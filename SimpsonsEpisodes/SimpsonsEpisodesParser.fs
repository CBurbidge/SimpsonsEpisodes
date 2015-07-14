module SimpsonsEpisodes.Parser

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

let getEpisodeFileName (seriesNumber, episodeNumber) : string =
    Path.Combine(episodesDataDirectory, "S" + seriesNumber.ToString() + "_E" + episodeNumber.ToString() + ".html")

let fixOBrotherEpisode =
    let oBrotherEpisodeUrl = "https://en.wikipedia.org/wiki/Oh_Brother,_Where_Art_Thou%3F"
    let fileName = getEpisodeFileName(2, 15)
    if File.Exists(fileName) = false || forceDownload || true then
        let episodeHtml = HtmlDocument.Load(oBrotherEpisodeUrl)
        File.WriteAllLines(fileName, [episodeHtml.ToString()])
        Console.WriteLine("Downloaded to file: " + fileName)
    else
        Console.WriteLine("File exists at:" + fileName)
    0

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

                let mutable episodeWikiHref = episodeWikiUrlAnchor.AttributeValue("href")
                
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
    let getSummaryFromContentText(contentText: HtmlNode): string =
        let children = 
            contentText.Elements()
        let mutable add = false
        let mutable hasBeenP = false
        let mutable pElements = []
        for child in children do
            if child.Name() = "table" then
                add <- true
            else
                if child.Name() = "div" && hasBeenP = true then
                    add <- false
                else
                    if child.Name() = "p" then 
                        hasBeenP <- true
                        if add then
                            pElements <- child :: pElements
        if hasBeenP = false then
            raise( Exception("Should have been a p element"))

        pElements 
        |> List.rev
        |> List.map (fun x ->  x.InnerText() )
        |> List.reduce (+)
    
    let getPlotFromContentText(contentText: HtmlNode): string =
        let children = 
            contentText.Elements()
        let mutable add = false
        let mutable pElements = []
        
        for child in children do
            if child.Name() = "h2" then
                let mutable isPlot = false
                let grandchildren = child.Elements()
                for gChild in grandchildren do
                    if gChild.HasId("Plot") then
                        add <- true
                    else
                        if gChild.HasId("Development") 
                        || gChild.HasId("Production") 
                        || gChild.HasId("Controversy") 
                        || gChild.HasId("Reception") 
                        || gChild.HasId("References") 
                        || gChild.HasId("Background") 
                        || gChild.HasId("External_links") 
                        || gChild.HasId("Production_and_allusions") 
                        || gChild.HasId("Production_and_analysis") 
                        || gChild.HasId("Production_and_cultural_references") 
                        || gChild.HasId("Production_and_themes") 
                        || gChild.HasId("Cultural_references") then
                            add <- false
            else
                if child.Name() = "p" && add then
                    pElements <- child :: pElements
                else
                    if child.Name() = "table" && child.HasClass("mbox-small plainlinks sistersitebox") then
                        add <- false
        if add then
            raise(Exception("this should not be true."))

        pElements 
        |> List.rev
        |> List.map (fun x ->  x.InnerText() )
        |> List.reduce (+)
    if summary.seasonNumber = 13 && summary.episodeNumber = 15 then
        0 |> ignore
    
    let fileLocation:string = fileLocationGettingFunc(summary.seasonNumber, summary.episodeNumber)
    let html = HtmlDocument.Load(fileLocation)
    let contentText = 
        html.Descendants["div"]
        |> Seq.filter (fun x -> x.HasAttribute("id", "mw-content-text"))
        |> Seq.head
    let sum = getSummaryFromContentText contentText
    let plot = getPlotFromContentText contentText
    Episode(summary, sum, plot)

let getEpisodes =
    downloadSeasonFilesToDisk
    
    let allEpisodesSummaryInfos: EpisodeSummaryInfo list = getAllEpisodes()
    
    ensureThatEpisodeFilesExist allEpisodesSummaryInfos
    
    fixOBrotherEpisode |> ignore
    
    allEpisodesSummaryInfos
        |> List.map  (fun x -> parseEpisodeFile(x, getEpisodeFileName))