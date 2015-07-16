module SimpsonsEpisodes.Aggregator

open System
open System.IO
open System.Web
open FSharp.Configuration
open FSharp.Data
open SimpsonsEpisodes.Parser

/// The description comes from the season page description of the episode and the summary and plot come from each individual episode page.
type SimpsonsEpisode(seasonNumber: int, episodeNumber:int, description:string, summary: string, plot: string) =
    member this.seasonNumber = seasonNumber
    member this.episodeNumber = episodeNumber
    member this.description = description
    member this.summary = summary
    member this.plot = plot

type SimpsonsSeason(seasonNumber: int, episodes: SimpsonsEpisode list) =
    member this.seasonNumber = seasonNumber
    member this.episodes = episodes

type SimpsonsFilmography(seasons: SimpsonsSeason list) =
    member this.seasons = seasons

let getFilmography(allEpisodes: Episode list): SimpsonsFilmography =
    
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