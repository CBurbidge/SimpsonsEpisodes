module SimpsonsEpisodes.Analyser

open System
open System.IO
open System.Web
open FSharp.Configuration
open FSharp.Data
open SimpsonsEpisodes.Parser

type WordFrequency(word:string, frequency: decimal) =
    member this.word = word
    member this.frequency = frequency

type Result(combinedText:string) =
    let getNameFromText (name:string, text:string) =
        let words = text.Split(null)
        let countOfName = words
                          |> Seq.filter (fun word -> 
                              word = name
                          )
                          |> Seq.length
        let totalWords = words.Length
        let ratio = (decimal)countOfName / (decimal)totalWords
        WordFrequency(name, ratio)

    member this.Bart = getNameFromText("bart", combinedText)

/// The description comes from the season page description of the episode and the summary and plot come from each individual episode page.
type SimpsonsEpisode(seasonNumber: int, episodeNumber:int, description:string, summary: string, plot: string) =
    let cleanText(text:string):string = 
        text.ToLower()
    member this.seasonNumber = seasonNumber
    member this.episodeNumber = episodeNumber
    member this.description = description
    member this.summary = summary
    member this.plot = plot
    member this.combinedText = cleanText(description + " " + summary + " " + plot)
    member this.result = Result(this.combinedText)

type SimpsonsSeason(seasonNumber: int, episodes: SimpsonsEpisode list) =
    member this.seasonNumber = seasonNumber
    member this.episodes = episodes
    member this.combinedText:string = this.episodes
                                      |> List.map (fun (e:SimpsonsEpisode) ->
                                          e.combinedText + " "
                                      )
                                      |> List.reduce (+)
    member this.result = Result(this.combinedText)

type SimpsonsFilmography(seasons: SimpsonsSeason list) =
    member this.seasons = seasons

let analyseEpisodes(allEpisodes: Episode list): SimpsonsFilmography =
    
    let simpsonsEpisodes = allEpisodes
                          |> List.map (fun (x:Episode) -> 
                          SimpsonsEpisode(x.summaryInfo.seasonNumber,
                              x.summaryInfo.episodeNumber,
                              x.summaryInfo.description,
                              x.summary, x.plot)
                          )
    let seasonNumbers = simpsonsEpisodes
                        |> List.map (fun x -> x.seasonNumber)
                        |> Seq.distinct

    let seasons =  seasonNumbers 
                |> Seq.map (fun seasonNumber ->
                    let episodes = simpsonsEpisodes
                                    |> List.filter (fun episode -> 
                                        episode.seasonNumber = seasonNumber
                                    )
                    SimpsonsSeason(seasonNumber, episodes)
                )
    let film = SimpsonsFilmography(Seq.toList (seasons))
    film